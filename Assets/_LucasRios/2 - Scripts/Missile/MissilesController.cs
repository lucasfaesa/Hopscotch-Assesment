using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissilesController : MonoBehaviour
{
    [Header("Event Channel")]
    [SerializeField] private MissileEventChannelSO missileEventChannel;
    [Space]
    [SerializeField] private List<MissileDataSO> missileData;


    private void OnEnable()
    {
        missileData.ForEach(x=>x.Reset());
    }
    
    public void LaunchMissile(int id)
    {
        if (!missileData[id].IsReadyToLaunch)
            return;
        
        missileEventChannel.LaunchMissile(id);
    }

    public void ResetMissile(int id)
    {
        missileEventChannel.ResetMissile(id);
    }

    public void ResetAllMissile()
    {
        missileEventChannel.ResetMissile(0);
        missileEventChannel.ResetMissile(1);
        missileEventChannel.ResetMissile(2);
    }

}
