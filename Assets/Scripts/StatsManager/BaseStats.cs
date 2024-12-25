using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// в обще решил оставить через словарь. такая система более универсальная + легче добавить стат
// однако! нужно наверное добавить какой-то шаблон для разных юнитов. кароче потещу пока ее, если что - переделаю, дело 5 минут

public enum StatType {
    HP,
    MoveSpeed,
    AttackSpeed,
    AttackRange,
    Damage,
    DamageMultiplier,
    TearSpeed,
    TearRadius,
}

[CreateAssetMenu(menuName = "Scriptable Objects/Stats")]
public class BaseStats : SerializedScriptableObject {
    public Dictionary<StatType, float> stats = new Dictionary<StatType, float>();
}
