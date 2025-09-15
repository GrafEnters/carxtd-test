using UnityEngine;

public class TowerConfig : ScriptableObject {
    public float ShootInterval = 0.5f;
    public float Range = 4f;
    
    [Header("Projectile")]
    [Min(0)]
    public int ProjectileDamage;
    public float ProjectileSpeed;
}