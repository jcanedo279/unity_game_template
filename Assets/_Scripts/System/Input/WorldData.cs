using UnityEngine;


[CreateAssetMenu(fileName = "WorldData", menuName = "WorldData")]
public class WorldData : ScriptableObject {
    [System.NonSerialized] public BoxCollider2D worldBounds;
}
