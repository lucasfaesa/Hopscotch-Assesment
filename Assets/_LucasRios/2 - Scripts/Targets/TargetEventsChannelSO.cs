using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetEventChannel", menuName = "ScriptableObjects/Target/TargetEventChannel")]
public class TargetEventsChannelSO : ScriptableObject
{
    public event Action<int> OnResetTarget;

    public void ResetTarget(int targetId)
    {
        OnResetTarget?.Invoke(targetId);
    }
}
