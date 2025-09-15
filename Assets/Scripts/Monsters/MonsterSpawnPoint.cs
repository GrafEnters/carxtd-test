using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour {
    [SerializeField]
    private GameObject _moveTarget;

    private float _lastSpawn = -1;

    private MonstersService _monstersService;
    private MonsterSpawningConfig _config;

    public void Init(MonstersService monstersService) {
        _monstersService = monstersService;
        _config = monstersService.MonsterSpawningConfig;
    }

    void Update() {
        if (!(Time.time > _lastSpawn + _config.Interval)) {
            return;
        }

        SpawnMonster();

        _lastSpawn = Time.time;
    }

    private void SpawnMonster() {
        Monster monster = _monstersService.GetRandomMonster();
        monster.transform.position = transform.position;
        monster.SetTarget(_moveTarget);
        monster.transform.SetParent(transform);
    }
}