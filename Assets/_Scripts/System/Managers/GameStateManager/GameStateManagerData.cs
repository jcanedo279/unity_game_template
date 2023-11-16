using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "GameStateManagerData", menuName = "Game State Manager Data")]
public class GameStateManagerData : ScriptableObject {
    // IMMUTABLE.
    [SerializeField] public GameAudioRequestEventChannel gameAudioRequestEventChannel;
    [SerializeField] public GameAudioRequest startBackgroundMusicGameAudioRequest;
    [SerializeField] public GameSceneManagerData gameSceneManagerData;
    // MUTABLE.
    public GameState currentGameState;
    public int numAsyncLoads = 1;

    // STATIC.
    public static Dictionary<GameState,List<string>> gameStateToSceneNames { get; }
            = new Dictionary<GameState, List<string>> {
        {GameState.GAME_STATE_DEFAULT, new List<string> { "Scene_Initializer" }},
        {GameState.GAME_STATE_START_DAY, new List<string> { "Scene_StartDay" }},
        {GameState.GAME_STATE_START_NIGHT, new List<string> { "Scene_StartNight" }},
        {GameState.GAME_STATE_AQUARIUM, new List<string> { "Scene_Aquarium" }}
    };
    public static List<string> sharedSceneNames { get; } = new List<string> { "Scene_Shared" };
}
