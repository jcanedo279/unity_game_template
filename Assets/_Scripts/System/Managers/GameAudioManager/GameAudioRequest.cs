using UnityEngine;

[CreateAssetMenu(fileName = "GameAudioRequest", menuName = "Game Audio Request")]
public class GameAudioRequest : ScriptableObject {
    [SerializeField] public string audioClipName;
    [SerializeField] public GameAudioType gameAudioType;
}

public enum GameAudioType {
    GAME_AUDIO_TYPE_CLIP,
    GAME_AUDIO_TYPE_MUSIC,
}
