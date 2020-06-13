using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System;
using System.IO;

public class Config: Singleton<Config>
{
    private const String CONST_CONFIG_NODE_COMMON = "common";

    public JsonData data = new JsonData();
    private JsonData textData;
    public float displayWaitingPanelDelay = 0;
    public void Init()
    {

        TextAsset txt = iResourceManager.Load<TextAsset>(SysDefine.SYS_PATH_ConfigJson);
        JsonData _configIndexes = JsonMapper.ToObject(txt.text);

        foreach (string name in _configIndexes.Keys)
        {
            string resourcePath = _configIndexes.GetString(name);
            TextAsset t = iResourceManager.Load<TextAsset>(resourcePath);
            data[name] = JsonMapper.ToObject(t.text);
        }
    }


    public override string ToString()
    {
        return data == null ? "no data" : data.ToJson();
    }

    public string GetText(string key, string defaultValue = "")
    {
        if (Instance.data != null && Instance.data.ContainsKey("text") && Instance.data["text"].ContainsKey(key))
        {
            return Instance.data["text"][key]["default"].ToString();
        }
        return defaultValue;
    }

    public string app_version
    {
        get { return Application.version; }
    }

    public JsonData commonNode
    {
        get { return data != null ? data[CONST_CONFIG_NODE_COMMON] : null; }
    }

    public JsonData GetConfig(string configName)
    {
        if (data.ContainsKey(configName))
        {
            return data[configName];
        }
        Debug.LogWarning("can not find config:" + configName);
        return new JsonData();
    }

    public LevelConfig[] GetLevelConfig()
    {
        JsonData data = GetConfig("LevelConfig")["level"];
        return JsonMapper.ToObject<LevelConfig[]>(data.ToJson());
    }

    public LevelConfig GetLevelConfigByLevel(int level)
    {
        LevelConfig[] config = GetLevelConfig();
        if (config.Length == 0)
        {
            Debug.LogWarning("can not find level config");
            return new LevelConfig();
        }
        int index = level - 1;
        index = index % config.Length;
        return config[index];
    }

    public Dictionary<string, LevelSize> GetLevelSizeConfig()
    {
        JsonData data = GetConfig("LevelConfig")["size"];
        return JsonMapper.ToObject<Dictionary<string, LevelSize>>(data.ToJson());
    }

    public LevelSize GetLevelSizeConfigById(string sizeId)
    {
        Dictionary<string, LevelSize> dic = GetLevelSizeConfig();
        if (dic.ContainsKey(sizeId))
        {
            return dic[sizeId];
        }
        Debug.LogWarning("can not find current size:" + sizeId);
        return new LevelSize();
    }
}
