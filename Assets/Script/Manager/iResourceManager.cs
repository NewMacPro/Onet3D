
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using LitJson;

public static class iResourceManager
{
    public static string RES_FOLDER = "Resources";
    static Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();

    public static T Load<T>(string path) where T : Object
    {
        T obj = null;
        if (!resourceCache.ContainsKey(path))
        {
            obj = (T)LoadFromResource(path, typeof(T).FullName);
        }
        else
        {
            obj = (T)resourceCache[path];
        }
        return obj;
    }


    /*
        * 使用类型反射加载类，类型需要使用带命名空间的类名
        */
    private static Assembly[] AssumblyList = {
        Assembly.Load("UnityEngine"),
        Assembly.Load("UnityEngine.UI"),
        Assembly.GetExecutingAssembly()
    };

    public static System.Type GetAssemblyType(string resType)
    {
        System.Type type = null;
        foreach (var assembly in AssumblyList)
        {
            type = assembly.GetType(resType);
            if (null != type)
            {
                break;
            }
        }
        return type;
    }

    public static System.Object LoadFromResource(string path, string resType)
    {
        System.Type type = GetAssemblyType(resType);
        if (type == null)
        {
            Debug.LogError("Can not find type:" + resType);
            return null;
        }
        if (!resourceCache.ContainsKey(path))
        {
            var obj = Resources.Load(path, type);
#if UNITY_EDITOR
            if (obj == null)
            {
                obj = LoadAssetAtPath(
                    string.Format("Assets/{0}/{1}", RES_FOLDER, path)
                    , type);
            }
#endif
            resourceCache[path] = obj;
        }

        return resourceCache[path];
    }

#if UNITY_EDITOR
    static string[] ext = { ".jpg", ".png", ".json", ".txt", ".prefab", ".mat"
            , ".tga", ".mp3", ".anim", ".controller", ".wav", ".ttf" , ".otf"
            , ".fbx", ".shader", ".mp4"};
    private static Object LoadAssetAtPath(string path, System.Type type)
    {

        Object obj = null;
        foreach (var i in ext)
        {
            obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path + i, type);
            if (obj != null)
            {
                return obj;
            }
        }
        return obj;
    }
#endif

    public static GameObject LoadGameObject(string path)
    {

        return Load<GameObject>(path);
    }

    public static Sprite LoadSprite(string spriteName)
    {

        string path = IndexManager.Instance.getSpritePath(spriteName, false);
        if (string.IsNullOrEmpty(spriteName))
        {
            Debug.LogWarning("cant find sprite:" + spriteName);
        }
        Sprite sprite_prefabs = null;
        if (!string.IsNullOrEmpty(path))
        {
            sprite_prefabs = Load<Sprite>(path);
        }
        else
        {
            sprite_prefabs = Resources.Load<Sprite>("Images/" + spriteName);
        }


        return sprite_prefabs;
    }

    public static UnityEngine.Object LoadAssets(string path, string typeName)
    {
        UnityEngine.Object obj = null;
        if (!resourceCache.ContainsKey(path))
        {
            obj = (UnityEngine.Object)LoadFromResource(path, typeName);
        }
        else
        {
            obj = (UnityEngine.Object)resourceCache[path];
        }
        return obj;
    }

    public static Material LoadMaterial(string sourceName)
    {
        return Load<Material>(sourceName);
    }

    public static UnityEngine.Video.VideoClip LoadVideoClip(string sourceName)
    {
        return Load<UnityEngine.Video.VideoClip>(sourceName);
    }

    public static Texture LoadTexture(string sourceName)
    {
        return Load<Texture>(sourceName);
    }

    public static TextAsset LoadTextAsset(string sourceName)
    {
        return Load<TextAsset>(sourceName);
    }

    public static Font LoadFont(string sourceName)
    {
        return Load<Font>(sourceName);
    }



    public static Coroutine SetSpriteFromPersistFolder(string path, RawImage image)
    {
        if (!resourceCache.ContainsKey(path))
        {
            Coroutine currentCoroutine = CoroutineHelper.Instance.ExcuteTask(delegate
                {
                    LoadAndSetImage(path, image);
                });
            return currentCoroutine;
        }
        else
            image.texture = (Texture)resourceCache[path];

        return null;
    }

    public static Coroutine SetSpriteFromPersistFolder(string path, Image image)
    {
        if (!resourceCache.ContainsKey(path))
        {
            Coroutine currentCoroutine = CoroutineHelper.Instance.ExcuteTask(delegate
                {
                    LoadAndSetImage(path, image);
                });
            return currentCoroutine;
        }
        else
            image.sprite = (Sprite)resourceCache[path];

        return null;
    }

    public static void LoadAndSetImage(string path, Image image)
    {
        if (image == null || image.IsDestroyed())
        {
            return;
        }

        if (!resourceCache.ContainsKey(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            resourceCache[path] = sp;
            image.sprite = (Sprite)resourceCache[path];
        }
        else
            image.sprite = (Sprite)resourceCache[path];
    }

    public static void LoadAndSetImage(string path, RawImage rawImage)
    {
        if (rawImage == null || rawImage.IsDestroyed())
        {
            return;
        }

        if (!resourceCache.ContainsKey(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);

            resourceCache[path] = texture;
            rawImage.texture = (Texture)resourceCache[path];
        }
        else
            rawImage.texture = (Texture)resourceCache[path];
    }

    public static void UnLoad()
    {
        resourceCache.Clear();
        Resources.UnloadUnusedAssets();
    }
}
