using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private MissileEventChannelSO missileEventChannel;
    [Space] 
    [SerializeField] private Camera cameraComponent;
    
    private void OnEnable()
    {
        missileEventChannel.OnMissileHitTarget +=DoCameraShake;
    }

    private void OnDisable()
    {
        missileEventChannel.OnMissileHitTarget -=DoCameraShake;
    }

    private void DoCameraShake(int _, GlobalEnums.ExplosionIntensity explosionIntensity)
    {
        switch (explosionIntensity)
        {
            case GlobalEnums.ExplosionIntensity.SMALL:
                cameraComponent.DOShakePosition(0.3f, 0.03f, 8, 90f, true);
                break;
            case GlobalEnums.ExplosionIntensity.MEDIUM:
                cameraComponent.DOShakePosition(0.6f, 0.06f, 18, 90f, true);
                break;
            case GlobalEnums.ExplosionIntensity.BIG:
                cameraComponent.DOShakePosition(2.7f, 0.08f, 81, 90f, true);
                break;
        }
    }
}
