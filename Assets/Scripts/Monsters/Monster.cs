using System;
using UnityEngine;

public class Monster : MonoBehaviour {
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private float _reachDistance = 0.3f;

    private GameObject _moveTarget;
    private float _speed;
    private int _maxHp;
    private int _hp;
    private Action<Monster> _releaseMonster;

    public Vector3 DirectionalSpeed => (_moveTarget.transform.position - transform.position).normalized * _speed;

    public void Init(MonsterConfig config, Action<Monster> releaseCallback) {
        _maxHp = config.MaxHp;
        _speed = config.Speed;
        GetComponent<MeshRenderer>().material.color = config.Color;

        _releaseMonster = releaseCallback;
        _hp = _maxHp;
    }

    public void SetTarget(GameObject target) {
        _moveTarget = target;
    }

    void Update() {
        if (_moveTarget == null) {
            return;
        }

        if (IsTargetReached) {
            Destroy(gameObject);
            return;
        }

        MoveToTarget();
    }

    private void MoveToTarget() {
        var translation = _moveTarget.transform.position - transform.position;
        if (translation.magnitude > _speed) {
            translation = translation.normalized * _speed;
        }

        transform.Translate(translation);
    }

    private bool IsTargetReached => Vector3.Distance(transform.position, _moveTarget.transform.position) <= _reachDistance;

    public void TakeDamage(int damage) {
        _hp -= damage;
        if (_hp <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        _releaseMonster?.Invoke(this);
    }
}