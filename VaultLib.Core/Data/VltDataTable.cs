using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Data;

public class VltDataTable<TKey> where TKey : struct, IKey<TKey>
{
    [DebuggerDisplay("{Key} = {Value}")]
    public class Entry
    {
        public Entry(TKey key, object value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; }
        public object Value { get; set; }
    }

    private readonly Dictionary<TKey, object> _entryLookup = new();

    private readonly List<Entry> _entries = new();

    public IReadOnlyList<Entry> GetEntries()
    {
        return _entries;
    }

    public IReadOnlyDictionary<TKey, object> GetDictionary() => _entryLookup;

    public bool HasValue(TKey key) => _entryLookup.ContainsKey(key);

    public object GetValue(TKey key)
    {
        return _entryLookup[key];
    }

    public T GetValue<T>(TKey key)
    {
        var val = _entryLookup[key];
        return val is T value
            ? value
            : throw new InvalidCastException(
                $"Type mismatch for key {key}: actual type is {val.GetType()}, requested type is {typeof(T)}");
    }

    public bool TryGetValue<T>(TKey key, [NotNullWhen(true)] out T? value)
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

    public void SetValue(TKey key, object value)
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

    public void RemoveValue(TKey key)
    {
        _entryLookup.Remove(key);
        _entries.RemoveAll(e => e.Key == key);
    }
}