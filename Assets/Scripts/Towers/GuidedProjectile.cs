using UnityEngine;

public class GuidedProjectile : MonoBehaviour {
    private GameObject _target;
    private float _speed = 0.2f;
    private int _damage = 10;

    public void Init(GameObject target, float speed, int damage) {
        _target = target;
        _speed = speed;
        _damage = damage;
    }

    void Update() {
        if (_target == null) {
            Destroy(gameObject);
            return;
        }

        var translation = _target.transform.position - transform.position;
        if (translation.magnitude > _speed * Time.deltaTime) {
            translation = translation.normalized * _speed * Time.deltaTime;
        }

        transform.Translate(translation);
    }

    void OnTriggerEnter(Collider other) {
        var monster = other.gameObject.GetComponent<Monster>();
        if (monster == null)
            return;

        monster.TakeDamage(_damage);
        Destroy(gameObject);
    }
}