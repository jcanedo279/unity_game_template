using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class StringBoolMapEventChannelListener : ParameterizedEventChannelListener<Dictionary<string,bool>>
{
    [Serializable]
    private class UnityEvent : UnityEvent<Dictionary<string,bool>> {}

    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    protected override void AddListener(UnityAction<Dictionary<string,bool>> action) => _unityEventResponse.AddListener(action);
    protected override void RemoveListener(UnityAction<Dictionary<string,bool>> action) => _unityEventResponse.RemoveListener(action);

    protected override void InvokeUnityEventResponse(Dictionary<string,bool> value) => _unityEventResponse.Invoke(value);
}
