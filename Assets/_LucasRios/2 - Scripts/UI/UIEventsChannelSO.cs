using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIEventsChannel", menuName = "ScriptableObjects/UI/UIEventsChannel")]
public class UIEventsChannelSO : ScriptableObject
{
    public event Action<int, bool> OnToggleMissileSettings;

    public void ToggleMissileSettings(int missileId, bool status)
    {
        OnToggleMissileSettings?.Invoke(missileId, status);
    }
    
}
