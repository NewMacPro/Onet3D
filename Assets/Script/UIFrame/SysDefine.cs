using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 系统枚举
/// <summary>
/// UI窗体类型
/// </summary>
public enum UIFormsType
{
    Normal,             // 普通全屏界面(例如主城UI界面)
    Fixed,              // 固定界面(例如“英雄信息条” [HeroTopBar])
    Middle,             // 中间层
    PopUp,              // 弹出模式(小窗口)窗口 (例如：商场、背包、确认窗口等)
    //Notice,             // 弹出模式通知类窗口，一次只能弹出一个
    NoviceGuide,        // 新手引导
    Front,              // 一般界面最前
    Waiting,            // 等待菊花 在所有最前
}

/// <summary>
/// UI窗体显示类型
/// </summary>
public enum UIFormsShowMode
{
    Normal,             //普通显示
    ReverseChange,      //反向切换
    HideOther,          //隐藏其他界面
    PopUp,              //弹出方式
    Mixed,
    NoviceGuide,        // 新手引导
    Simple,             // 简单形弹窗，不会有特别互动

}

/// <summary>
/// UI窗体透明度类型
/// </summary>
public enum UIFormsLucencyType
{
    Lucency,            //完全透明,但不能穿透。
    Translucence,       //半透明度,不能穿透。
    Impenetrable,       //低透明度,不能穿透,
    Penetrate,          //可以穿透
    OpaqueBlack         //纯黑不能穿透
}
#endregion

/// <summary>
/// 系统定义静态类
/// </summary>
public static class SysDefine
{
    #region 系统常量
    /* 配置常量 */

    /* 路径常量 */
    public const string SYS_PATH_CANVAS = "Prefabs/UIBase/UICanvas";
    public const string SYS_PATH_UICAMERA = "Prefabs/UIBase/UICamera";
    
    public const string SYS_PATH_UIFormConfigJson = "Config/Index/PrefabPathJson";
    public const string SYS_PATH_SpriteConfigJson = "Config/Index/SpriteIndex";
    public const string SYS_PATH_ConfigJson = "Config/Index/ConfigIndex";

    /* 标签常量 */
    public const string SYS_TAG_CANVAS = "_TagCanvas";
    public const string SYS_TAG_UICAMERA = "_TagUICamera";
    public const string SYS_TAG_GAMECONTROLLER = "_TagGameController";
    public const string SYS_TAG_NOREFLECTIONTILE = "_TagNoReflectionTile";
    public const string SYS_TAG_CLONETILE = "_TagCloneTile";
    public const string SYS_TAG_GUIDEZKEEPUI = "_TagGuideZKeepUI";
    /* Canvas节点名称 */
    public const string SYS_CANVAS_NORMAL_NODE_NAME = "Normal";
    public const string SYS_CANVAS_FIXED_NODE_NAME = "Fixed";
    public const string SYS_CANVAS_POPUP_NODE_NAME = "PopUp";
    public const string SYS_CANVAS_WAITING_NODE_NAME = "Waiting";
    public const string SYS_CANVAS_FRONT_NODE_NAME = "Front";
    public const string SYS_CANVAS_MIDDLE_NODE_NAME = "Middle";
    public const string SYS_CANVAS_NOVICE_NODE_NAME = "Novice";
    public const string SYS_CANVAS_UISCRIPTS_NODE_NAME = "_UIScripts";
    public const string SYS_CANVAS_POPLAYERMASK_NODE_NAME = "PopLayerMask";
    public const string SYS_CANVAS_FIXEDLAYERMASK_NODE_NAME = "FixedLayerMask";
    /* 遮罩管理器常量 */
    //完全透明度
    public const float SYS_UIMASK_LUCENCY_COLOR_RGB = 255F / 255F;
    public const float SYS_UIMASK_LUCENCY_COLOR_A = 0F / 255F;
    //半透明度
    public const float SYS_UIMASK_TRANSLUCENCY_COLOR_RGB = 0F / 255F;
    public const float SYS_UIMASK_TRANSLUCENCY_COLOR_A = 190F / 255F;
    //低透明度
    public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB = 0F;
    public const float SYS_UIMASK_IMPENETRABLE_COLOR_A = 200F / 255F;
    //纯黑色
    public const float SYS_UIMASK_OPAQUE_BLACK_COLOR_RGB = 0F;
    public const float SYS_UIMASK_OPAQUE_BLACK_COLOR_A = 1F;

