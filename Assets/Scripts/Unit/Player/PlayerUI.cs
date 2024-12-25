using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private InfoScreen _infoScreen;
    [SerializeField] private HUD _hud;
    [SerializeField] private Player _player;

    private bool _isInfoScreenShown = false;
    
    void Start() {
        _inputManager.OnShowInfo += OnShowInfo_Callback;
    }

    private void OnShowInfo_Callback(object sender, EventArgs e) {
        _isInfoScreenShown = !_isInfoScreenShown;
        _infoScreen.gameObject.SetActive(_isInfoScreenShown);
    }
}
