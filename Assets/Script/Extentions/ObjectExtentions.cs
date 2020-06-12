using UnityEngine;
using System.Collections.Generic;
using System;

public static class ObjectExtentions
{
    public static T FindAChild<T>(this Transform transform, string name) where T : Component
    {
        return UnityHelper.GetTheChildComponent<T>(transform.gameObject, name);
    }

    public static Component FindAChildByType(this Transform transform, string name, string componentType)
    {
        Transform child = UnityHelper.GetTheChildComponent<Transform>(transform.gameObject, name);
        if (child != null)
        {
            return child.GetComponent(componentType);
        }
        return null;
    }

    public static Transform FindAChild(this Transform transform, string name)
    {
        return UnityHelper.GetTheChildComponent<Transform>(transform.gameObject, name);
    }

    public static T Peek<T>(this List<T> list) where T : UIBase
    {
        if (list.Count > 0)
            return list[list.Count - 1];

        return null;
    }

    public static T Pop<T>(this List<T> list) where T : UIBase
    {
        if (list.Count > 0)
        {
            T value = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return value;
        }

        return null;
    }


    public static void Push<T>(this List<T> list, T item) where T : UIBase
    {
        list.Add(item);
    }

    public static T At<T>(this List<T> list, int index)
    {
        if (list.Count > index)
        {
            return list[index];
        }

        return default(T);
    }

    public static GameObject GetChildGameObject(this UnityEngine.Object obj, int index)
    {
        Transform transform = obj.GetTransform();
        if (transform != null && transform.childCount > index)
        {
            return transform.GetChild(index).gameObject;
        }
        return null;
    }

    public static Transform GetTransform(this UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (obj is GameObject)
            {
                return (obj as GameObject).transform;
            }
            else if (obj is Transform)
            {
                return obj as Transform;
            }
            else if (obj is Component)
            {
                return (obj as Component).transform;
            }
        }
        return null;
    }

    public static GameObject GetGameObject(this UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (obj is GameObject)
            {
                return obj as GameObject;
            }
            else if (obj is Transform)
            {
                return (obj as Transform).gameObject;
            }
            else if (obj is Component)
            {
                return (obj as Component).gameObject;
            }
        }
        return null;
    }

    public static T GetComponent<T>(this UnityEngine.Object obj) where T : Component
    {
        if (obj != null)
        {
            if (obj is T)
            {
                return obj as T;
            }
            else
            {
                GameObject gameObject = obj.GetGameObject();
                if (gameObject != null)
                {
                    return gameObject.GetComponent<T>();
                }
            }
        }
        return null;
    }

    public static T AddComponent<T>(this UnityEngine.Object obj) where T : Component
    {
        if (obj != null)
        {
            if (obj is T)
            {
                return obj as T;
            }
            else
            {
                GameObject gameObject = obj.GetGameObject();
                if (gameObject != null)
                {
                    gameObject.AddComponent<T>();
                    return gameObject.GetComponent<T>();
                }
            }
        }
        return null;
    }

    private static Transform FindChild(this UnityEngine.Object p, string name)
    {
        if (p != null)
        {
            Transform transform = p.GetTransform();
            if (transform != null)
            {
                return string.IsNullOrEmpty(name) ? transform : transform.Find(name);
            }
        }
        return null;
    }

    private static string GetAssemblyQualifiedName(string typeName, string typeAssembly, string version = "0.0.0.0", string culture = "neutral", string publicKeyToken = null)
    {
        return string.Format("{0}, {1}, Version={2}, Culture={3}, PublicKeyToken={4}", typeName, typeAssembly, version, culture, publicKeyToken);
    }

    public static Component GetChildByName(this UnityEngine.Object p, string name, string componentType)
    {
        Transform child = p.FindChild(name);
        if (child != null)
        {
            return child.GetComponent(componentType);
        }
        return null;
    }

    /// <summary>
    /// 相当于调用GetComponentsInChildren()
    /// </summary>
    /// <param name="p"></param>
    /// <param name="name"></param>
    /// <param name="componentType"></param>
    /// <param name="componentAssembly">UnityEngine or Assembly-CSharp</param>
    /// <param name="includeInactive"></param>
    /// <returns></returns>
    public static Component[] GetChildrenByName(this UnityEngine.Object p, string name, string componentType, string componentAssembly, bool includeInactive)
    {
        Transform child = p.FindChild(name);
        if (child != null)
        {
            string assemblyQualifiedName = GetAssemblyQualifiedName(componentType, componentAssembly);
            Type type = Type.GetType(assemblyQualifiedName, false);
            if (type != null)
            {
                return child.GetComponentsInChildren(type, includeInactive);
            }
            else
            {
                Debug.LogErrorFormat("Invalid component, name: {0}, assembly: {1}", componentType, componentAssembly);
            }
        }
        return new Component[0];
    }

    public static GameObject GetGameObjectByName(this UnityEngine.Object p, string name)
    {
        try
        {
            Transform child = p.FindChild(name);
            if (child != null)
            {
                return child.gameObject;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return null;
    }

    // public static void SetLayerWithChildren(this Transform trans, string layer)
    // {
    //     trans.gameObject.layer = LayerMask.NameToLayer(layer);

    //     foreach(Transform t in trans)
    //     {
    //         SetLayerWithChildren(t, layer);
    //     }
    // }

    public static void SetLayerWithChildren(this GameObject go, string layer)
    {
        go.layer = LayerMask.NameToLayer(layer);

        if (go.transform.childCount <= 0)
            return;

        foreach(Transform t in go.transform)
        {
            SetLayerWithChildren(t.gameObject, layer);
        }
    }

    //获取相机的bounds
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    // public static bool
}

