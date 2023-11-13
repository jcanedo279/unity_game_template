using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


[CreateAssetMenu(fileName = "HashSetEventChannel", menuName = "Event System/Channels/Hash Set Event Channel", order = 3)]
public class HashSetEventChannel : ParameterizedEventChannel<HashSet<object>> {}


/// <summary>
/// Parameterized global event channel for relaying/broadcasting messages.
/// </summary>
public class ParameterizedEventChannel<ActionType> : ScriptableObject
{
    protected HashSet<ParameterizedEventChannelListener<ActionType>> _listeners = new HashSet<ParameterizedEventChannelListener<ActionType>>();

    public IReadOnlyCollection<ParameterizedEventChannelListener<ActionType>> Listeners => _listeners;

    public bool RegisterListener(ParameterizedEventChannelListener<ActionType> listener) {
        return _listeners.Add(listener);
    }

    public void UnregisterListener(ParameterizedEventChannelListener<ActionType> listener) {
        _listeners.Remove(listener);
    }
    
    public void RaiseEvent(ActionType value) {
        foreach (ParameterizedEventChannelListener<ActionType> listener in _listeners) {
            listener.OnEventRaised(value);
        }
    }
}
