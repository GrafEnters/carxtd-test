using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigsProvider {

    public List<MonsterConfig> MonstersConfigs;
    public MonsterSpawningConfig MonsterSpawningConfig;
    public ConfigsProvider() {
        MonstersConfigs = Resources.LoadAll<MonsterConfig>("Configs").ToList();
        MonsterSpawningConfig =  Resources.LoadAll<MonsterSpawningConfig>("Configs").FirstOrDefault();
    }
}