    /// <summary>
    /// UI摄像机，层深增加量
    /// </summary>
    public const int SYS_UICAMERA_DEPTH_INCREMENT = 100;
    #endregion

    #region 全局性变量（方法）
    //得到日志配置文件(XML)路径
    public static string GetLogPath()
    {
        string logPath = null;

        //Android 或者Iphone 环境
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            logPath = Application.streamingAssetsPath + "/LogConfigInfo.xml";
        }
        //Win环境
        else
        {
            logPath = "file://" + Application.streamingAssetsPath + "/LogConfigInfo.xml";
        }

        return logPath;
    }
    //得到日志配置文件(XML)根节点名称
    public static string GetLogRootNodeName()
    {
        string strReturnXMLRootNodeName = null;

        strReturnXMLRootNodeName = "SystemConfigInfo";
        return strReturnXMLRootNodeName;
    }

    //得到"UI窗体预设"配置文件(XML)路径
    public static string GetUIFormsConfigFilePath()
    {
        string logPath = null;

        //Android 或者Iphone 环境
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            logPath = Application.streamingAssetsPath + "/UIFormsConfigInfo.xml";
        }
        //Win环境
        else
        {
            logPath = "file://" + Application.streamingAssetsPath + "/UIFormsConfigInfo.xml";
        }

        return logPath;
    }
    //得到"UI窗体预设"配置文件(XML)的根节点名称
    public static string GetUIFormsConfigFileRootNodeName()
    {
        string strReturnXMLRootNodeName = null;

        strReturnXMLRootNodeName = "UIFormsConfigInfo";
        return strReturnXMLRootNodeName;
    }


    #endregion

    #region 委托定义

    #endregion

    #region 框架接口

    #endregion
}//Class_end

/// <summary>
/// UI（窗体）类型
/// </summary>
public class UIType
{

    //是否需要清空其他的所有窗体
    public bool IsClearAllOtherView = false;
    //是否需要清空“反向切换”
    public bool IsClearReverseChange = false;

    public bool IsClearPopUp = false;
    //作为normal窗体时，是否需要被压栈
    public bool IsNeedPush = true;
    //popup是否在显示后一个popup时隐藏
    public bool NoHidePopUp = false;
    public bool IsGuide = false;
    //是否可以弹出消息弹窗
    public bool CanPopMessage = false;
    //是否是全局弹窗，不是则在主界面弹出
    public bool IsOverAllPop = true;

    //是否作为最后一个弹窗时被关闭时刷新当前normal界面
    public bool IsRefreshCurrentNormalViewWhenIsLastPopClosed = false;
    //是否绑定到当前界面
    public bool NeedBind = true;
    //是不是大厅界面
    public bool IsLobbyView = false;
    //是不是强制弹出
    public bool IsForcedPop = false;
    //需不需要播放弹出动画
    public bool PopUpNeedAnime = true;
    //新手引导时是否会被隐藏
    public bool IsNeedHideWhenGuiding = true;
    public UIFormsType UIForms_Type = UIFormsType.Normal;
    //UI窗体显示类型
    public UIFormsShowMode UIForms_ShowMode = UIFormsShowMode.Normal;
    //UI窗体透明度类型
    public UIFormsLucencyType UIForms_LucencyType = UIFormsLucencyType.Lucency;
    //是否隐藏普通界面
    public bool IsHideNormalView = false;

    public string LinkUIName = "";
}
