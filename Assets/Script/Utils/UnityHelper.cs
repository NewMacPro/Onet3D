#pragma warning disable 1591
/***
*
*
*           主题： Unity帮助类
*    Description:
*           功能： 提供程序用户常用功能集。
*    Date: 2017
*    Version: 0.1版本
*    Modify Recoder:
*
*
*/
using System;
using System.IO;
using System.Text;
using System.Reflection;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UnityHelper : UnitySingleton<UnityHelper>
{
    //是否是第一次加载游戏,默认是
    // public static bool isFirstLoad = true;

    /// <summary>
    /// 获取指定范围内随机数
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static int GetRandom(int num1, int num2)
    {
        if (num1 < num2)
        {
            return UnityEngine.Random.Range(num1, num2 + 1);
        }
        else
        {
            return UnityEngine.Random.Range(num2, num1 + 1);
        }
    }

    /// <summary>
    /// 清理内存(一般在切换场景的时候调用)
    /// </summary>
    public static void ClearMemory()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }

    /// <summary>
    /// 获取子物体的脚本
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            return searchTrans.gameObject.GetComponent<T>();
        }
        else
        {
            // Debug.LogWarning("Cant Find This Child: " + childName);
            return null;
        }
    }

    /// <summary>
    /// 给子物体添加脚本
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T AddTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            T[] theComponentsArr = searchTrans.GetComponents<T>();
            for (int i = 0; i < theComponentsArr.Length; i++)
            {
                if (theComponentsArr[i] != null)
                {
                    Destroy(theComponentsArr[i]);
                }
            }
            return searchTrans.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 给子物体添加父对象
    /// </summary>
    /// <param name="parentTrs">父对象的方位</param>
    /// <param name="childTrs">子对象的方位</param>
    public static void AddChildToParent(Transform parentTrs, Transform childTrs)
    {
        //childTrs.parent = parentTrs; //Original Method
        childTrs.SetParent(parentTrs, false);
        childTrs.localPosition = Vector3.zero;
        childTrs.localScale = Vector3.one;
        childTrs.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 检查是否点击到了这个控件区域
    /// </summary>
    /// <param name="responseArea">检查区域控件</param>
    /// <returns></returns>
    public static bool IsTouchIn(Transform responseArea)
    {
        GameObject clickTarget = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickTarget == null)
            return false;

        return responseArea != null ? clickTarget.transform.IsChildOf(responseArea) : false;
    }


    public static string GetScreenshotPath()
    {
        return Application.persistentDataPath + "/ScreenShot.png";
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary>
    /// 字符串转Unicode
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <returns>Unicode编码后的字符串</returns>
    public static string String2Unicode(string source)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(source);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i += 2)
        {
            stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
        }
        return stringBuilder.ToString();
    }

    public static string GetNetWorkTypeName()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                return "运营商网路";
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                return "wifi";
            default:
                return "获取网路信息错误";
        }
    }

    public static string GetDeviceName()
    {
        return SystemInfo.deviceName;
    }

    public static string GetDeviceSystem()
    {
        return SystemInfo.operatingSystem;
    }

    #region 平台获取
    public static bool IsAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }

    public static bool IsIOS()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
    }

    public static bool IsEditor()
    {
        return Application.isEditor;
    }

    public static string GetPlatformName()
    {
        if (IsAndroid())
        {
            return "Android";
        }
        else if (IsIOS())
        {
            return "iOS";
        }
        else if (IsEditor())
        {
            return "editor";
        }

        return "default";
    }

    public static string GetActivePlatformName()
    {
#if UNITY_ANDROID
        return "Android";
#elif UNITY_IPHONE
            return "iOS";
#else
            return "UnKnow";
#endif
    }

    public static bool IsActiveAndroid()
    {
#if UNITY_ANDROID
        return true;
#else
        return false;
#endif
    }

    public static bool IsActiveIos()
    {
#if UNITY_IPHONE
        return true;
#else
        return false;
#endif
    }


    #endregion

    //版本号对比
    public static bool VersionMatching(string theOperator, string theVersion)
    {
        string[] cVersionArr = Application.version.Split('.');
        string[] sVersionArr = theVersion.Split('.');

        switch (theOperator)
        {
            case ">":
            case ">=":
                for (int i = 0; i < cVersionArr.Length; i++)
                {
                    int cVersion;
                    int sVersion;
                    bool cValue = int.TryParse(cVersionArr[i], out cVersion);
                    bool sValue = int.TryParse(sVersionArr[i], out sVersion);
                    if (!cValue || !sValue)
                        return false;
                    if (cVersion > sVersion)
                    {
                        return true;
                    }
                    if (cVersion < sVersion)
                    {
                        return false;
                    }
                }
                if (theOperator == ">=")
                    return Application.version == theVersion;
                return false;
            case "<":
            case "<=":
                for (int i = 0; i < cVersionArr.Length; i++)
                {
                    int cVersion;
                    int sVersion;
                    bool cValue = int.TryParse(cVersionArr[i], out cVersion);
                    bool sValue = int.TryParse(sVersionArr[i], out sVersion);
                    if (!cValue || !sValue)
                        return false;
                    if (cVersion < sVersion)
                        return true;
                    if (cVersion > sVersion)
                    {
                        return false;
                    }
                }
                if (theOperator == "<=")
                    return Application.version == theVersion;
                return false;
            case "==":
            case "=":
                return Application.version == theVersion;
            default:
                return Application.version == theVersion;
        }
    }

    public static bool VersionMatching(string version)
    {
        char[] operators = new char[] { '>', '<', '=' };
        int index = version.LastIndexOfAny(operators);
        string theOperator = version.Substring(0, index + 1);
        string theVersion = version.Substring(index + 1, version.Length - index - 1);
        return VersionMatching(theOperator, theVersion);
    }

    public static float BatteryLevel()
    {
        if (IsEditor())
            return 1;
        return SystemInfo.batteryLevel;
    }

    public static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }


    public static string GetPlatformPersistentDataPath()
    {
        return string.Format("{0}/{1}", Application.persistentDataPath, GetActivePlatformName());
    }

    public static void LogToTxt(string content, string textFileName = "MyDebug.txt")
    {
        if (!IsEditor())
            return;

        string tmpPath = Application.dataPath + "/../HiddenFolder/" + textFileName;


        FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.Write);
        fs.Flush();
        StreamWriter bw = new StreamWriter(fs);
        bw.Write(content);
        bw.Close();

        fs.Close();
    }


}//Class_end
