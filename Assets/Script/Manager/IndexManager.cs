using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System;

public class IndexManager: Singleton<IndexManager>
{
    //存储所有“UI窗体预设(Prefab)”路径
    //参数含义： 第1个string 表示“窗体预设”名称，后一个string 表示对应的路径
    private JsonData _prefabIndexes;

    private JsonData _spriteIndexes;

    public IndexManager()
    {
        var txt = iResourceManager.Load<TextAsset>(SysDefine.SYS_PATH_SpriteConfigJson);
        _spriteIndexes = JsonMapper.ToObject(txt.text);
        txt = iResourceManager.Load<TextAsset>(SysDefine.SYS_PATH_UIFormConfigJson);
        _prefabIndexes = JsonMapper.ToObject(txt.text);
    }

    public string getSpritePath(string name, bool require = true)
    {
        if(name.Contains("/") || name.Contains("\\"))
        {
            return name;
        }
        return _spriteIndexes.GetString(name, require);
    }

    public string getPrefabPath(string name)
    {
        if (name.Contains("/") || name.Contains("\\"))
        {
            return name;
        }   
        return _prefabIndexes.GetString(name);
    }
}
