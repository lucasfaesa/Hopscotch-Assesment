using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private MissileEventChannelSO missileEventChannel;
    [SerializeField] private MissileDataSO missileData;
    [Space]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject childObject;
    [SerializeField] private MeshFilter childMeshFilter;
    [SerializeField] private MeshRenderer childMeshRenderer;
    
    private Vector3 _initialPos;
    private Vector3 _initialRot;
    
    private void OnEnable()
    {
        missileEventChannel.OnMissileLaunched += OnMissileLaunched;
        missileEventChannel.OnMissileReset += OnMissileReset;
        missileEventChannel.OnMissileMeshChanged += ChangeMesh;
        missileEventChannel.OnMissileMaterialChanged += ChangeMaterial;
    }

    private void OnDisable()
    {
        missileEventChannel.OnMissileLaunched -= OnMissileLaunched;
        missileEventChannel.OnMissileReset -= OnMissileReset;
        missileEventChannel.OnMissileMeshChanged -= ChangeMesh;
        missileEventChannel.OnMissileMaterialChanged -= ChangeMaterial;
    }

    private void Start()
    {
        _initialPos = this.transform.position;
        _initialRot = this.transform.rotation.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        childObject.SetActive(false);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        
        Explode();
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
        
        childObject.SetActive(true);
    }

    private void ChangeMesh(int missileId, int meshIndex)
    {
        if (missileId == missileData.MissileId)
        {
            childMeshFilter.mesh = missileData.MeshesList[meshIndex];
            missileData.SelectedMeshIndex = meshIndex;
        }
    }
    
    private void ChangeMaterial(int missileId, int materialIndex)
    {
        if (missileId == missileData.MissileId)
        {
            childMeshRenderer.material = missileData.MaterialsList[materialIndex];
            missileData.SelectedMaterialIndex = materialIndex;
        }
    }

    private void Explode()
    {
        Vector3 explosionPos = transform.position;
        
        Collider[] hitColliders = new Collider[10];
        var size = Physics.OverlapSphereNonAlloc(explosionPos, missileData.ExplosionRadius, hitColliders);
        
        foreach (Collider hit in hitColliders)
        {
            if (!hit)
                return;
            
            if(hit.TryGetComponent(out Rigidbody rb))
            {
                if (rb == rigidBody)
                    continue;
                
                rb.AddExplosionForce(missileData.ExplosionForce, explosionPos,  missileData.ExplosionRadius, 
                    missileData.UpwardsModifier, missileData.ForceMode);
            }
        }
    }
}
