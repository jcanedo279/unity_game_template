using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameSceneManagerData gameSceneManagerData;
    [SerializeField] GameAudioRequestEventChannel gameAudioRequestEventChannel;
    [SerializeField] GameAudioRequest splashScreenGameAudioRequest;

    void Start() {
        Debug.Log("GameInitializer Begin");
        gameSceneManagerData.sceneNameToSceneHandle
            = new Dictionary<string,AsyncOperationHandle<SceneInstance>>{};
        StartCoroutine(LoadStartingScene(2f));
        gameAudioRequestEventChannel.RaiseEvent(splashScreenGameAudioRequest);
    }

    IEnumerator LoadStartingScene(float splashScreenDurationSeconds) {
        yield return new WaitForSeconds(splashScreenDurationSeconds);
        yield return GameSceneManager.LoadSceneAsyncByName("Scene_Shared",
            gameSceneManagerData:gameSceneManagerData,
            onComplete:()=>{
                Debug.Log("GameInitializer Cleanup");
                StartCoroutine(GameSceneManager.UnloadSceneAsyncByName("Scene_Initializer",
                                                                       gameSceneManagerData:gameSceneManagerData));
            });
    }
}
