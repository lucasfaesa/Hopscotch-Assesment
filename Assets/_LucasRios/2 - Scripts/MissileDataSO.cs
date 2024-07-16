using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissileData", menuName = "ScriptableObjects/Missile/MissileData")]
public class MissileDataSO : ScriptableObject
{
    [field: SerializeField] public int MissileId { get; set; }
    [field: SerializeField] public float Speed { get; set; } = 5;
    [field: SerializeField] public bool IsReadyToLaunch { get; set; } = true;


    public void Reset()
    {
        IsReadyToLaunch = true;
    }
}
