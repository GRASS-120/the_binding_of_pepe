using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// !? переделать слезы под isTrigger???? как буд-то бы так лучше будет - зачем им объем?


public class Tear : MonoBehaviour {
    public event EventHandler OnCollisionGround;

    private Rigidbody _rb;
    private LayerMask _groundMask;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _groundMask = LayerMask.GetMask(Const.GROUND_LAYER);
    }

    void OnTriggerEnter(Collider other) {
        LayerMask colliderMask = 1 << other.gameObject.layer;

        if ((_groundMask & colliderMask) != 0) {
            OnCollisionGround?.Invoke(this, EventArgs.Empty);
        } 
    }

    public void Launch(Vector3 spawnPoint, Vector3 aimDir, float tearSpeed, float tearRadius) {
        _rb.isKinematic = false;
        transform.position = spawnPoint;
        _rb.AddForce(aimDir * tearSpeed * tearRadius, ForceMode.Impulse);  // removed .normilized
    }

    public void ResetTear(Vector3 tearPoolPoint) {
        _rb.isKinematic = true;
        transform.position = tearPoolPoint; 
        transform.rotation = Quaternion.identity;
    }

    public void SetOnCollisionGroundHandler(EventHandler handler) {
        OnCollisionGround += handler;
    }
}
