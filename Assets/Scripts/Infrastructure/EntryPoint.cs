using UnityEngine;

public class EntryPoint : MonoBehaviour {
    private void Awake() {
        InitScene();
    }

    private void InitScene() {
        var configsProvider = new ConfigsProvider();
        var monstersService = new MonstersService(configsProvider);
        var spawners = FindObjectsByType<MonsterSpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var spawner in spawners) {
            spawner.Init(monstersService);
        }

        var towers = FindObjectsByType<CannonTower>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var tower in towers) {
            tower.Init(monstersService);
        }
    }
}