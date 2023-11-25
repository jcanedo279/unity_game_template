using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject {
    [System.NonSerialized] public Transform playerTransform;
}
