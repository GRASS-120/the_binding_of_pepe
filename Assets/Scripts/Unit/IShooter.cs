using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// мб сделать в enemy это??? а не, нужно присваивать это тем, кто может именно стрелять - некоторые враги будут просто идти
public interface IShooter {
    public void HandleAttack();
    // public Dictionary<Stat, float> GetLocalStats();
    public Vector3 GetAimDir();
}
