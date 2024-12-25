using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {
    [Header("Entities")]
    [SerializeField] private PlayerUI _playerUI;

    [Header("UI elements")]
    [SerializeField] private Transform _healthBarContainer;
    [SerializeField] private Transform _healthBarFullTemplate;
    [SerializeField] private Transform _healthBarEmptyTemplate;
    [SerializeField] private Transform _pickupActiveSlotContainer;
    [SerializeField] private Transform _pickupActiveTemplate;
    [SerializeField] private Transform _pickupActiveEnergyContainer;
    [SerializeField] private Transform _pickupActiveEnergyItemTemplate;

    void Awake() {

    }

    void Start() {
        
    }
}
