using UnityEngine;

[CreateAssetMenu(fileName = "CanonTowerConfig", menuName = "Scriptable Objects/CanonTowerConfig", order = 0)]
public class CanonTowerConfig : TowerConfig {
    public float ShootInterval = 0.5f;
    public float Range = 4f;

    public float ProjectileSpeed;
    public float Gravity;

    [Min(0)]
    public int ProjectileDamage;

    public float MinPitch = -5f;

    public float MaxPitch = 45f;

    public float MaxHorizontalAngle = 45f;

    [Min(0.01f)]
    public float AngleDiffToShoot = 1;

    [Min(0f)]
    public float HorRotationSpeed = 0.5f, VertRotationSpeed = 0.5f;
}