using System.Linq;
using UnityEngine;

public class CrystalTower : AbstractTower {
    [SerializeField]
    private CrystalTowerConfig _config;

    [SerializeField]
    private GuidedProjectile _projectilePrefab;

    private float _lastShotTime = -0.5f;

    void Update() {
        if (_projectilePrefab == null)
            return;

        if (_lastShotTime + _config.ShootInterval > Time.time)
            return;

        Shoot();
        _lastShotTime = Time.time;
    }

    private void Shoot() {
        var monstersInRange = _monstersService.ActiveMonsters.Where(CheckInRange);
        if (_config.IsShootingEveryone) {
            foreach (var monster in monstersInRange) {
                ShootMonster(monster);
            }
        } else {
            Monster monster = monstersInRange.FirstOrDefault();
            if (monster != null) {
                ShootMonster(monster);
            }
        }
    }

    private void ShootMonster(Monster monsterInRange) {
        var projectile = Instantiate(_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        projectile.Init(monsterInRange.gameObject, _config.ProjectileSpeed, _config.ProjectileDamage);
    }

    private bool CheckInRange(Monster m) => Vector3.Distance(transform.position, m.transform.position) <= _config.Range;
}