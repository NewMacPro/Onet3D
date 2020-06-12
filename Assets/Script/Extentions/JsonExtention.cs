using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using LitJson;
using System;

public static class JsonExtention
{
    public static int GetInt(this JsonData jsonData, string key, bool require = true, int defaultValue = 0)
    {
        if (jsonData == null)
        {
            if (require) { Debug.LogWarning("The jsondata is null"); }
            return defaultValue;
        }

        if (string.IsNullOrEmpty(key))
        {
            return (int)jsonData;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            if (require) { Debug.LogWarning("The jsondata dont have key:" + key); };
            return defaultValue;
        }

        if (!jsonData[key].IsInt)
        {
            if (require) { Debug.LogWarning("The jsondata is not int:" + key); };
            return defaultValue;
        }

        return (int)jsonData[key];
    }

    public static string GetString(this JsonData jsonData, string key, bool require = true, string defaultValue = "")
    {
        if (jsonData == null)
        {
            if (require)
            {
                Debug.LogWarning("The jsondata is null");
            }
            return defaultValue;
        }
        if (string.IsNullOrEmpty(key))
        {
            return (string)jsonData;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            if (require)
            {
                Debug.LogWarning("The jsondata dont have key:" + key);
            }
            return defaultValue;
        }

        if (!jsonData[key].IsString)
        {
            Debug.LogWarning("The jsondata is not string:" + key);
            return jsonData[key].ToString();
        }

        return (string)jsonData[key];
    }

    public static JsonData GetNode(this JsonData jsonData, string key, bool require = true)
    {
        if (jsonData == null)
        {
            if (require)
            {
                Debug.LogWarning("The jsondata is null");
            }
            return null;
        }

        if (string.IsNullOrEmpty(key))
        {
            return jsonData;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            if (require)
            {
                Debug.LogWarning("The jsondata dont have key:" + key);
            }
            return null;
        }

        return jsonData[key];
    }

