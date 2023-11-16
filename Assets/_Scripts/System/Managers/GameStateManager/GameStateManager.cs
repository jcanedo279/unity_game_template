using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum GameState {
    GAME_STATE_DEFAULT,
    GAME_STATE_START_DAY,
    GAME_STATE_START_NIGHT,
    GAME_STATE_AQUARIUM,
}


/// <summary>
/// GameStateManager based on Addresseable Scenes.
/// </summary>
public class GameStateManager : ParameterizedEventChannelListener<GameState>
{
    // INTERNAL - OVERRIDE LISTENER
    // ------------------------------------------------------------------------------------------------------
    [Serializable]
    private class UnityEvent : UnityEvent<GameState> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<GameState> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<GameState> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(GameState nextGameState) {
        SetGameState(nextGameState);
        _unityEventResponse.Invoke(nextGameState);
    }


    // GameStateManager
    // ------------------------------------------------------------------------------------------------------
    [SerializeField] private GameStateManagerData gameStateManagerData;

    void Start() {
        if (!gameStateManagerData) {
            throw new System.ArgumentNullException("No GameStateManagerData ScriptableObject loaded :<");
        }
        
        InitializeMutableGameStateManagerData();
        SetGameState(GameState.GAME_STATE_START_DAY);
        LoadSharedScenes();
    }

    void InitializeMutableGameStateManagerData() {
        gameStateManagerData.currentGameState = GameState.GAME_STATE_DEFAULT;
        gameStateManagerData.numAsyncLoads = 1;
    }

    public void SetGameState(GameState nextGameState) {
        if (gameStateManagerData.currentGameState == nextGameState) {
            return;
        }
        gameStateManagerData.numAsyncLoads = 1;
        int requiredNumberScenesLoaded = GameStateManagerData.gameStateToSceneNames[nextGameState].Count;
        // Load next scenes.
        foreach (string sceneName in GameStateManagerData.gameStateToSceneNames[nextGameState]) {
            StartCoroutine(GameSceneManager.LoadSceneAsyncByName(sceneName,
                gameSceneManagerData:gameStateManagerData.gameSceneManagerData,
                onComplete:()=>{ OnGameStateSceneComplete(requiredNumberScenesLoaded,nextGameState); }));
        }
    }

    void LoadSharedScenes() {
        foreach (string sceneName in GameStateManagerData.sharedSceneNames) {
            StartCoroutine(GameSceneManager.LoadSceneAsyncByName(sceneName,
                gameSceneManagerData:gameStateManagerData.gameSceneManagerData));
        }
    }

    void UnloadCurrentScenes() {
        foreach (string sceneName in GameStateManagerData.gameStateToSceneNames[gameStateManagerData.currentGameState]) {
            StartCoroutine(GameSceneManager.UnloadSceneAsyncByName(sceneName,
                gameSceneManagerData:gameStateManagerData.gameSceneManagerData));
        }
    }

    // GameStateManager Callbacks
    // ----------------------------------------------------------------------------------------------------------

    // This is all the background state transition things we have to immediately do when the state changes.
    void OnGameStateSceneComplete(int requiredNumberScenesLoaded,
                                  GameState nextGameState) {
        // We want to increment the required num load scenes onComplete.
        gameStateManagerData.numAsyncLoads += 1;
        // If we are done loading all the scenes, do this stuff...
        if (gameStateManagerData.numAsyncLoads >= requiredNumberScenesLoaded) {
            UnloadCurrentScenes();
        }
        StartCoroutine(OnAsyncOnGameStateSceneComplete(nextGameState));

    }

    // This is all the stuff we actually want to do (game-wise) when the state changes.
    IEnumerator OnAsyncOnGameStateSceneComplete(GameState nextGameState) {
        switch (nextGameState) {
            case GameState.GAME_STATE_START_DAY:
                GameObject.FindWithTag("GlobalCanvas").GetComponent<Canvas>().enabled = true;
                gameStateManagerData.gameAudioRequestEventChannel.RaiseEvent(gameStateManagerData.startBackgroundMusicGameAudioRequest);
                break;
            default:
                break;
        }
        gameStateManagerData.currentGameState = nextGameState;
        yield return null;
    }
}
