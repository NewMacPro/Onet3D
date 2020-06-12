using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesLoader
{
    static Dictionary<string, Object> resourceDic = new Dictionary<string, Object>();

    public static T Load<T>(string path) where T:Object
    {
        if (resourceDic.ContainsKey(path))
            return (T)resourceDic[path];

        T obj = Resources.Load<T>(path);
        if (obj == null)
        {
            Debug.LogError("Can't find resouces: " + path);
            return null;
        }

        resourceDic.Add(path, obj);
        return obj;
    }

    public static T CreateObjFromPrefab<T>(string path) where T:Object
    {
        return GameObject.Instantiate(Load<T>("Prefabs/" + path));
    }   
}
