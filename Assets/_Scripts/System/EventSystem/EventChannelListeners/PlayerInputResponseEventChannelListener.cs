using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PlayerInputResponseEventChannelListener : ParameterizedEventChannelListener<PlayerInputResponse>
{
    [Serializable]
    private class UnityEvent : UnityEvent<PlayerInputResponse> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<PlayerInputResponse> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<PlayerInputResponse> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(PlayerInputResponse value) => _unityEventResponse.Invoke(value);
}
