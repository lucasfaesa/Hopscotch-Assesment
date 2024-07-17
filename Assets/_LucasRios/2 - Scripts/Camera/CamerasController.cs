using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private MissileEventChannelSO missileEventChannel; 
    [Space]
    [SerializeField] private CinemachineVirtualCamera defaultCamera;
    [SerializeField] private CinemachineVirtualCamera cinematicModeCamera;
    [Space] 
    [SerializeField] private List<Transform> missilesTransform;
    [SerializeField] private List<Transform> targetsTransform;
    [Space]
    [SerializeField] private NoiseSettings SIXDCameraShake;

    private Coroutine _shakeRoutine;
    private CinemachineBasicMultiChannelPerlin _defaultCameraNoiseComponent;
    private CinemachineBasicMultiChannelPerlin _cinematicCameraNoiseComponent;
    
    private void OnEnable()
    {
        missileEventChannel.OnMissileLaunched += SetCinematicFollowAndTarget;
        missileEventChannel.OnMissileHitTarget += DoCameraShake;
    }

    private void OnDisable()
    {
        missileEventChannel.OnMissileLaunched -= SetCinematicFollowAndTarget;
        missileEventChannel.OnMissileHitTarget -= DoCameraShake;
    }

    private void Start()
    {
        _defaultCameraNoiseComponent = defaultCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cinematicCameraNoiseComponent = cinematicModeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetCinematicMode(bool status)
    {
        defaultCamera.Priority = status ? 0 : 1;
        cinematicModeCamera.Priority = status ? 1 : 0;
    }

    private void SetCinematicFollowAndTarget(int missileId)
    {
        cinematicModeCamera.Follow = missilesTransform[missileId];
        cinematicModeCamera.m_LookAt = targetsTransform[missileId];
    }
    
    private void DoCameraShake(int _, GlobalEnums.ExplosionIntensity explosionIntensity)
    {
        if(_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);
        
        switch (explosionIntensity)
        {
            case GlobalEnums.ExplosionIntensity.SMALL:
                _shakeRoutine = StartCoroutine(ShakeCameraRoutine(0.3f, SIXDCameraShake, 0.26f, 0.1f));
                break;
            case GlobalEnums.ExplosionIntensity.MEDIUM:
                
                break;
            case GlobalEnums.ExplosionIntensity.BIG:
                
                break;
        }
    }

    private IEnumerator ShakeCameraRoutine(float duration, NoiseSettings noise, float amplitude, float frequency)
    {

        _defaultCameraNoiseComponent.m_NoiseProfile = noise;
        _defaultCameraNoiseComponent.m_AmplitudeGain = amplitude;
        _defaultCameraNoiseComponent.m_FrequencyGain = frequency;
        
        _cinematicCameraNoiseComponent.m_NoiseProfile = noise;
        _cinematicCameraNoiseComponent.m_AmplitudeGain = amplitude;
        _cinematicCameraNoiseComponent.m_FrequencyGain = frequency;
        
        yield return new WaitForSeconds(duration);
        _defaultCameraNoiseComponent.m_NoiseProfile = null;
        _cinematicCameraNoiseComponent.m_NoiseProfile = null;
    }
    
}