    public static bool ContainsKey(this JsonData jsonData, string key)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return false;
        }

        if (!jsonData.IsObject)
        {
            return false;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            // Debug.LogWarning("The jsonData dont have key:" + key);
            return false;
        }

        return true;
    }

    public static float GetFloat(this JsonData jsonData, string key)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return 0;
        }

        if (string.IsNullOrEmpty(key))
        {
            return (float)jsonData;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            Debug.LogWarning("The jsondata dont have key:" + key);
            return 0;
        }

        if (!jsonData[key].IsDouble && !jsonData[key].IsInt)
        {
            Debug.LogWarning("The jsondata is not double:" + key);
            return 0;
        }

        return (float)jsonData[key];
    }

    public static bool GetBool(this JsonData jsonData, string key, bool require = true, bool defaultValue = false)
    {
        if (jsonData == null)
        {
            if (require)
            {
                Debug.LogWarning("The jsondata is null");
            }
            return defaultValue;
        }

        if (string.IsNullOrEmpty(key))
        {
            return (bool)jsonData;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            if (require)
            {
                Debug.LogWarning("The jsondata dont have key:" + key);
            }
            return defaultValue;
        }

        if (!jsonData[key].IsBoolean)
        {
            if (require)
            {
                Debug.LogWarning("The jsondata is not bool:" + key);
            }
            return defaultValue;
        }

        return (bool)jsonData[key];
    }

    public static long GetLong(this JsonData jsonData, string key, bool require = true)
    {
        if (jsonData == null)
        {
            if (require)
                Debug.LogWarning("The jsondata is null");
            return 0;
        }

        if (string.IsNullOrEmpty(key))
        {
            return (long)jsonData;
        }

        if (jsonData.Keys == null || !jsonData.Keys.Contains(key))
        {
            if (require)
                Debug.LogWarning("The jsondata dont have key:" + key);
            return 0;
        }

        if (!jsonData[key].IsLong && !jsonData[key].IsInt)
        {
            if (require)
                Debug.LogWarning("The jsondata is not long:" + key);
            return 0;
        }

        if (jsonData[key].IsInt)
        {
            int temp = (int)jsonData[key];
            return (long)temp;
        }

        return (long)jsonData[key];
    }

    public static int SetValue(this JsonData jd, string key, object obj, string type = "")
    {
        if (jd == null)
        {
            Debug.LogWarning("The jsondata is null");
            return -1;
        }


        if (obj is string) {
            jd[key] = (string)obj;
        }
        else if (obj is bool) {
            jd[key] = (bool)obj;
        }
        else if (obj is JsonData) {
            jd[key] = (JsonData)obj;
        }
        else if (obj is int)
        {
            jd[key] = (int)obj;
        }
        else if(obj is long)
        {
            jd[key] = (long)obj;
        }
        else 
        {
            double num = (double)obj;
            switch (type)
            {
                case "int":
                    jd[key] = (int)num;
                    break;
                case "float":
                    jd[key] = (float)num;
                    break;
                case "long":
                    jd[key] = (long)num;
                    break;
                case "double":
                    jd[key] = (double)num;
                    break;
                default:
                    Debug.LogWarning("The value unknown: " +  obj.ToString());
                    break;
            }
        }

        return 0;
    }

    public static int FindIndex(this JsonData jsonData, Predicate<JsonData> match)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return -1;
        }

        if (!jsonData.IsArray)
            return -1;
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (match(jsonData[i]))
                return i;
        }

        return -1;
    }

    public static JsonData Find(this JsonData jsonData, Predicate<JsonData> match)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return null;
        }

        if (!jsonData.IsArray)
        {
            Debug.LogWarning("The jsondata is not a Array:" + jsonData.ToJson());
            return null;
        }
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (match(jsonData[i]))
                return jsonData[i];
        }

        return null;
    }

    public static JsonData FindAll(this JsonData jsonData, Predicate<JsonData> match)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return null;
        }

        if (!jsonData.IsArray)
            return null;
        JsonData jd = new JsonData();
        jd.SetJsonType(JsonType.Array);
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (match(jsonData[i]))
                jd.Add(jsonData[i]);
        }

        return jd;
    }

    public static JsonData Remove(this JsonData jsonData, JsonData item)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return null;
        }

        if (!jsonData.IsArray)
            return jsonData;

        JsonData jd = new JsonData();
        jd.SetJsonType(JsonType.Array);
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (!jsonData[i].Equals(item))
                jd.Add(jsonData[i]);
        }

        return jd;
    }

    public static JsonData RemoveAll(this JsonData jsonData, Predicate<JsonData> match)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return null;
        }

        if (!jsonData.IsArray)
            return jsonData;

        JsonData jd = new JsonData();
        jd.SetJsonType(JsonType.Array);
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (!match(jsonData[i]))
                jd.Add(jsonData[i]);
        }

        return jd;
    }

    public static JsonData RemoveAt(this JsonData jsonData, int index)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return null;
        }

        if (!jsonData.IsArray)
            return jsonData;

        JsonData jd = new JsonData();
        jd.SetJsonType(JsonType.Array);
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (i != index)
                jd.Add(jsonData[i]);
        }

        return jd;
    }

    public static int ArrAdd(this JsonData jsonData, JsonData node)
    {
        if(jsonData.GetJsonType() != JsonType.Array)
        {
            Debug.LogWarning("The jsondata is not array");
            return -1;
        }
        jsonData.Add(node);
        return jsonData.Count;
    }

    public static int ToInt(this JsonData jsonData)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return 0;
        }

        return (int)jsonData;
    }

    public static string ToStr(this JsonData jsonData)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return "";
        }

        return (string)jsonData;
    }

    public static double ToDouble(this JsonData jsonData)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return 0;
        }

        return (double)jsonData;
    }

    public static float ToFloat(this JsonData jsonData)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return 0f;
        }

        return (float)jsonData;
    }

    public static JsonData GetArrayMember(this JsonData jsonData, int index)
    {
        if (jsonData == null)
        {
            Debug.LogWarning("The jsondata is null");
            return null;
        }

        return jsonData[index];
    }

    public static List<T> ToList<T>(this JsonData jsonData)
    {
        if (jsonData.Keys == null)
            return null;

        Dictionary<string, T> dic = JsonMapper.ToObject<Dictionary<string, T>>(jsonData.ToJson());
        return dic.Values.ToList<T>();
    }

}
