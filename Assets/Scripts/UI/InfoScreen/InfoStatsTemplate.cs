using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ? а что если попробовать обновлять не весь список, а конкретный item???

public class InfoStatsTemplate : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI statText;

    public void Init(StatType statType, float value) {
        string newText = $"{statType}: {value}";
        
        // statText.gameObject.SetActive(true);
        statText.text = newText;
    }
}
