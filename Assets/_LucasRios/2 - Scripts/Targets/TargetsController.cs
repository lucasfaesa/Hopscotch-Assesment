using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsController : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private TargetEventsChannelSO targetEventsChannel;

    public void ResetTarget(int targetId)
    {
        targetEventsChannel.ResetTarget(targetId);
    }

    public void ResetAllTargets()
    {
        targetEventsChannel.ResetTarget(0);
        targetEventsChannel.ResetTarget(1);
        targetEventsChannel.ResetTarget(2);
    }
}
