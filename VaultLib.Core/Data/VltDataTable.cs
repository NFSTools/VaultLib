using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VaultLib.Core.Data;

public class VltDataTable
{
    [DebuggerDisplay("{Key} = {Value}")]
    public class Entry
    {
        public Entry(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public object Value { get; set; }
    }

    private readonly Dictionary<string, object> _entryLookup = new();

    private readonly List<Entry> _entries = new();

    public IReadOnlyList<Entry> GetEntries()
    {
        return _entries;
    }

    public IReadOnlyDictionary<string, object> GetDictionary() => _entryLookup;

    public bool HasValue(string key) => _entryLookup.ContainsKey(key);

    public object GetValue(string key)
    {
        return _entryLookup[key];
    }

    public bool TryGetValue(string key, out object value) => _entryLookup.TryGetValue(key, out value);

    public T GetValue<T>(string key)
    {
        var val = _entryLookup[key];
        return val is T value
            ? value
            : throw new InvalidCastException(
                $"Type mismatch for key {key}: actual type is {val.GetType()}, requested type is {typeof(T)}");
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        if (!_entryLookup.TryGetValue(key, out var val))
        {
            value = default;
            return false;
        }

        if (val is not T casted)
        {
            throw new InvalidCastException(
                $"Type mismatch for key {key}: actual type is {val.GetType()}, requested type is {typeof(T)}");
        }

        value = casted;
        return true;
    }

    public void SetValue(string key, object value)
    {
        if (_entryLookup.TryGetValue(key, out var previousValue))
        {
            if (previousValue.GetType() != value.GetType())
            {
                throw new Exception(
                    $"Type mismatch for key {key}: previous type was {previousValue.GetType()}, new type is {value.GetType()}");
            }
        }

        _entryLookup[key] = value;

        if (_entries.Find(e => e.Key == key) is { } entry)
        {
            entry.Value = value;
        }
        else
        {
            _entries.Add(new Entry(key, value));
        }
    }

    public void RemoveValue(string key)
    {
        _entryLookup.Remove(key);
        _entries.RemoveAll(e => e.Key == key);
    }
}