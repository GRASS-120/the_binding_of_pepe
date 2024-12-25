using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum OldStat {
    hp,
    moveSpeed,
    attackSpeed,
    attackRange,
    damage,
    damageMultiplier,
    tearRadius,
    tearSpeed,
}

// ! как я понимаю, эта система не рассчитана на сохранения. то есть статы храняться в пределах одной игровой сессии. но не факт что так

// [CreateAssetMenu(menuName = "Scriptable Objects/Stats")]
public class OldStats : SerializedScriptableObject {
    // зачем нужны два словаря
    // пример: есть несколько юнитов одного типа, и для них будет использоваться один и тот же СО => статы у них общие, и если
    // изменить стат любой, он измениться у всех.чтобы этого избежать, нужна какая-то локальная копия для статов,
    // которые будут личными у каждого инстанса этого юнита. пример: хп, мана и тп. у игрока это все
    public Dictionary<OldStat, float> instanceStats = new Dictionary<OldStat, float>(); // для статов инстанса (локальные) => делать нужно его копию
    public Dictionary<OldStat, float> stats = new Dictionary<OldStat, float>(); // для общих статов
    private List<OldStatsUpgrade> _appliedUpgrades = new List<OldStatsUpgrade>();

    public float GetStat(OldStat stat) {
        // я так понимаю, здесь подразумевается, что можно оба типа статов прокачивать.

        // если стат из инстанса
        if (instanceStats.TryGetValue(stat, out float instanceValue)) {
            return GetUpgradedValue(stat, instanceValue);
        }

        // если стат статичный
        else if (stats.TryGetValue(stat, out float value)) {
            return GetUpgradedValue(stat, value);
        }

        else {
            Debug.LogError($"No stat value found for {stat} on {this.name}");  // ! мб просто дебаг в консоль
            return 0;
        }
    }

    // применяем улучшение -> добавление его в список улучшений
    public void UnlockUpgrade(OldStatsUpgrade upgrade) {
        // ? нужна ли мне жта проверка? 
        if (!_appliedUpgrades.Contains(upgrade)) {
            _appliedUpgrades.Add(upgrade);
        }
    }

    // как я понимаю каждый раз при обращении применябтся улучшения заново??? или че бляяя
    private float GetUpgradedValue(OldStat stat, float baseValue) {
        foreach (var upgrade in _appliedUpgrades) {
            // ищу улучшение для конкретного стата
            if (!upgrade.upgradeToApply.TryGetValue(stat, out float upgradeValue)) {  // ! upgradeValue оказывается доступен не только внутри if...
                continue;
            }
            if (upgrade.isPercentUpgrade) {
                baseValue *= (upgradeValue / 100f) + 1f;
            }
            else {
                baseValue += upgradeValue;
            }
        }

        return baseValue;
    }

    // ? зачем
    public void ResetAppliedUpgrades() {
        _appliedUpgrades.Clear();
    }
}
