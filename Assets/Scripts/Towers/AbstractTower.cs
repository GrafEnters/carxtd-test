using UnityEngine;

public class AbstractTower : MonoBehaviour {
    protected MonstersService _monstersService;

    public void Init(MonstersService monstersService) {
        _monstersService = monstersService;
    }
}