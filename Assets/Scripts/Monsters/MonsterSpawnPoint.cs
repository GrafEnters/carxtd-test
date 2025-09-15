using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour {
    public float m_interval = 3;
    public GameObject m_moveTarget;

    private float m_lastSpawn = -1;

    private MonstersService _monstersService;

    public void Init(MonstersService monstersService) {
        _monstersService = monstersService;
    }

    void Update() {
        if (!(Time.time > m_lastSpawn + m_interval)) {
            return;
        }

        SpawnMonster();

        m_lastSpawn = Time.time;
    }

    private void SpawnMonster() {
        Monster monster = _monstersService.GetRandomMonster();
        monster.transform.position = transform.position;
        monster.SetTarget(m_moveTarget);
        monster.transform.SetParent(transform);
    }
}