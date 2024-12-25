using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// вообще shoot у всех shooter'ов одинаковый по идее... и по хорошему 

public class PlayerTearManager : MonoBehaviour {
    private const int TEAR_PRELOAD_COUNT = 7;

    [Header("Entities")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform tearPoolPoint;
    [SerializeField] private Tear tearPrefab;

    private Unit _unit;
    private IShooter _shooter;
    private ObjectPool<Tear> _tearObjectPool;

    void Awake() {
        // ! по сути одно и то же беру... как можно пофиксить? 
        _unit = GetComponent<Unit>();
        _shooter = GetComponent<IShooter>();

        _tearObjectPool = new ObjectPool<Tear>(tearPrefab, tearPoolPoint, TEAR_PRELOAD_COUNT);
    }

    public void Shoot() {
        Vector3 aimDir = _shooter.GetAimDir();
        Tear tear = _tearObjectPool.GetFreeElement();

        // ? так то логично было бы если TearSpeed и прочие Tear параметры были только у шутера...
        tear.Launch(shootPoint.position, aimDir, _unit.Stats.GetStat(StatType.TearSpeed), _unit.Stats.GetStat(StatType.TearRadius));
        
        tear.SetOnCollisionGroundHandler((e, sender) => {
            _tearObjectPool.Remove(tear);
            tear.ResetTear(tearPoolPoint.position);
        });
    }
}
