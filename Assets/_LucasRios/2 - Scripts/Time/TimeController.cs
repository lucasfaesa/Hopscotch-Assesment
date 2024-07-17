using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private CameraEventsChannelSO cameraEventsChannel;
    [SerializeField] private MissileEventChannelSO missileEventChannel;

    private float _slowMotionTime = 0.5f;

    private Coroutine _slowMotionRoutine;
    
    private void OnEnable()
    {
        missileEventChannel.OnMissileHitTarget += DoSlowMotion;
    }

    private void OnDisable()
    {
        missileEventChannel.OnMissileHitTarget -= DoSlowMotion;
    }

    private void DoSlowMotion(int _, GlobalEnums.ExplosionIntensity __)
    {
        if (!cameraEventsChannel.InCinematicMode) return;
        
        if(_slowMotionRoutine != null)
            StopCoroutine(_slowMotionRoutine);
        
        //_slowMotionRoutine = StartCoroutine(SlowMotionRoutine());

        Time.timeScale = 0.03f;
        DOTween.To(x => Time.timeScale = x, Time.timeScale, 1f, _slowMotionTime).SetEase(Ease.InOutSine);
    }

    private IEnumerator SlowMotionRoutine()
    {
        yield break;
        /*float elapsedTime = 0;

        Time.timeScale = 0.1f;

        while (elapsedTime < _slowMotionTime)
        {
            elapsedTime += Time.deltaTime;

            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/_slowMotionTime));

            yield return null;
        } */
    }
}
