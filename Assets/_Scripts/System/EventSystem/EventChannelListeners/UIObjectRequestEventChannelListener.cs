using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class UIObjectRequestEventChannelListener : ParameterizedEventChannelListener<UIObjectRequest>
{
    [Serializable]
    private class UnityEvent : UnityEvent<UIObjectRequest> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<UIObjectRequest> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<UIObjectRequest> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(UIObjectRequest value) => _unityEventResponse.Invoke(value);
}
