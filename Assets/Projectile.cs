using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Event Channel")] 
    [SerializeField] private MissileEventChannelSO missileEventChannel;
    [Space]
    [SerializeField] private MissileDataSO missileData;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject childMesh;
    
    private Vector3 _initialPos;
    private Vector3 _initialRot;
    
    private void OnEnable()
    {
        missileEventChannel.OnMissileLaunched += OnMissileLaunched;
        missileEventChannel.OnMissileReset += OnMissileReset;
    }

    private void OnDisable()
    {
        missileEventChannel.OnMissileLaunched -= OnMissileLaunched;
        missileEventChannel.OnMissileReset -= OnMissileReset;
    }

    private void Start()
    {
        _initialPos = this.transform.position;
        _initialRot = this.transform.rotation.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        childMesh.SetActive(false);
    }


    private void OnMissileLaunched(int id)
    {
        if (id == missileData.MissileId && missileData.IsReadyToLaunch)
        {
            missileData.IsReadyToLaunch = false;
            
            rigidBody.AddForce(Vector3.back * missileData.Speed, ForceMode.VelocityChange);
        }
    }

    private void OnMissileReset(int id)
    {
        if (id == missileData.MissileId)
        {
            ResetMissile();
        }
    }

    private void ResetMissile()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        
        this.transform.position = _initialPos;
        this.transform.rotation = Quaternion.Euler(_initialRot);
        
        missileData.IsReadyToLaunch = true;
        
        childMesh.SetActive(true);
    }
}
