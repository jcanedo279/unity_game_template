using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class UIObjectResponseEventChannelListener : ParameterizedEventChannelListener<UIObjectResponse>
{
    [Serializable]
    private class UnityEvent : UnityEvent<UIObjectResponse> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<UIObjectResponse> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<UIObjectResponse> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(UIObjectResponse value) => _unityEventResponse.Invoke(value);
}
