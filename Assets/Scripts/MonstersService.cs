using System.Collections.Generic;
using UnityEngine;

public class MonstersService {
    public readonly List<Monster> ActiveMonsters;

    private List<MonsterConfig> _monsterConfigs = new List<MonsterConfig>();

    public MonstersService(ConfigsProvider configsProvider) {
        _monsterConfigs = configsProvider.MonstersConfigs;
        ActiveMonsters = new List<Monster>();
    }

    public Monster GetRandomMonster() {
        var config = _monsterConfigs[Random.Range(0, _monsterConfigs.Count)];
        GameObject monsterGo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Rigidbody rb = monsterGo.AddComponent<Rigidbody>();
        rb.useGravity = false;
        Monster monster = monsterGo.AddComponent<Monster>();
        monster.Init(config, ReleaseMonster);
        ActiveMonsters.Add(monster);
        return monster;
    }

    private void ReleaseMonster(Monster monster) {
        ActiveMonsters.Remove(monster);
    }
}