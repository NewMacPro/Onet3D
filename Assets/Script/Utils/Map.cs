using UnityEngine;
using System;
using System.Collections.Generic;


[System.Serializable]
public class Map<K, V>
{

    private Dictionary<K, V> dictionary;

    public Map()
    {
        dictionary = new Dictionary<K, V>();
    }

    public Map(int capacity)
    {
        dictionary = new Dictionary<K, V>(capacity);
    }

    public V this[K key]
    {
        get { return Get(key); }
        set { Put(key, value); }
    }

    public void Put(K key, V value, bool overrideExist = true)
    {
        if (overrideExist)
        {
            dictionary.Remove(key);
        }
        dictionary.Add(key, value);
    }

    public V Remove(K key)
    {
        V value = Get(key);
        dictionary.Remove(key);
        return value;
    }

    public bool Contains(K key)
    {
        return dictionary.ContainsKey(key);
    }

    public V Get(K key, bool autoNew = false, params object[] autoNewArgs)
    {
        V value;
        if (key != null && dictionary.TryGetValue(key, out value))
        {
            return value;
        }
        else if (autoNew)
        {
            try
            {
                value = (V)System.Activator.CreateInstance(typeof(V), autoNewArgs);
                Put(key, value);
                return value;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        return default(V);
    }

    public V GetFirst()
    {
        V[] values = Values();
        return values.Length > 0 ? values[0] : default(V);
    }

    public KeyValuePair<K, V>[] EntryArray()
    {
        List<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>(dictionary.Count);
        foreach (KeyValuePair<K, V> pair in dictionary)
        {
            list.Add(pair);
        }
        return list.ToArray();
    }

    public K[] Keys()
    {
        if (dictionary.Count == 0)
        {
            return new K[0];
        }
        K[] keys = new K[dictionary.Count];
        dictionary.Keys.CopyTo(keys, 0);
        return keys;
    }


    public V[] Values()
    {
        if (dictionary.Count == 0)
        {
            return new V[0];
        }
        V[] values = new V[dictionary.Count];
        dictionary.Values.CopyTo(values, 0);
        return values;
    }

    public int Size()
    {
        return dictionary.Count;
    }

    public bool Empty()
    {
        return dictionary.Count == 0;
    }

    public bool NotEmpty()
    {
        return dictionary.Count > 0;
    }

    public void Clear()
    {
        dictionary.Clear();
    }

}

