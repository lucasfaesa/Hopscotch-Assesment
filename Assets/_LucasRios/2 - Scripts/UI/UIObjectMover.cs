using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIObjectMover : MonoBehaviour
{
    [SerializeField] private RectTransform objectToMoveRect;
    [SerializeField] private Vector2 targetPosition;
    
    private Vector2 _initialPosition;
    private bool _isOnTargetPos;

    private void Start()
    {
        _initialPosition = objectToMoveRect.anchoredPosition;
    }

    public void Toggle()
    {
        _isOnTargetPos = !_isOnTargetPos;

        if (_isOnTargetPos)
            objectToMoveRect.DOAnchorPos(targetPosition, 0.3f).SetEase(Ease.InOutSine);
        else
            objectToMoveRect.DOAnchorPos(_initialPosition, 0.3f).SetEase(Ease.InOutSine);
        
    }
}
