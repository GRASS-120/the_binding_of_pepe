using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// IDamagable у всех, а вот pickup - только игрок (у равгов будет баф рандомный)
// IDamagable сделать как визитор?
public abstract class Unit : MonoBehaviour {
    [SerializeField, Required] BaseStats baseStats;
    public Stats Stats { get; private set; }

    protected virtual void Awake() {
        Stats = new Stats(new StatsMediator(), baseStats);
    }

    protected virtual void Start() {
        Stats.Mediator.OnAddModifier += OnAddModifier_Callback;
        Stats.Mediator.OnDisposeModifier += OnDisposeModifier_Callback; 
    }

    protected virtual void Update() {
        Stats.Mediator.Update(Time.deltaTime);
    }

    protected virtual void OnAddModifier_Callback(object sender, EventArgs e) {
        updateCalculatedStats();
    }

    protected virtual void OnDisposeModifier_Callback(object sender, EventArgs e) {
        updateCalculatedStats();
    }

    public abstract void updateCalculatedStats();
}
