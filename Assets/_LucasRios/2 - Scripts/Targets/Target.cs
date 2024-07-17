using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("SOs")]
    [SerializeField] private TargetEventsChannelSO targetEventsChannel;
    [Space] 
    [SerializeField] private int targetId;
    [SerializeField] private Rigidbody targetRigidBody;

    private Vector3 _initialPos;
    private Vector3 _initialRot;
    
    private void OnEnable()
    {
        targetEventsChannel.OnResetTarget += ResetTarget;
    }

    private void OnDisable()
    {
        targetEventsChannel.OnResetTarget -= ResetTarget;
    }

    private void Start()
    {
        _initialPos = this.transform.position;
        _initialRot = this.transform.rotation.eulerAngles;
    }

    private void ResetTarget(int id)
    {
        if (id == targetId)
        {
            targetRigidBody.velocity = Vector3.zero;
            targetRigidBody.angularVelocity = Vector3.zero;
            this.transform.position = _initialPos;
            this.transform.rotation = Quaternion.Euler(_initialRot);
        }
    }
}
