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
    [SerializeField] private Slider explosionForceSlider;
    [SerializeField] private TextMeshProUGUI explosionForceValueText;
    [Space]
    [SerializeField] private Slider explosionRadiusSlider;
    [SerializeField] private TextMeshProUGUI explosionRadiusValueText;
    [Space]
    [SerializeField] private Slider upwardsForceSlider;
    [SerializeField] private TextMeshProUGUI upwardsModifierValueText;
    [Space] 
    [SerializeField] private TMP_Dropdown forceModeDropdown;
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
        explosionForceSlider.SetValueWithoutNotify(missileData.ExplosionForce);
        explosionForceValueText.text = missileData.ExplosionForce.ToString("F1");
        
        explosionRadiusSlider.SetValueWithoutNotify(missileData.ExplosionRadius);
        explosionRadiusValueText.text = missileData.ExplosionRadius.ToString("F1");
        
        upwardsForceSlider.SetValueWithoutNotify(missileData.UpwardsModifier);
        upwardsModifierValueText.text = missileData.UpwardsModifier.ToString("F1");

        switch (missileData.ForceMode)
        {
            case ForceMode.Impulse:
                forceModeDropdown.SetValueWithoutNotify(0);    
            break;
            case ForceMode.VelocityChange:
                forceModeDropdown.SetValueWithoutNotify(1);
            break;
        }
        
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
    
    public void AdjustForceMode(int value)
    {
        switch (value)
        {
            case 0:
                missileData.ForceMode = ForceMode.Impulse;
                break;
            case 1:
                missileData.ForceMode = ForceMode.VelocityChange;
            break;
        }
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
