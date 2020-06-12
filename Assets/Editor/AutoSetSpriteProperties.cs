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

public class AutoSetSpriteProperties : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        SetAtlasImporterSettings();
    }

    private void SetAtlasImporterSettings()
    {
        string dirName = System.IO.Path.GetDirectoryName(assetPath);
        dirName = dirName.Replace("\\", "/");
        if (!dirName.Contains("Resources/Images"))
            return;

        TextureImporter textureImporter = (TextureImporter)assetImporter;
        if (!string.IsNullOrEmpty(textureImporter.spritePackingTag))
            return;

        textureImporter.textureType = TextureImporterType.Sprite;

        string folderStr = System.IO.Path.GetFileName(dirName);
        textureImporter.spritePackingTag = folderStr;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        textureImporter.wrapMode = TextureWrapMode.Clamp;
        textureImporter.spritePixelsPerUnit = 100;
        textureImporter.mipmapEnabled = false;
        textureImporter.alphaIsTransparency = true;

        TextureImporterSettings textureSettings = new TextureImporterSettings();
        textureImporter.ReadTextureSettings(textureSettings);
        textureSettings.spriteMeshType = SpriteMeshType.FullRect;
        textureImporter.SetTextureSettings(textureSettings);

        GetAtlasImporterSettings(textureImporter);
    }

    private void GetAtlasImporterSettings(TextureImporter textureImporter)
    {

        //android
        TextureImporterPlatformSettings android_settings = new TextureImporterPlatformSettings();
        android_settings.overridden = true;
        android_settings.allowsAlphaSplitting = true;
        android_settings.maxTextureSize = 2048;
        android_settings.format = TextureImporterFormat.ETC2_RGBA8;
        android_settings.name = "Android";
        android_settings.textureCompression = TextureImporterCompression.Compressed;

        textureImporter.SetPlatformTextureSettings(android_settings);

        //ios
        //TextureImporterPlatformSettings ios_settings = new TextureImporterPlatformSettings();
        //ios_settings.overridden = true;
        //ios_settings.allowsAlphaSplitting = true;
        //ios_settings.maxTextureSize = 2048;
        //ios_settings.format = TextureImporterFormat.ASTC_RGBA_4x4;
        //ios_settings.name = "iPhone";
        //ios_settings.textureCompression = TextureImporterCompression.Compressed;

        //textureImporter.SetPlatformTextureSettings(ios_settings);
    }
}
