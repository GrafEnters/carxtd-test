using UnityEngine;

[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Scriptable Objects/MonsterConfig", order = 0)]
public class MonsterConfig : ScriptableObject {
    public float Speed = 0.1f;
    public int MaxHp = 30;
    public Color Color = Color.red;
}