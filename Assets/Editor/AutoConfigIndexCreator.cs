#region 模块信息
// **********************************************************************
// Copyright (C) 2017 The company name
//
// 文件名(File Name):             AutoSetSpriteProperties.cs
// 作者(Author):                  Tom
// 创建时间(CreateTime):           Tuesday, 09 May 2017
// 修改者列表(modifier):
// 模块描述(Module description):   自动设定导入图片的属性
// **********************************************************************
#endregion

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using LitJson;

public class AutoConfigIndexCreator : AssetPostprocessor
{

    static string ASSETS_PATH = Application.dataPath + "/Resources";
    public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool needUpdate = false;
        //当移动资源的时候  也就是重新导入资源  
        for (int i = 0; i < importedAsset.Length; i++)
        {
            if (CheckIsConfig(importedAsset[i]))
            {
                needUpdate = true;
            }
        }
        if (!needUpdate)
        {
            //删除资源  
            for (int i = 0; i < deletedAssets.Length; i++)
            {
                if (CheckIsConfig(deletedAssets[i]))
                {
                    needUpdate = true;
                }
            }
        }
        if (!needUpdate)
        {
            //移动资源 
            for (int i = 0; i < movedAssets.Length; i++)
            {
                if (CheckIsConfig(movedAssets[i]))
                {
                    needUpdate = true;
                }
            }
        }
        if (needUpdate)
        {
            CreateConfigPathJson();
        }
    }

    private static bool CheckIsConfig(string assetPath)
    {
        string dirName = System.IO.Path.GetDirectoryName(assetPath);
        dirName = dirName.Replace("\\", "/");
        if (!dirName.Contains("Resources/Config"))
            return false;

        return true;
    }

    private static void CreateConfigPathJson()
    {
        JsonData jsonData = new JsonData();

        string objPath = ASSETS_PATH + "/Config";
        DirectoryInfo dir = new DirectoryInfo(objPath);
        FileInfo[] files = dir.GetFiles("*.json", SearchOption.AllDirectories);
        int fileCount = files.Length;
        for (int i = 0; i < fileCount; i++)
        {
            FileInfo item = files[i];
            string fullName = item.FullName.Replace("\\", "/");
            string resourcePath = fullName.Substring(fullName.LastIndexOf("Config/"));
            resourcePath = resourcePath.Remove(resourcePath.LastIndexOf("."));

            if (EditorUtility.DisplayCancelableProgressBar((i + 1) + "/" + fileCount, resourcePath, (float)(i + 1) / fileCount))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string name = Path.GetFileNameWithoutExtension(item.Name);
            if (jsonData.ContainsKey(name))
            {
                Debug.LogErrorFormat("Duplicate name [{0}] and [{1}]", jsonData[name], resourcePath);
            }
            jsonData[name] = resourcePath;
        }
        string jsonStorePath = ASSETS_PATH + "/" + SysDefine.SYS_PATH_ConfigJson + ".json";

        if (File.Exists(jsonStorePath))
        {
            File.Delete(jsonStorePath);
        }

        JsonUtil.WriteJson(jsonStorePath, jsonData);

        //刷新editor
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }
}
