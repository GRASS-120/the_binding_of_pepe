using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreen : MonoBehaviour {
    [Header("Entities")]
    [SerializeField] private Player _player;

    [Header("UI elements")]
    [SerializeField] private Transform _infoStatsContainer;
    [SerializeField] private Transform _infoStatsTemplate;
    [SerializeField] private Transform _itemIconsContainer;
    [SerializeField] private Transform _itemIconTemplate;

    private Stats _stats;
    private List<PickupItem> _pickupItemList;

    void Awake() {
        _pickupItemList = _player.GetPickupItemList();
    }

    void Start() {
        // почему то Info.Awake() отрабатывает раньше чем Player.Awake(), поэтому если делать в Awake(), то state = null
        _stats = _player.Stats;

        _player.Stats.Mediator.OnStatsChange += OnStatsChange_Callback;
        Pickup.OnAddPickupItem += OnAddPickupItem_Callback;

        RenderStats();
        RenderIcons();
    }

    private void OnAddPickupItem_Callback(object sender, EventArgs e) {
        RenderIcons();
    }

    private void OnStatsChange_Callback(object sender, EventArgs e) {
        RenderStats();
    }

    // ?не очень оптимизированно наверное. думаю, нужно переписать будет на object pool???
    private void RenderStats() {
        foreach (Transform child in _infoStatsContainer) {
            if (child == _infoStatsTemplate) continue;  // проверка, чтобы не уничтожить сам шаблон
            Destroy(child.gameObject);
        }

        foreach (StatType statKey in Enum.GetValues(typeof(StatType))) {
            var statValue = _stats.GetStat(statKey);
            var infoStatItem = Instantiate(_infoStatsTemplate, _infoStatsContainer);

            infoStatItem.gameObject.SetActive(true);
            infoStatItem.GetComponent<InfoStatsTemplate>().Init(statKey, statValue);
        }
    }

    private void RenderIcons() {
        foreach (Transform child in _itemIconsContainer) {
            if (child == _itemIconTemplate) continue; 
            Destroy(child.gameObject);
        }

        foreach (var item in _pickupItemList) {
            var itemIconItem = Instantiate(_itemIconTemplate, _itemIconsContainer);

            itemIconItem.gameObject.SetActive(true);
            itemIconItem.GetComponent<ItemIconTemplate>().Init(item);
        }
    }
}
