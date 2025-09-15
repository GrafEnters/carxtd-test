using UnityEngine;

[CreateAssetMenu(fileName = "CanonTowerConfig", menuName = "Scriptable Objects/CanonTowerConfig", order = 0)]
public class CanonTowerConfig : TowerConfig {
    [Header("Canon Tower")]
    public float Gravity;
    
    [Header("Rotation")]
    public float MinPitch = -5f;
    public float MaxPitch = 45f;
    public float MaxHorizontalAngle = 45f;

    [Min(0.01f)]
    public float AngleDiffToShoot = 1;

    [Min(0f)]
    public float HorRotationSpeed = 0.5f, VertRotationSpeed = 0.5f;
}