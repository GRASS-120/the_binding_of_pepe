using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// как я понял, если в первой реализации через dict и SO все параметры были только в SO => нужно было фиксить проблемы
// связанные с сохранением и очисткой данных, то здесь нет, так как по сути данные хранятся в классе обычном 
public class Stats {
    readonly StatsMediator mediator;
    public readonly BaseStats baseStats;

    public StatsMediator Mediator => mediator;

    public Stats(StatsMediator mediator, BaseStats baseStats) {
        this.mediator = mediator;
        this.baseStats = baseStats;
    }

    public float GetStat(StatType statType) {
        if (baseStats.stats.TryGetValue(statType, out float value)) {
            var q = new Query(statType, value);
            mediator.PerformQuery(this, q);
            return q.value;
        } else {
            Debug.LogError($"No stat value found for {statType}");
            return 0;
        }
    }

    // public float MoveSpeed {
    //     get {
    //         var q = new Query(StatType.MoveSpeed, baseStats.moveSpeed);
    //         mediator.PerformQuery(this, q);
    //         return q.value;
    //     }
    // }
}