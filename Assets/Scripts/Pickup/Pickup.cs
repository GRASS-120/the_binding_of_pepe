using System;
using UnityEngine;

//! план действий
// 1. сделать ui
// 1.1 отображение шмоток
// 1.2 отображение статов и прочей инфы, полезной для тестов и разработки на f3 +
// 1.3 вместо мыши указатель
// !!! сделать прозрачность нижней стены (есть видос на ноуте вроде)
// 2. звуки ходьбы, слез (запуск и лопанье) + музыка на фон (утилита для микса звуков есть в прошлом проекте)
// 2.1 визуальные эфекты слез
// 3. система здоровья и урона
// 3.1 хп + ui + (анимаци потери хп и хила?) + звуки (сделать сделать редкий звук потери урона - стон? - типа ржомба)
// 3.2 урон (пока сделать от шипов)
// 4. анимация шмоток, когда они лежат (левитация + прокрутка)
//---
// 5. враги
// 6. статус эффекты


// визуал - наверное врагами и гг сделаю обычных монстров из пака с монстрами, похожих на покемонов. впадлу пепе делать + они лучше вписываются

// сделать dev интерфейс на f3 - отображение статов, фпс и тп


// visitor pattern
public abstract class Pickup : MonoBehaviour, IPickupVisitor {
    //чтобы всегда была возможность получать доступ к ивенту, нужно сделать его статичным
    // иначе я хз как в Player получить доступ к добавленному Pickup, когда добавление происходит именно в Pickup
    public static event EventHandler OnAddPickupItem;
    public static event EventHandler<OnAddPickupActiveArgs> OnAddPickupActive;
    public class OnAddPickupActiveArgs {
        public PickupActive pickupActive;
    }

    public string pickupName;
    public string description;
    public Sprite sprite;
    public GameObject prefab;
    public StatModifierSO statModifierSO;

    public void OnTriggerEnter(Collider other) {
        // вообще тут должен быть IVisitable, но так как pickup только Player может поднимать, то можно и так 
        // а вот например для шипов можно сделать ISpikeVisitable
        if (other.TryGetComponent<Player>(out var player)) {   
            player?.Accept(this);  // ? что делает this => принимает эту компоненту в качестве посетителя 
            Destroy(gameObject);
        }    
    }

    public void Visit(Player player) {
        if (this is PickupItem) {
            statModifierSO.ApplyModifierEffect(player);
            OnAddPickupItem?.Invoke(this, EventArgs.Empty);
        } else if (this is PickupActive) {
            OnAddPickupActive?.Invoke(this, new OnAddPickupActiveArgs {
                pickupActive = (PickupActive)this
            });
        }
    }
}