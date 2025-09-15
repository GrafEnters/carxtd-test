using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CannonTower : AbstractTower {
    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public CannonProjectile m_projectilePrefab;
    public Transform m_shootPoint;

    [SerializeField]
    private Transform _cannonHub, _cannonHead;

    private float m_lastShotTime = -0.5f;

    [SerializeField]
    private float minPitch = -5f;

    [SerializeField]
    private float maxPitch = 45f;

    [SerializeField]
    private float maxHorizontalAngle = 45f, _angleDiffToShoot = 1;

    private float _currentHorSpeed;
    private float _currentVertSpeed;

    [SerializeField]
    private float _verticalSpeed = 5;

    private float _gravity = 0.01f;

    [SerializeField]
    private float horRotationSpeed = 0.5f, vertRotationSpeed = 0.5f;

    private Vector3 _initialHubForward;

    private void Awake() {
        _initialHubForward = _cannonHub.forward;
    }

    void Update() {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        var monstersInRange = _monstersService.ActiveMonsters.Where(CheckInRange);
        foreach (var monster in monstersInRange) {
            if (LeadingShot.TryGetInterceptDirection(m_shootPoint.position, monster.transform.position, monster.DirectionalSpeed,
                    m_projectilePrefab.m_speed, _gravity, out Vector3 shootDir, out float verticalSpeed, out float time)) {
                if (!CanRotateToTarget(shootDir)) {
                    Debug.Log("Can't rotate there");
                    continue;
                }

                RotateToTarget(shootDir);

                if (m_lastShotTime + m_shootInterval > Time.time)
                    return;

                if (Vector3.Angle(_cannonHead.forward, shootDir) > _angleDiffToShoot) {
                    return;
                }

                // shot
                var proj = Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
                proj.Init(shootDir, verticalSpeed, _gravity);
                m_lastShotTime = Time.time;
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
        if (Mathf.Abs(horizontalAngle) > maxHorizontalAngle) return false;

// вертикальный угол
        float distance = flatDir.magnitude;
        if (distance < 0.001f) return false; // цель прямо над башней
        float targetPitch = Mathf.Atan2(shootDir.y, distance) * Mathf.Rad2Deg;
        if (targetPitch < minPitch || targetPitch > maxPitch) return false;

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
            float maxDelta = horRotationSpeed * Time.deltaTime;
            _cannonHub.localRotation = Quaternion.RotateTowards(_cannonHub.localRotation, targetHubRot, maxDelta);
        }

        // --- вертикальный поворот дуло ---
        Vector3 localDir = _cannonHub.InverseTransformDirection(shootDir);
        float targetPitch = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        Vector3 headEuler = _cannonHead.localEulerAngles;
        // поправка для отрицательных углов (EulerAngles всегда 0..360)
        if (headEuler.x > 180f) headEuler.x -= 360f;

        float maxVertDelta = vertRotationSpeed * Time.deltaTime;
        headEuler.x = Mathf.MoveTowards(headEuler.x, targetPitch, maxVertDelta);

        _cannonHead.localEulerAngles = headEuler;
    }

    private bool CheckInRange(Monster m) => Vector3.Distance(transform.position, m.transform.position) <= m_range;

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, m_range);
    }
}