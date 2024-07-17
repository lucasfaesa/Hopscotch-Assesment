using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraEventsChannel", menuName = "ScriptableObjects/Camera/CameraEventsChannel")]
public class CameraEventsChannelSO : ScriptableObject
{
    public event Action<bool> OnCinematicModeToggled;

    
    private bool _inCinematicMode;
    public bool InCinematicMode
    {
        get => _inCinematicMode;
        set
        {
             OnCinematicModeToggled?.Invoke(value);
             _inCinematicMode = value;
        }
    }
    
}
