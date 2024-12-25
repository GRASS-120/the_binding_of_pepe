using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemIconTemplate : MonoBehaviour {
    [SerializeField] private Image icon;

    public void Init(PickupItem pickupItem) {
        icon.sprite = pickupItem.sprite;
    }
}
