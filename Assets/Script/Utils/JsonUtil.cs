using UnityEngine;
using System.Text;
using System.IO;
using LitJson;

public static class JsonUtil
{
    public static JsonData CreateJsonArray()
    {
        JsonData jd = new JsonData();
        jd.SetJsonType(JsonType.Array);
        return jd;
    }

    private static JsonData EmptyData()
    {
        JsonData jd = new JsonData();
        jd.SetJsonType(JsonType.Object);
        return jd;
    }

    public static void AddUnderDataNode(this JsonData jd, string key, object obj)
    {
        if (!jd.ContainsKey("data"))
            jd["data"] = new JsonData();

        if (obj is int)
            jd["data"][key] = (int)obj;
        else if (obj is string)
            jd["data"][key] = (string)obj;
        else if (obj is float)
            jd["data"][key] = (float)obj;
        else if (obj is bool)
            jd["data"][key] = (bool)obj;
        else if (obj is JsonData)
            jd["data"][key] = (JsonData)obj;
    }

    public static void SetValue(this JsonData jd, string key, object obj)
    {
        if (obj is int)
            jd[key] = (int)obj;
        else if (obj is string)
            jd[key] = (string)obj;
        else if (obj is float)
            jd[key] = (float)obj;
        else if (obj is bool)
            jd[key] = (bool)obj;
        else if (obj is JsonData)
            jd[key] = (JsonData)obj;
    }

    public static void WriteJson(string path, JsonData json, bool prettyPrint = false)
    {
        StringBuilder buff = new StringBuilder();
        JsonWriter writer = new JsonWriter(buff);
        writer.PrettyPrint = prettyPrint;
        json.ToJson(writer);
        File.WriteAllText(path, buff.ToString().Trim(), Encoding.UTF8);
    }

    public static JsonData ReadJson(string path)
    {
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path, Encoding.UTF8);
            return JsonMapper.ToObject(data.Trim());
        }
        return null;
    }
    public static JsonData GetJson(JsonData json, string key, bool ignoreError = false)
    {
        try
        {
            if (json != null)
            {
                return json[key];
            }
            else
            {
                Debug.LogError("Json data is null");
            }
        }
        catch (System.Exception)
        {
            if (!ignoreError)
            {
                Debug.LogError("Json key not found: " + key);
            }
        }
        return null;
    }

    public static string GetJsonString(JsonData json, string key, bool ignoreError = false)
    {
        try
        {
            if (json != null)
            {
                return json[key].ToJson();
            }
            else
            {
                Debug.LogError("Json data is null");
            }
        }
        catch (System.Exception)
        {
            if (!ignoreError)
            {
                Debug.LogError("Json key not found: " + key);
            }
        }
        return null;
    }

    public static bool ContainsKey(JsonData json, string key)
    {
        return json.Keys.Contains(key);
    }

    public static string GetString(JsonData json, string key, string defaultValue = "", bool ignoreError = false)
    {
        try
        {
            if (json != null)
            {
                return json[key].ToString();
            }
            else
            {
                Debug.LogWarning("Json data is null");
            }
        }
        catch (System.Exception)
        {
            if (!ignoreError)
            {
                Debug.LogWarning("Json key not found: " + key);
            }
        }
        return defaultValue;
        //return json.Keys.Contains(key) ? json[key].ToString() : string.Empty;
    }

    public static int GetInt(JsonData json, string key, int defaultValue = 0, bool ignoreError = false)
    {
        int value = defaultValue;
        string str = GetString(json, key, ignoreError: ignoreError);
        if (int.TryParse(str, out value) == false)
        {
            if (!ignoreError)
            {
                Debug.LogError(string.Concat(str, " is not a int, key:", key));
            }
            return defaultValue;
        }
        return value;
    }

    public static long GetLong(JsonData json, string key, long defaultValue = 0, bool ignoreError = false)
    {
        long value = defaultValue;
        string str = GetString(json, key, ignoreError: ignoreError);
        if (long.TryParse(str, out value) == false)
        {
            if (!ignoreError)
            {
                Debug.LogError(string.Concat(str, " is not a long, key:", key));
            }
            return defaultValue;
        }
        return value;
    }

    public static bool GetBool(JsonData json, string key, bool defaultValue = false, bool ignoreError = false)
    {
        bool value = defaultValue;
        string str = GetString(json, key, ignoreError: ignoreError);
        if (bool.TryParse(str, out value) == false)
        {
            if (!ignoreError)
            {
                Debug.LogError(string.Concat(str, " is not a bool, key:", key));
            }
            return defaultValue;
        }
        return value;
    }

    public static float GetFloat(JsonData json, string key, float defaultValue = 0, bool ignoreError = false)
    {
        float value = defaultValue;
        string str = GetString(json, key, ignoreError: ignoreError);
        if (float.TryParse(str, out value) == false)
        {
            if (!ignoreError)
            {
                Debug.LogError(string.Concat(str, " is not a float, key:", key));
            }
            return defaultValue;
        }
        return value;
    }

    public static double GetDouble(JsonData json, string key, double defaultValue = 0, bool ignoreError = false)
    {
        double value = defaultValue;
        string str = GetString(json, key, ignoreError: ignoreError);
        if (double.TryParse(str, out value) == false)
        {
            if (!ignoreError)
            {
                Debug.LogError(string.Concat(str, " is not a double, key:", key));
            }
            return defaultValue;
        }
        return value;
    }

    public static string GetPrettyPrint(JsonData json)
    {
        StringBuilder buff = new StringBuilder();
        JsonWriter writer = new JsonWriter(buff);
        writer.PrettyPrint = true;
        json.ToJson(writer);
        return buff.ToString();
    }

    public static string GetStrValue(JsonData jd, string key, string defaultStr = "")
    {
        if (jd.ContainsKey(key))
        {
            return jd[key].ToString();
        }
        return defaultStr;
    }

    public static void UpdateValue(string path, string key, JsonData v)
    {
        var jd = ReadJson(path);
        jd[key] = v;
    }

    public static void Union(JsonData src, JsonData dst)
    {
        foreach(string key in dst.Keys)
        {
            if (src.ContainsKey(key))
            {
                Debug.LogWarningFormat("Duplicated key [{0}], Override!", key);
            }
            src[key] = dst[key];
        }
    }
}
