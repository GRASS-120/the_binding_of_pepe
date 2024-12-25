using System;
using System.Collections.Generic;
using UnityEngine;

// ! сделать вместо мыши pointer
// ! слезы по сути нужно процедурно генерировать, а не менять готовые модели
            
public class Player : Unit, IShooter, IPickupVisitable {
    private const float _attackDelay = 1.5f;

    [Header("Entities")]
    [SerializeField] private InputManager inputManager;
    
    private float _localHP;
    private List<PickupItem> _pickupItemList;
    private PickupActive _currentPickupActive;
    private LayerMask _groundMask;
    private CharacterController _characterController;
    private Camera _mainCamera;
    private PlayerTearManager _tearManager;
    private Vector3 _aimDir;
    private bool _isAttacking = false;
    private float _nextAttackTime = 0f;
    private float _attackInterval = 0.5f;

    protected override void Awake() {
        base.Awake();
        _localHP = Stats.GetStat(StatType.HP);
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _groundMask = LayerMask.GetMask(Const.GROUND_LAYER);
        _tearManager = GetComponent<PlayerTearManager>();
        _attackInterval = _attackDelay / Stats.GetStat(StatType.AttackSpeed);  // ! нужен пересчет при изменении стата!
        _pickupItemList = new List<PickupItem>();
    }

    protected override void Start() {
        base.Start();
        inputManager.OnAttack += OnAttack_Callback;
        inputManager.OnAttackCanceled += OnAttackCanceled_Callback;

        Pickup.OnAddPickupItem += HandlePickupItemList;
        Pickup.OnAddPickupActive += HandleCurrentPickupActive;
    }

    protected override void Update() {
        base.Update();
        HandleMovement();
        HandleRotation();
        HandleAttack();
    }

    private void HandleMovement() {
        Vector2 inputDir = inputManager.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

        _characterController.Move(moveDir * Stats.GetStat(StatType.MoveSpeed) * Time.deltaTime);
    }

    // ! ПОНЯТЬ КАК РАБОТАЕТ !
    private void HandleRotation() {
        var (success, position) = GetMouseRaycast();

        if (success) {
            // ? нужно понять вектора... почему вычитаем??? зачем???
            // -> конспект в obsidian: GPO_Ecobot.Векторы
            Vector3 lookDir = position - transform.position;
            lookDir.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            // ? пофиксил траеторию полета просто нормализовав вектор........ как???????
            // -> конспект в obsidian: GPO_Ecobot.Векторы
            _aimDir = lookDir.normalized;  

            if (_aimDir != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.15f);
            }
        }
    }

    private void OnAttack_Callback(object sender, EventArgs e) {
        _isAttacking = true;
    }

    private void OnAttackCanceled_Callback(object sender, EventArgs e) {
        _isAttacking = false;
    }

    private void SetPickupActive(PickupActive newPickupActive) {
        this._currentPickupActive = newPickupActive;
    }

    // ПОЧЕМУ ТАК ПИСАТЬ ЛУЧШЕ ЧЕМ ЧЕРЕЗ КОРУТИНЫ
    // 1. не работает абуз через постоянное кликанье, так как _nextAttackTime не позволит выпустить пули раньше чем через _attackInterval
    // 2. с корутинами ломалась физика...

    public void HandleAttack() {
        if (_isAttacking) {
            if (Time.time >= _nextAttackTime) {
                _tearManager.Shoot();
                _nextAttackTime = Time.time + _attackInterval;
            }
        }
    }

    private void HandlePickupItemList(object sender, EventArgs e) {
        _pickupItemList.Add((PickupItem)sender);
        
    }

    private void HandleCurrentPickupActive(object sender, Pickup.OnAddPickupActiveArgs e) {
        if (this._currentPickupActive == null) {
            SetPickupActive(e.pickupActive);
            // кароче какой-то баг, что я _currentPickupActive = null, когда я его проверяю в OnActivatePickup_Callback,
            // но при этом он меняется + Activate() работает... поэтому сделаю так, чтобы событие OnActivatePickup
            // отрабатывало только когда _currentPickupActive ТОЧНО != null, то есть после SetPickupActive()
            inputManager.OnActivatePickup += OnActivatePickup_Callback;
        }
    }

    private void OnActivatePickup_Callback(object sender, EventArgs e) {
        if (_currentPickupActive.isReadyToUse()) {
            _currentPickupActive.Activate(this);
            _currentPickupActive.SetIsCharged(false);
        } else {
            Debug.Log("no energy");
        }
    }

    private (bool success, Vector3 position) GetMouseRaycast() {
        // из камеры посылается луч, который направлен в место, где сейчас находиться мышка
        Vector2 mousePosition = inputManager.GetMousePosition();
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundMask)) {
            // the raycast hit something, return its position
            return (success: true, position: hitInfo.point);
        } else {
            // the raycast did not hit anything
            return (success: false, position: Vector3.zero);
        }
    }

    public override void updateCalculatedStats() {
        this._attackInterval = _attackDelay / Stats.GetStat(StatType.AttackSpeed);
    }

    // ? вообще можно сделать так, чтобы Player узнавал о Pickup через Accept(), так как visitor - Pickup, который подобрал игрок
    // ? однако такой подход странный как мне кажется, так как
    // 1. Accept() - чисто абстрактная функция, нужная для работы паттерна. она не должна знать конкретику (?)
    // 2. добавлять Pickup в список вещей ДО того, как эта вещь была фактически применена к персонажу - так себе идея.
    // в моей ситуации норм будет работать, но а если внутри Visit есть какие-то проверки? то уже все, поломка
    // => делаю связь через статичное событие в Pickup 
    public void Accept(IPickupVisitor visitor) => visitor.Visit(this);

    public Vector3 GetAimDir() {
        return _aimDir;
    }

    public PickupActive GetCurrentPickupActive() {
        return _currentPickupActive;
    }

    public List<PickupItem> GetPickupItemList() {
        return _pickupItemList;
    }

    public float GetLocalHP() {
        return _localHP;
    }

    // public float GetMaxHP() {
    //     return _localHP;
    // }
}
