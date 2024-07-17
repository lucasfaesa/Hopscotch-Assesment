using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissileData", menuName = "ScriptableObjects/Missile/MissileData")]
public class MissileDataSO : ScriptableObject
{
    [field: SerializeField] public int MissileId { get; set; }
    
    [field:Header("Explosion Settings")]
    [field: SerializeField] public float ExplosionForce { get; set; } = 10f;
    [field: SerializeField] public float ExplosionRadius { get; set; } = 1f;
    [field: SerializeField] public float UpwardsModifier { get; set; } = 1f;
    [field: SerializeField] public ForceMode ForceMode { get; set; } = ForceMode.Impulse;
    
    [field:Header("Missile Settings")]
    [field: SerializeField] public float Speed { get; set; } = 5;
    [field: SerializeField] public bool IsReadyToLaunch { get; set; } = true;
    [field:Space]
    [field: SerializeField] public List<Mesh> MeshesList { get; set; }
    [field: SerializeField] public int SelectedMeshIndex { get; set; }
    [field:Space]
    [field: SerializeField] public List<Material> MaterialsList { get; set; }
    [field: SerializeField] public int SelectedMaterialIndex { get; set; }
    
    public void Reset()
    {
        IsReadyToLaunch = true;
    }
}
