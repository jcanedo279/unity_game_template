using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {
    GAME_STATE_DEFAULT,
    GAME_STATE_START_DAY,
    GAME_STATE_START_NIGHT,
    GAME_STATE_AQUARIUM,
}


/// <summary>
/// GameStateManager based on Addresseable Scenes.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameStateManagerData gameStateManagerData;
    [SerializeField] public GameSceneManagerData gameSceneManagerData;
    [SerializeField] StringBoolMapEventChannel gameStateTransitionMultiCriteriaEventChannel;
    StringEventChannelListener onGameStateTransitionMultiCriteriaMetEventChannelListener;
    public static Dictionary<string,GameState> gameStateNameToGameState = new Dictionary<string,GameState> {
        {"multiCriteriaGameStateTransitionToAquarium",GameState.GAME_STATE_AQUARIUM},
    };

    void Awake() {
        onGameStateTransitionMultiCriteriaMetEventChannelListener
            = GetComponent<StringEventChannelListener>();
    }

    void Start() {
        if (!gameStateManagerData) {
            Debug.Log("No GameStateManagerData ScriptableObject loaded :<");
            return;
        }
        
        InitializeMutableGameStateManagerData();
        SetGameState(GameState.GAME_STATE_START_DAY);
        LoadSharedScenes();

        gameStateTransitionMultiCriteriaEventChannel.RaiseEvent(new Dictionary<string,bool>{
            {"isCriteriaMetSceneLoad",true},
            {"isCriteriaMetDataLoad",true}, // In the future this should come from data.
        });
    }

    void InitializeMutableGameStateManagerData() {
        gameStateManagerData.gameState = GameState.GAME_STATE_DEFAULT;
        gameStateManagerData.numAsyncLoads = 1;
    }

    public void onGameStateTransitionMultiCriteriaMet(string gameStateTransitionName) {
        if (!gameStateNameToGameState.ContainsKey(gameStateTransitionName)) {
            Debug.Log($"Could not interpret gameStateTransition: {gameStateTransitionName} to GameState :<");
            return;
        }
        GameState nextGameState = GameStateManager.gameStateNameToGameState[gameStateTransitionName];
        print($"GameState transition to: {gameStateTransitionName} intercepted, forwarding to state: {nextGameState}");
        SetGameState(nextGameState);
    }

    public void SetGameState(GameState nextGameState) {
        if (nextGameState == gameStateManagerData.gameState) {
            return;
        }
        gameStateManagerData.numAsyncLoads = 1;
        int requiredNumberScenesLoaded = gameStateManagerData.gameStateToSceneNames[nextGameState].Count;
        // Load next scenes.
        foreach (string sceneName in gameStateManagerData.gameStateToSceneNames[nextGameState]) {
            StartCoroutine(GameSceneManager.LoadSceneAsyncByName(sceneName,
                gameSceneManagerData:gameSceneManagerData,
                onComplete:()=>{ OnGameStateSceneComplete(requiredNumberScenesLoaded,nextGameState); }));
        }
    }

    void LoadSharedScenes() {
        foreach (string sceneName in gameStateManagerData.sharedSceneNames) {
            StartCoroutine(GameSceneManager.LoadSceneAsyncByName(sceneName,
                gameSceneManagerData:gameSceneManagerData));
        }
    }

    void UnloadCurrentScenes() {
        foreach (string sceneName in gameStateManagerData.gameStateToSceneNames[gameStateManagerData.gameState]) {
            StartCoroutine(GameSceneManager.UnloadSceneAsyncByName(sceneName,
                gameSceneManagerData:gameSceneManagerData));
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
        gameStateManagerData.gameState = nextGameState;
        yield return null;
    }
}
