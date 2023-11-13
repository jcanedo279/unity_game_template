using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class GameSceneManager {

    // Callbacks used to store and release the sceneHandle assets.
    public static void StoreSceneHandle(string sceneName,
                          AsyncOperationHandle<SceneInstance> sceneHandle,
                          GameSceneManagerData gameSceneManagerData) {
        if (gameSceneManagerData.sceneNameToSceneHandle.ContainsKey(sceneName)) {
            return;
        }
        gameSceneManagerData.sceneNameToSceneHandle.Add(sceneName, sceneHandle);
    }

    public static void ReleaseSceneHandle(string sceneName,
                            GameSceneManagerData gameSceneManagerData) {
        if (!gameSceneManagerData.sceneNameToSceneHandle.ContainsKey(sceneName)) {
            return;
        }
        gameSceneManagerData.sceneNameToSceneHandle.Remove(sceneName);
    }

    public static IEnumerator LoadSceneAsyncByName(string sceneName,
                                                   GameSceneManagerData gameSceneManagerData = null,
                                                   System.Action onComplete = null) {
        if (SceneManager.GetSceneByName(sceneName).isLoaded) {
            yield break;
        }
        AsyncOperationHandle<SceneInstance> sceneHandle =
            Addressables.LoadSceneAsync($"Assets/Scenes/{sceneName}.unity",LoadSceneMode.Additive);
        sceneHandle.Completed += (asyncHandle) => {
            if (gameSceneManagerData != null) {
                GameSceneManager.StoreSceneHandle(sceneName,
                                                  sceneHandle,
                                                  gameSceneManagerData);
            }
            onComplete?.Invoke();
        };
    }

    public static IEnumerator UnloadSceneAsyncByName(string sceneName,
                                                     GameSceneManagerData gameSceneManagerData = null) {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded) {
            yield break;
        }
        // Unload each of the current scenes in the background.
        SceneManager.UnloadSceneAsync(sceneName).completed += (asyncHandle) => {
            if (gameSceneManagerData != null) {
                GameSceneManager.ReleaseSceneHandle(sceneName,
                                                    gameSceneManagerData);
            }
        };
    }
}
