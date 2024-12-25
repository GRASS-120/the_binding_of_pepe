using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// [CreateAssetMenu(menuName = "Scriptable Objects/Stats Upgrade")]
public class OldStatsUpgrade : OldUpgrade {
    public List<OldStats> unitsToUpgrade = new List<OldStats>();
    public Dictionary<OldStat, float> upgradeToApply = new Dictionary<OldStat, float>();
    public bool isPercentUpgrade = false; // ?

    public override void DoUpgrade() {
        foreach (var unitToUpgrade in unitsToUpgrade) {
            unitToUpgrade.UnlockUpgrade(this);
        }
    }
}
