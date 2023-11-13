using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class GameAudioRequestEventChannelListener : ParameterizedEventChannelListener<GameAudioRequest>
{
    [Serializable]
    private class UnityEvent : UnityEvent<GameAudioRequest> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<GameAudioRequest> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<GameAudioRequest> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(GameAudioRequest value) => _unityEventResponse.Invoke(value);
}
