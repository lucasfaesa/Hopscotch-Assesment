using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissileSettingsController : MonoBehaviour
{
    [Header("SOs")] 
    [SerializeField] private MissileEventChannelSO missileEventChannel;
    [SerializeField] private MissileDataSO missileData;
    
    [Header("Components")] 
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TextMeshProUGUI speedValueText;
    [Space]
    [SerializeField] private Slider explosionForceSlider;
    [SerializeField] private TextMeshProUGUI explosionForceValueText;
    [Space]
    [SerializeField] private Slider explosionRadiusSlider;
    [SerializeField] private TextMeshProUGUI explosionRadiusValueText;
    [Space]
    [SerializeField] private Slider upwardsForceSlider;
    [SerializeField] private TextMeshProUGUI upwardsModifierValueText;
    [Space]
    [SerializeField] private TMP_Dropdown meshesDropdown;
    [Space]
    [SerializeField] private TMP_Dropdown materialsDropdown;
    
    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        speedSlider.SetValueWithoutNotify(missileData.Speed);
        speedValueText.text = missileData.Speed.ToString("F1");
        
        explosionForceSlider.SetValueWithoutNotify(missileData.ExplosionForce);
        explosionForceValueText.text = missileData.ExplosionForce.ToString("F1");
        
        explosionRadiusSlider.SetValueWithoutNotify(missileData.ExplosionRadius);
        explosionRadiusValueText.text = missileData.ExplosionRadius.ToString("F1");
        
        upwardsForceSlider.SetValueWithoutNotify(missileData.UpwardsModifier);
        upwardsModifierValueText.text = missileData.UpwardsModifier.ToString("F1");
        
        foreach (Mesh mesh in missileData.MeshesList)
        {
            meshesDropdown.options.Add (new TMP_Dropdown.OptionData() {text=mesh.name});
        }
        meshesDropdown.value = missileData.SelectedMeshIndex;
        meshesDropdown.RefreshShownValue();
        
        foreach (Material mat in missileData.MaterialsList)
        {
            materialsDropdown.options.Add (new TMP_Dropdown.OptionData() {text=mat.name});
        }
        materialsDropdown.value = missileData.SelectedMaterialIndex;
        materialsDropdown.RefreshShownValue();
    }

    public void AdjustSpeed(float value)
    {
        missileData.Speed = value;
        speedValueText.text = value.ToString("F1");
    }
    
    public void AdjustExplosiveForce(float value)
    {
        missileData.ExplosionForce = value;
        explosionForceValueText.text = value.ToString("F1");
    }
    
    public void AdjustExplosionRadius(float value)
    {
        missileData.ExplosionRadius = value;
        explosionRadiusValueText.text = value.ToString("F1");
    }
    
    public void AdjustUpwardsModifier(float value)
    {
        missileData.UpwardsModifier = value;
        upwardsModifierValueText.text = value.ToString("F1");
    }

    public void ChangeModel(int value)
    {
        missileEventChannel.ChangeMissileMesh(missileData.MissileId, value);
    }

    public void ChangeMaterial(int value)
    {
        missileEventChannel.ChangeMissileMaterial(missileData.MissileId, value);
    }
}
