using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "GameStateManagerData", menuName = "Game State Manager Data")]
public class GameStateManagerData : ScriptableObject {
    // Serialized.
    [SerializeField] public StringEventChannel onGameStateTransitionMultiCriteriaMetEventChannel;
    [SerializeField] public GameAudioRequestEventChannel gameAudioRequestEventChannel;
    [SerializeField] public GameAudioRequest startBackgroundMusicGameAudioRequest;
    // Mutable.
    public GameState gameState;
    public int numAsyncLoads = 1;

    // Immutable.
    public Dictionary<GameState,List<string>> gameStateToSceneNames { get; private set; }
            = new Dictionary<GameState, List<string>> {
        {GameState.GAME_STATE_DEFAULT, new List<string> { "Scene_Initializer" }},
        {GameState.GAME_STATE_START_DAY, new List<string> { "Scene_StartDay" }},
        {GameState.GAME_STATE_START_NIGHT, new List<string> { "Scene_StartNight" }},
        {GameState.GAME_STATE_AQUARIUM, new List<string> { "Scene_Aquarium" }}
    };
    public List<string> sharedSceneNames { get; private set; } = new List<string> { "Scene_Shared" };
}
