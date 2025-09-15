using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CannonTower : AbstractTower {
    [SerializeField]
    private CannonProjectile _projectilePrefab;

    [SerializeField]
    private Transform _shootPoint;

    [SerializeField]
    private CanonTowerConfig _config;

    [SerializeField]
    private Transform _cannonHub, _cannonHead;

    private Vector3 _initialHubForward;
    private float _lastShotTime = -0.5f;
    private float _currentHorSpeed;
    private float _currentVertSpeed;

    private void Awake() {
        _initialHubForward = _cannonHub.forward;
    }

    void Update() {
        if (_projectilePrefab == null || _shootPoint == null)
            return;

        var monstersInRange = _monstersService.ActiveMonsters.Where(CheckInRange);
        foreach (var monster in monstersInRange) {
            if (LeadingShot.TryGetInterceptDirection(_shootPoint.position, monster.transform.position, monster.DirectionalSpeed,
                    _config.ProjectileSpeed, _config.Gravity, out Vector3 shootDir)) {
                if (!CanRotateToTarget(shootDir)) {
                    Debug.Log("Can't rotate there");
                    continue;
                }

                RotateToTarget(shootDir);

                if (_lastShotTime + _config.ShootInterval > Time.time)
                    return;

                if (Vector3.Angle(_cannonHead.forward, shootDir) > _config.AngleDiffToShoot) {
                    return;
                }

                // shot
                var proj = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
                proj.Init(shootDir, _config.ProjectileDamage, _config.Gravity);
                _lastShotTime = Time.time;
                break;
            } else {
                Debug.Log("Cant hit");
            }
        }
    }

    private bool CanRotateToTarget(Vector3 shootDir) {
        // горизонтальный компонент
        Vector3 flatDir = Vector3.ProjectOnPlane(shootDir, Vector3.up);
        Vector3 hubForwardFlat = Vector3.ProjectOnPlane(_initialHubForward, Vector3.up);
        float horizontalAngle = Vector3.SignedAngle(hubForwardFlat, flatDir, Vector3.up);
        if (Mathf.Abs(horizontalAngle) > _config.MaxHorizontalAngle) return false;

// вертикальный угол
        float distance = flatDir.magnitude;
        if (distance < 0.001f) return false; // цель прямо над башней
        float targetPitch = Mathf.Atan2(shootDir.y, distance) * Mathf.Rad2Deg;
        if (targetPitch < _config.MinPitch || targetPitch > _config.MaxPitch) return false;

        return true;
    }

    private void RotateToTarget(Vector3 shootDir) {
        if (shootDir.sqrMagnitude < 0.0001f) {
            return;
        }

        // --- горизонтальный поворот базы ---
        Vector3 flatDir = new Vector3(shootDir.x, 0f, shootDir.z);
        if (flatDir.sqrMagnitude > 0.0001f) {
            Quaternion targetHubRot = Quaternion.LookRotation(flatDir, Vector3.up);
            float maxDelta = _config.HorRotationSpeed * Time.deltaTime;
            _cannonHub.localRotation = Quaternion.RotateTowards(_cannonHub.localRotation, targetHubRot, maxDelta);
        }

        // --- вертикальный поворот дуло ---
        Vector3 localDir = _cannonHub.InverseTransformDirection(shootDir);
        float targetPitch = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        targetPitch = Mathf.Clamp(targetPitch, _config.MinPitch, _config.MaxPitch);

        Vector3 headEuler = _cannonHead.localEulerAngles;
        // поправка для отрицательных углов (EulerAngles всегда 0..360)
        if (headEuler.x > 180f) headEuler.x -= 360f;

        float maxVertDelta = _config.VertRotationSpeed * Time.deltaTime;
        headEuler.x = Mathf.MoveTowards(headEuler.x, targetPitch, maxVertDelta);

        _cannonHead.localEulerAngles = headEuler;
    }

    private bool CheckInRange(Monster m) => Vector3.Distance(transform.position, m.transform.position) <= _config.Range;

    private void OnDrawGizmosSelected() {
        if (_config) {
            Gizmos.DrawWireSphere(transform.position, _config.Range);
        }
    }
}