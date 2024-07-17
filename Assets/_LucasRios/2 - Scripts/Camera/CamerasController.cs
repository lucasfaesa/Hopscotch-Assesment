using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CamerasController : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private CameraEventsChannelSO cameraEventsChannel;
    [SerializeField] private MissileEventChannelSO missileEventChannel;
    [Space] 
    [SerializeField] private Toggle cinematicToggle;
    [Space]
    [SerializeField] private CinemachineVirtualCamera defaultCamera;
    [SerializeField] private CinemachineVirtualCamera cinematicModeCamera;
    [Space]
    [SerializeField] private NoiseSettings SIXDCameraShake;
    [SerializeField] private NoiseSettings SIXDWobbleShake;
    [Space] 
    [SerializeField] private List<Transform> missilesTransform;
    [SerializeField] private List<Transform> targetsTransform;

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
        cameraEventsChannel.Reset();
        
        _defaultCameraNoiseComponent = defaultCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cinematicCameraNoiseComponent = cinematicModeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ChangeDefaultCameraPos(int pos)
    {
        SetCinematicMode(false);
        
        switch (pos)
        {
            case 0:
                defaultCamera.transform.localPosition = new Vector3(4.044125f,4.694639f,19.02609f);
                defaultCamera.transform.localRotation = Quaternion.Euler(new Vector3(13.579f,193.751f,0f));
                break;
            case 1:
                defaultCamera.transform.localPosition = new Vector3(22.52535f,14.54672f,22.22963f);
                defaultCamera.transform.localRotation = Quaternion.Euler(new Vector3(16.329f,215.685f,0f));
                break;
            case 2:
                defaultCamera.transform.localPosition = new Vector3(2.45992f, 2.255448f, -78.59266f);
                defaultCamera.transform.localRotation = Quaternion.Euler(new Vector3(-7.047f, -12.994f,0f));
                break;
        }
    }

    public void SetCinematicMode(bool status)
    {
        defaultCamera.Priority = status ? 0 : 1;
        cinematicModeCamera.Priority = status ? 1 : 0;
        cameraEventsChannel.InCinematicMode = status;
        //some redundancy, this method can be called outside the toggle event
        cinematicToggle.SetIsOnWithoutNotify(status);
    }

    private void SetCinematicFollowAndTarget(int missileId)
    {
        cinematicModeCamera.Follow = missilesTransform[missileId];
        cinematicModeCamera.m_LookAt = targetsTransform[missileId];
    }
    
    private void DoCameraShake(int _, GlobalEnums.ExplosionIntensity explosionIntensity)
    {
        //if()
        if(_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);
        
        switch (explosionIntensity)
        {
            case GlobalEnums.ExplosionIntensity.SMALL:
                _shakeRoutine = StartCoroutine(ShakeCameraRoutine(0.5f, SIXDCameraShake, 0.26f, 0.1f));
                break;
            case GlobalEnums.ExplosionIntensity.MEDIUM:
                _shakeRoutine = StartCoroutine(ShakeCameraRoutine(1.6f, SIXDCameraShake, 1.17f, 0.1f));
                break;
            case GlobalEnums.ExplosionIntensity.BIG:
                _shakeRoutine = StartCoroutine(ShakeCameraRoutine(1.9f, SIXDWobbleShake, 3.88f, 0.32f));
                break;
        }
    }

    private IEnumerator ShakeCameraRoutine(float duration, NoiseSettings noise, float amplitude, float frequency)
    {

        _defaultCameraNoiseComponent.m_NoiseProfile = noise;
        _defaultCameraNoiseComponent.m_AmplitudeGain = amplitude;
        _defaultCameraNoiseComponent.m_FrequencyGain = frequency;
        
        yield return new WaitForSeconds(duration);
        _defaultCameraNoiseComponent.m_NoiseProfile = null;
    }
    
}
