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
    [Space]
    [SerializeField] private List<ParticleSystem> rocketParticles;
    [Space]
    [SerializeField] private List<ParticleSystem> explosionParticles;
    
    private Vector3 _initialPos;
    private Vector3 _initialRot;
    
    private float _maxTravelTime = 5f;
    
    private Coroutine travelTimeRoutine;

    private bool _exploded;
    
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
        
        rocketParticles.ForEach(x=>x.Stop());
        
        Explode();
    }


    private void OnMissileLaunched(int id)
    {
        if (id == missileData.MissileId && missileData.IsReadyToLaunch)
        {
            missileData.IsReadyToLaunch = false;
               
            rigidBody.AddForce(Vector3.back * missileData.Speed, ForceMode.VelocityChange);

            if(missileData.Speed is >= 1f and < 3f)
                rocketParticles[0].Play();
            if(missileData.Speed is >= 3f and < 6f)
                rocketParticles[1].Play();
            if(missileData.Speed >= 6f)
                rocketParticles[2].Play();
            
            if(travelTimeRoutine != null)
                StopCoroutine(travelTimeRoutine);

            travelTimeRoutine = StartCoroutine(CountTravelTime());
        }
    }

    private IEnumerator CountTravelTime()
    {
        yield return new WaitForSeconds(_maxTravelTime);

        if (_exploded) yield break;

        OnTriggerEnter(null);
    }

    private void OnMissileReset(int id)
    {
        if (id == missileData.MissileId)
        {
            ResetMissile();
        }
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
        _exploded = true;

        if(travelTimeRoutine != null)
            StopCoroutine(travelTimeRoutine);

        for (int i = 0; i < explosionParticles.Count; i++)
        {
            var shape = explosionParticles[i].shape;
            shape.radius = missileData.ExplosionRadius;
        }

        switch (missileData.GetExplosionIntensity())
        {
            case GlobalEnums.ExplosionIntensity.SMALL:
                explosionParticles[0].Play();
                missileEventChannel.MissileHitTarget(missileData.MissileId, GlobalEnums.ExplosionIntensity.SMALL);
                break;
            case GlobalEnums.ExplosionIntensity.MEDIUM:
                explosionParticles[1].Play();
                missileEventChannel.MissileHitTarget(missileData.MissileId, GlobalEnums.ExplosionIntensity.MEDIUM);
                break;
            case GlobalEnums.ExplosionIntensity.BIG:
                explosionParticles[2].Play();
                missileEventChannel.MissileHitTarget(missileData.MissileId, GlobalEnums.ExplosionIntensity.BIG);
                break;
        }
        
        
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
                    missileData.UpwardsModifier, ForceMode.Impulse);
            }
        }
    }
    
    private void ResetMissile()
    {
        _exploded = false;
        
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        
        this.transform.position = _initialPos;
        this.transform.rotation = Quaternion.Euler(_initialRot);
        
        missileData.IsReadyToLaunch = true;
        
        rocketParticles.ForEach(x=>x.Stop());
        
        childObject.SetActive(true);
    }
}
