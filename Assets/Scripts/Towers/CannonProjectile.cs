using UnityEngine;

public class CannonProjectile : MonoBehaviour {
    private int _damage = 10;
    private float _gravity;
    private Vector3 _speedV;

    public void Init(Vector3 direction, int damage, float gravity) {
        _speedV = direction;
        _gravity = gravity;
        _damage = damage;
        Destroy(gameObject, 10);
    }

    void Update() {
        var nextPos = transform.position + _speedV * Time.deltaTime;
        transform.forward = nextPos - transform.position;
        transform.position = nextPos;

        _speedV.y -= _gravity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        var monster = other.gameObject.GetComponent<Monster>();
        if (monster == null)
            return;

        monster.TakeDamage(_damage);
        Destroy(gameObject);
    }
}