using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissileEventChannel", menuName = "ScriptableObjects/Missile/MissileEventChannel")]
public class MissileEventChannelSO : ScriptableObject
{
    public event Action<int> OnMissileLaunched;
    public event Action<int> OnMissileReset;
    public event Action<int, int> OnMissileMeshChanged;
    public event Action<int, int> OnMissileMaterialChanged;

    public void LaunchMissile(int missileId)
    {
        OnMissileLaunched?.Invoke(missileId);
    }

    public void ResetMissile(int missileId)
    {
        OnMissileReset?.Invoke(missileId);
    }

    public void ChangeMissileMesh(int missileId, int value)
    {
        OnMissileMeshChanged?.Invoke(missileId, value);
    }
    
    public void ChangeMissileMaterial(int missileId, int value)
    {
        OnMissileMaterialChanged?.Invoke(missileId, value);
    }
}
