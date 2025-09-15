using UnityEngine;

public class CannonProjectile : MonoBehaviour {
    public float m_speed = 0.2f;
    public int m_damage = 10;
    private float _gravity;
    private Vector3 _speedV;

    public void Init(Vector3 direction, float verticalSpeed, float gravity) {
        _speedV = direction;
        _gravity = gravity;
    }

    void Update() {
        var nextPos = transform.position + _speedV;
        transform.forward = nextPos - transform.position;
        transform.position = nextPos;

        _speedV.y -= _gravity;
    }

    void OnTriggerEnter(Collider other) {
        var monster = other.gameObject.GetComponent<Monster>();
        if (monster == null)
            return;

        monster.TakeDamage(m_damage);
        Destroy(gameObject);
    }
}