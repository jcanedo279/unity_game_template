using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class UIComponentRequestEventChannelListener : ParameterizedEventChannelListener<UIComponentRequest>
{
    [Serializable]
    private class UnityEvent : UnityEvent<UIComponentRequest> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<UIComponentRequest> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<UIComponentRequest> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(UIComponentRequest value) => _unityEventResponse.Invoke(value);
}
