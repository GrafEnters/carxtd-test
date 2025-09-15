using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour {
    public float m_speed = 0.2f;
    public int m_damage = 10;

    void Update() {
        transform.position += transform.forward * m_speed;
    }

    void OnTriggerEnter(Collider other) {
        var monster = other.gameObject.GetComponent<Monster>();
        if (monster == null)
            return;

        monster.TakeDamage(m_damage);
        Destroy(gameObject);
    }
}