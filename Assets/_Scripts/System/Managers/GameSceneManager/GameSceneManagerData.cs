using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(fileName = "GameSceneManagerData", menuName = "Game Scene Manager Data")]
[System.Serializable]
public class GameSceneManagerData : ScriptableObject {
    // Use scene name to store reference to Addressable asset handle.
    public Dictionary<string,AsyncOperationHandle<SceneInstance>> sceneNameToSceneHandle;
}
