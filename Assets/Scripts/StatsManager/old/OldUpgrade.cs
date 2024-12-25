using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// ? наверное мне так нужно сделать с  Item, а не делать отдельно upgrade

public abstract class OldUpgrade : SerializedScriptableObject {
    public Sprite icon;
    public string upgradeName;
    // public bool isPercentUpgrade
    
    public abstract void DoUpgrade();
}
