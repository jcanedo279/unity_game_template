using UnityEngine;


public class AquariumWorldBounds : MonoBehaviour {
    [SerializeField] WorldData worldData;

    void Awake() {
        worldData.worldBounds = GetComponent<BoxCollider2D>();
    }
}
