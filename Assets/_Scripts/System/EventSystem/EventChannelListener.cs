using System;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Parent class for all game event listeners.
/// </summary>
public abstract class AbstractEventListener : MonoBehaviour {
    [Serializable]
    public enum ResponseMode {
        InvokeUnityEvents,
        InvokeCSharpEvents
    }

    [SerializeField] protected ResponseMode _responseActivationMode;

    public ResponseMode ResponseActivationMode { get => _responseActivationMode; set => _responseActivationMode = value; }
}


/// <summary>
/// Listener for the VoidEventChannel.
/// </summary>
public class VoidEventChannelListener : AbstractEventListener 
{
    [SerializeField] VoidEventChannel _event;

    public VoidEventChannel Event { get => _event;
        set {
            _event?.UnregisterListener(this);
            _event = value;
            _event.RegisterListener(this);
        }
    }

    public void RaiseEvent() => Event.RaiseEvent();

    // C# Event.
    public event Action Response;

    // Unity Event.
    public event UnityAction UnityEventResponse {
        add => _unityEventResponse.AddListener(value);
        remove => _unityEventResponse.RemoveListener(value);
    }
    [SerializeField] UnityEvent _unityEventResponse;

    private void Awake() {
        if (_unityEventResponse == null) _unityEventResponse = new UnityEvent();
    }

    private void OnEnable() {
        Event?.RegisterListener(this);
    }

    public void ForceOnEnable() {
        OnEnable();
    }

    private void OnDisable() {
        Event?.UnregisterListener(this);
    }

    public void OnEventRaised() {
        switch (_responseActivationMode) {
            case ResponseMode.InvokeUnityEvents:
                _unityEventResponse.Invoke();
                break;
            case ResponseMode.InvokeCSharpEvents:
                Response?.Invoke();
                break;
        }
    }
}

public abstract class ParameterizedEventChannelListener<ActionType> : AbstractEventListener
{
    public ParameterizedEventChannel<ActionType> _event;
    public ParameterizedEventChannel<ActionType> Event { get => _event;
        set {
            _event?.UnregisterListener(this);
            _event = value;
            _event?.RegisterListener(this);
        }
    }

    // C# Event
    public event Action<ActionType> Response;

    // Unity Event
    public event UnityAction<ActionType> UnityEventResponse {
        add => AddListener(value);
        remove => RemoveListener(value);
    }

    protected virtual void OnEnable() {
        Event?.RegisterListener(this);
    }

    public void ForceOnEnable() {
        OnEnable();
    }

    protected virtual void OnDisable() {
        Event?.UnregisterListener(this);
    }

    public void OnEventRaised(ActionType value) {
        switch (_responseActivationMode) {
            case ResponseMode.InvokeUnityEvents:
                InvokeUnityEventResponse(value);
                break;
            case ResponseMode.InvokeCSharpEvents:
                Response?.Invoke(value);
                break;
        }
    }

    protected abstract void InvokeUnityEventResponse(ActionType value);
    protected abstract void AddListener(UnityAction<ActionType> action);
    protected abstract void RemoveListener(UnityAction<ActionType> action);
}
