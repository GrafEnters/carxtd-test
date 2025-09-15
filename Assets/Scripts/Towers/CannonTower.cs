using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour {
    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public CannonProjectile m_projectilePrefab;
    public Transform m_shootPoint;

    [SerializeField]
    private Transform _cannonHub, _cannonHead;

    private float m_lastShotTime = -0.5f;

    [SerializeField]
    private float _verticalSpeed = 5;

    private float _gravity = 0.01f;
    
    

    void Update() {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        if (m_lastShotTime + m_shootInterval > Time.time)
            return;

        foreach (var monster in FindObjectsOfType<Monster>()) {
            if (Vector3.Distance(transform.position, monster.transform.position) > m_range)
                continue;

            if (LeadingShot.TryGetInterceptDirection(m_shootPoint.position, monster.transform.position, monster.DirectionalSpeed,
                    m_projectilePrefab.m_speed, _gravity, out Vector3 shootDir, out float verticalSpeed, out float time)) {
                shootDir *= m_projectilePrefab.m_speed;
                shootDir.y = verticalSpeed;

                m_shootPoint.forward = shootDir;
                /*
                _cannonHub.localRotation = Quaternion.Euler(0, m_shootPoint.localRotation.eulerAngles.y, 0);
                _cannonHead.localRotation = Quaternion.Euler(m_shootPoint.localRotation.eulerAngles.x, 0, 0);*/
                _cannonHead.forward = shootDir;

                // shot
                var proj = Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
                proj.Init(shootDir, verticalSpeed, _gravity);
            }

            m_lastShotTime = Time.time;
        }
    }
}