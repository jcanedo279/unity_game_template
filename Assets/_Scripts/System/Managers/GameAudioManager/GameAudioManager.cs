using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameAudioManager : MonoBehaviour {
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundsSource;

    public void didReceiveGameAudioRequest(GameAudioRequest gameAudioRequest) {
        string audioClipName = gameAudioRequest.audioClipName;
        AsyncOperationHandle<AudioClip> handle
            = Addressables.LoadAssetAsync<AudioClip>($"Assets/_Data/AudioData/{audioClipName}.mp3");
        handle.Completed += (asyncHandle) => {
            if (asyncHandle.Status == AsyncOperationStatus.Succeeded) {
                if (gameAudioRequest.gameAudioType == GameAudioType.GAME_AUDIO_TYPE_CLIP) {
                    soundsSource.PlayOneShot(asyncHandle.Result);
                } else if (gameAudioRequest.gameAudioType == GameAudioType.GAME_AUDIO_TYPE_MUSIC) {
                    musicSource.PlayOneShot(asyncHandle.Result);
                }
                soundsSource.PlayOneShot(asyncHandle.Result);
            }
        };
    }
}
