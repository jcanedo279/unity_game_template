using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [System.Serializable]
    public class KeyValue
    {
        public TKey key;
        public TValue value;
    }

    private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
    public Dictionary<TKey, TValue> Dict => _dict;

    [SerializeField]
    private List<KeyValue> _serializedDictionaryData = new List<KeyValue>();

    public void OnAfterDeserialize()
    {
        _dict.Clear();
        foreach(var kv  in _serializedDictionaryData)
            _dict.Add(kv.key, kv.value);
    }

    public void OnBeforeSerialize()
    {
        _serializedDictionaryData.Clear();
        foreach (var kv in _dict)
            _serializedDictionaryData.Add(new KeyValue { key = kv.Key, value = kv.Value });
    }
}
