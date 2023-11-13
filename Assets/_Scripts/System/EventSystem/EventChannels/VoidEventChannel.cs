using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Unparameterized global event channel for relaying/broadcasting messages.
/// </summary>
[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Event System/Channels/Void Event Channel", order = 0)]
public class VoidEventChannel : ScriptableObject
{
    protected HashSet<VoidEventChannelListener> _listeners = new HashSet<VoidEventChannelListener>();

    public IReadOnlyCollection<VoidEventChannelListener> Listeners => _listeners;

    public bool RegisterListener(VoidEventChannelListener listener) {
        return _listeners.Add(listener);
    }

    public void UnregisterListener(VoidEventChannelListener listener) {
        _listeners.Remove(listener);
    }
    
    public void RaiseEvent() {
        foreach (VoidEventChannelListener listener in _listeners) {
            listener.OnEventRaised();
        }
    }
}
