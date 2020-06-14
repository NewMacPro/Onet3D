using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using DG.Tweening.Core;
using LitJson;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class ViewUtils
{
    public static float TILE_WIDTH = 0f;
    public static float TILE_GAP = 0.007f;
    public static float TILE_DEPTH = 0.0121f;
    public static float TILE_HEIGHT = 0f;

    public const int DISCARD_ROW_NUM = 9;


    public static readonly Regex EmojiRegex = new Regex("(\uD83C[\uDDE8-\uDDFF]\uD83C[\uDDE7-\uDDFF])|[\uD800-\uDBFF][\uDC00-\uDFFF]|[\u2600-\u27ff][\uFE0F]|[\u2600-\u27ff]|([\uD800-\uDBFF](?![\uDC00-\uDFFF])|([^\uD800-\uDBFF]|^)[\uDC00-\uDFFF])");

    static ViewUtils() { }

    // 实例化一个prefab
    public static GameObject CreatePrefab(string path)
    {
        string prefabPath = IndexManager.Instance.getPrefabPath(path);
        return UnityEngine.Object.Instantiate(iResourceManager.Load<GameObject>(prefabPath));
    }

    public static GameObject CreatePrefabAndSetParent(Transform parent, string path)
    {
        GameObject obj = CreatePrefab(path);
        if (obj != null)
            obj.transform.SetParent(parent, false);
        return obj;
    }

    // 删除一个节点下的所有子节点
    public static void RemoveAllChildren(Transform transform, string exceptObjName = "")
    {
        if (transform == null)
            return;
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).gameObject.name == exceptObjName)
                continue;

            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public static WaitingPanel ShowWaiting(bool showLogoBg = false, bool needDelayShow = true) {
        return ShowWaiting("", showLogoBg, needDelayShow);
    }

    public static WaitingPanel ShowWaiting(string content, bool showLogoBg = false, bool needDelayShow = true) {
        return UIManager.GetInstance().ShowWaitingPanel(content, showLogoBg, needDelayShow);
    }

    public static void RemoveWaiting() {
        UIManager.GetInstance().HideWaitingPanel();
    }

    public static void RemoveAllWaitings() {
        UIManager.GetInstance().HideWaitingPanel(true);
    }


    /// <summary>
    /// Shows the tips panel.
    /// </summary>
    /// <param name="content">确定按钮显示文本</param>
    /// <param name="confirmAction">确定回调函数</param>
    /// <param name="closeAfterClickComfire">点击确定后是否关闭界面</param>
    public static void ShowTipsPanel(string content, UnityAction confirmAction = null, bool closeAfterClickComfire = true)
    {
        //new TipsView(content, confirmAction, 1, closeAfterClickComfire);
    }

    /// <summary>
    /// 确认取消弹窗
    /// </summary>
    /// <param name="content"></param>
    /// <param name="confirmAction"></param>
    /// <param name="cancelAction"></param>
    public static void ShowConfirmPanel(string content, UnityAction confirmAction, UnityAction cancelAction = null)
    {
        //new TipsView(content, confirmAction, cancelAction);
    }

    //public static void ShowTipsPanel (string content, UnityAction confirmAction, UnityAction<bool> toggleAction = null, string confirmText = "", bool remove_waiting_panel = true, int option = 0) {
    //    new TipsView (content, confirmAction, toggleAction, option);
    //}

    public static void ShowDisappearsTipsView(string content, float showTime = 2, Action callBack = null)
    {
        //new DisappearsTipsView(content, showTime, callBack);
    }

    public static DateTime AddSeconds(DateTime time, int seconds)
    {
        DateTime dt = time.AddSeconds(seconds);

        return dt;
    }

    public static string GetTimeByTimeZone(string time)
    {
        DateTime startTime = DateTime.Parse(time);

        startTime = startTime.AddHours(-8); //按北京时间时区转utc时间
        DateTime localStartTime = startTime.Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)); //按时区调整时间
        string shortTime = localStartTime.ToString("HH:mm");

        return shortTime;
    }

    /// <summary>
    /// 显示 月 日 小时 分钟
    /// </summary>
    /// <param name="time"></param>
    /// <param name="option">0 月 日；1 年 月；2 月; 3 月 日 小时 分钟; 4 年-月-日 小时：分钟; 5年 月 日； 6 年-月-日</param>
    /// <returns></returns>
    public static string GetFormatDateTimeString(long ticks, int option)
    {

        DateTime dt = new DateTime(ticks);
        // DateTime dt = new DateTime(ticks * 10000); //奈秒
        // DateTime dtd = new DateTime(ticks);
        return GetFormatDateString(dt, option);
    }

    /// <summary>
    /// Gets the format date string.
    /// </summary>
    /// <returns>The format date string.</returns>
    /// <param name="dt">Dt.</param>
    /// <param name="option">0 月 日；1 年 月；2 月; 3 月 日 HH:mm; 4 年-月-日 小时：分钟; 5年 月 日； 6 年-月-日</param>
    public static string GetFormatDateString(DateTime dt, int option)
    {
        string str = "";

        if (option == 0)
            str = dt.Month + "月" + dt.Day + "日";

        if (option == 1)
            str = dt.Year + "年" + dt.Month + "月";

        if (option == 2)
            str = dt.Month + "月";

        if (option == 3)
            str = dt.Month + "月" + dt.Day + "日" + " " + dt.ToString("HH:mm");

        if (option == 4)
            str = string.Format("{0}-{1}-{2} {3}", dt.Year, dt.Month, dt.Day, dt.ToString("HH:mm"));

        if (option == 5)
            str = string.Format("{0}年{1}月{2}日", dt.Year, dt.Month, dt.Day);

        if (option == 6)
            str = string.Format("{0}-{1}-{2}", dt.Year, dt.Month, dt.Day);
        return str;
    }

    public static string GetItemTimeString(int hours)
    {
        if (hours >= 24)
            return (hours / 24).ToString() + "天";

        return hours + "小时";
    }

    public static string FixLengthString(string str, int fixed_length)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        float length = 0, i_length = 0;
        int index = 0;

        for (int i = 0; i < str.Length; i++)
        {
            int x = (int)str[i];
            length += 1.5f;
            if (x > 0x100)
                length += 0.5f;

            if (length + 2f <= fixed_length)
            {
                i_length = length;
                index = i;
            }
        }

        string ret = str;
        if (length > fixed_length)
        {
            ret = str.Substring(0, index + 1) + "..";
            if (i_length + 2 < fixed_length)
                ret += ".";
            return ret;
        }

        //		for (int i = length; i < fixed_length; i++)
        //			ret += " ";
        return ret;
    }

    /// <summary>
    /// 添加正负符号
    /// </summary>
    /// <returns>The signed number string.</returns>
    /// <param name="num">Number.</param>
    public static string GetSignedNumString(int num)
    {
        if (num >= 0)
            return "+" + num;

        return num.ToString();
    }

    /// <summary>
    /// 添加正负符号
    /// </summary>
    /// <returns>The signed number string.</returns>
    /// <param name="num">Number.</param>
    public static string GetSignedNumString(string num)
    {
        if (num == "--")
            return num;

        int _num = int.Parse(num);

        if (_num >= 0)
            return "+" + _num;

        return _num.ToString();
    }

    public static double GetNowSecond()
    {
        return GetXDaysSecondFromNow(0);
    }

    public static double GetXDaysSecondFromNow(int days)
    {
        TimeSpan timeSpan = DateTime.Now.AddDays(-days) - DateTime.Parse("1970-01-01 00:00:00");
        // Debug.Log(" timeSpan.TotalSeconds:" +  timeSpan.TotalSeconds);
        return timeSpan.TotalSeconds;
    }

    public static string QuoteColor(string str, string color)
    {
        return string.Format("<color={0}>{1}</color>", color, str);
    }

    public static string QuoteColor(int value, string color)
    {
        return string.Format("<color={0}>{1}</color>", color, value.ToString());
    }

    public static Color HexRGBA2Color(string hex)
    {

        // Color color;
        // ColorUtility.TryParseHtmlString(hex, out color);

        // return color;

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, a);
    }

    private static DateTime log_time = DateTime.Now;

    public static void LogDeltime(bool inited = false)
    {
        if (inited)
        {
            log_time = DateTime.Now;
            return;
        }

        LogDeltime("");
    }

    public static void LogDeltime(string msg)
    {
        TimeSpan ts = DateTime.Now - log_time;
        TestLog(msg + ' ' + ts.Ticks.ToString());
        TestLog(msg + ' ' + ts.TotalSeconds.ToString());
        log_time = DateTime.Now;
    }

    public static T CreateMonoClassObj<T>(string path) where T : MonoBehaviour
    {
        GameObject rawObj = CreatePrefab(path);
        T obj = rawObj.AddComponent<T>();
        return obj;
    }

    public static string GetShortenDigitNumString(float wealth)
    {

        if (wealth < 10000)
            return wealth.ToString("f0");

        if (wealth >= 10000 && wealth < 100000)
            return Math.Truncate((wealth / 10000f) * 100) / 100 + "万";

        if (wealth >= 100000 && wealth < 1000000)
            return Math.Truncate((wealth / 10000f) * 10) / 10 + "万";

        return (wealth / 10000f).ToString("f0") + "万";
    }

    public static string NumberToChinese(int number)
    {
        string res = string.Empty;
        string str = number.ToString();
        string schar = str.Substring(0, 1);
        switch (schar)
        {
            case "1":
                res = "一";
                break;

            case "2":
                res = "二";
                break;

            case "3":
                res = "三";
                break;

            case "4":
                res = "四";
                break;

            case "5":
                res = "五";
                break;

            case "6":
                res = "六";
                break;

            case "7":
                res = "七";
                break;

            case "8":
                res = "八";
                break;

            case "9":
                res = "九";
                break;

            default:
                res = "零";
                break;
        }

        if (str.Length > 1)
        {
            switch (str.Length)
            {
                case 2:
                case 6:
                    res += "十";
                    break;

                case 3:
                case 7:
                    res += "百";
                    break;

                case 4:
                    res += "千";
                    break;

                case 5:
                    res += "万";
                    break;

                default:
                    res += "";
                    break;
            }

            res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
        }

        return res;
    }

    public static Texture2D ScaleTexture(Texture2D source, int width, int height)
    {
        Texture2D result = new Texture2D(width, height, source.format, false);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();

        return result;
    }

    public static bool IsMobilePlatform()
    {
        return (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
    }

    public static int ParseTimeStrToSceconds(string s)
    {
        string[] sArr = s.Split(':');
        int seconds = 0;

        if (sArr == null || sArr.Length < 2)
            return seconds;

        sArr[0] = sArr[0].Substring(sArr[0].Length - 2);
        sArr[1] = sArr[1].Substring(0, 2);

        for (int i = 0; i < sArr.Length; i++)
        {
            if (i == 0)
                seconds += int.Parse(sArr[0]) * 3600;

            if (i == 1)
                seconds += int.Parse(sArr[1]) * 60;
        }

        return seconds;
    }

    public static Button InitBtn(Transform root, string path, UnityAction action)
    {
        Button btn = root.Find(path).GetComponent<Button>();
        btn.onClick.AddListener(action);

        return btn;
    }

    public static void RefreshRedSpot(Transform parent, bool isShow)
    {
        if (isShow)
            CreateAndSetRedSpot(parent);
        else
            RemoveRedSpot(parent);
    }

    public static Transform CreateRedSpot(string text, bool showPattern = false)
    {
        Transform redSpot = ViewUtils.CreatePrefab(("RedSpot")).transform;
        if (showPattern)
            redSpot.Find("Pattern").gameObject.SetActive(true);
        else
        {
            Text label = redSpot.Find("Label").GetComponent<Text>();
            if (text != "")
            {
                label.text = text;
            }
            else
            {
                label.gameObject.SetActive(false);
            }
        }

        return redSpot;
    }

    public static void SetActive(Transform parent, string path, bool isActive) {
        parent.FindAChild(path).gameObject.SetActive(isActive);
    }

    public static void CreateAndSetRedSpot(Transform parent)
    {
        // Transform RedSpotClone = parent.Find ("RedSpot(Clone)");
        // if (RedSpotClone != null)
        //     return;
        Vector3 position = Vector3.zero;
        CreateAndSetRedSpot(parent, position);
    }

    public static void CreateAndSetRedSpot(Transform parent, Vector3 position)
    {

        if (parent.Find("RedSpot") != null)
        {
            parent.Find("RedSpot").gameObject.SetActive(true);
        }
        else
        {
            Transform redSpot = CreateRedSpot("");
            redSpot.SetParent(parent, false);
            redSpot.name = "RedSpot";
            if (position == Vector3.zero)
            {
                Transform redSpotNode = parent.Find("RedSpotNode");
                if (redSpotNode != null)
                    redSpot.localPosition = redSpotNode.localPosition;

            }
            else
                redSpot.localPosition = position;
        }
    }

    public static void RemoveRedSpot(Transform parent)
    {

        Transform redSpot = parent.Find("RedSpot");
        if (redSpot != null)
        {
            redSpot.gameObject.SetActive(false);
            return;
        }

        Transform redSpotNode = parent.Find("RedSpot(Clone)");
        if (redSpotNode != null)
        {
            GameObject.Destroy(redSpotNode.gameObject);
        }
    }

    public static bool CompareToCurrentTime(long milSeconds, int dSeconds)
    {
        DateTime dt = new DateTime(1970, 1, 1).AddMilliseconds(milSeconds);
        return CompareToCurrentTime(dt, dSeconds);
    }

    public static bool CompareToCurrentTime(DateTime time, int dSeconds)
    {
        TimeSpan ts = time - DateTime.UtcNow;
        if (ts.TotalSeconds < dSeconds)
            return false;

        return true;
    }

    public static string ParseTimeStrRemoveSceconds(string s)
    {
        if (s == null)
        {
            return "";
        }
        string[] sArr = s.Split(':');

        if (sArr == null || sArr.Length == 0)
            return "00:00";

        if (sArr.Length < 2)
            return sArr[0] + ":00";

        return sArr[0] + ":" + sArr[1];
    }
        
    public static float LengthOfText(string text, float chineseChar = 2f)
    {
        float length = 0;
        foreach (char item in text)
        {
            if (Regex.IsMatch(item.ToString(), @"^[\u4e00-\u9fa5]+$"))
                length += chineseChar;
            else
                length += 1.0f;
        }
        return length;
    }

    public static Vector2 GetUICanvasReferenceRect()
    {
        GameObject canvasObj = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);
        if (canvasObj == null)
            return Vector2.zero;

        return canvasObj.GetComponent<CanvasScaler>().referenceResolution;
    }

    public static bool NameMatch(string input)
    {
        string pattern = @"^[\u4e00-\u9fa5A-Za-z0-9]+$";
        return Regex.IsMatch(input, pattern);
    }

    public static bool CheckName(string name, bool hasSpace = true)
    {
        name = name.ToLower();
        List<string> errNameArr = LoadErrNameText();
        if (!hasSpace)
            errNameArr.RemoveAll(v => v.Equals(" "));
        foreach (string item in errNameArr)
        {
            string str = item.ToLower();
            int index = name.IndexOf(str);
            if (index != -1)
                return false;
        }
        return true;
    }

    public static bool CheckNullObj(UnityEngine.Object obj)
    {
        return obj == null;
    }

    public static RenderTexture CreateRenderTexture(int width, int height, int depth)
    {
        return new RenderTexture(width, height, depth, RenderTextureFormat.ARGB32);
    }

    public static void SetVideoRenderTexture(GameObject parent, string path, RenderTexture texture)
    {
        VideoPlayer player = parent.transform.Find(path).GetComponent<VideoPlayer>();
        if (player != null)
        {
            player.targetTexture = texture;
        }
    }
    public static void SetRawImage(GameObject parent, string path, Texture texture)
    {
        RawImage com = parent.transform.Find(path).GetComponent<RawImage>();
        if (com != null)
        {
            com.texture = texture;
        }
    }

    public static string GetPlayerName(string name, int length = 12)
    {
        return RemoveEmojiFromString(FixLengthString(name, length));
    }

    public static string MinutesToHour(int minutes)
    {
        return string.Format("{0}小时", (minutes / 60.0).ToString("0.#"));
    }

    public static string GetFloatNum(float num)
    {
        return num.ToString("0.#");
    }

    public static string GetAcceleratorDegree(int degree)
    {
        return ((degree + 100) / 100.0).ToString("0.#");
    }

    private static List<string> LoadErrNameText()
    {
        string text = iResourceManager.Load<TextAsset>("JsonData/Text/sensitive").text;
        if (text == null)
            return new List<string>();

        Dictionary<string, List<string>> words = JsonMapper.ToObject<Dictionary<string, List<string>>>(text);
        return words["words"];
    }

    /// <summary>
    /// Logs message to the Unity Console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="option">1 Log,2 LogWarning,3 LogError</param>
    public static void TestLog(object message, int option = 1)
    {
        //#if (TEST_DEBUG)
        try
        {
            switch (option)
            {
                case 1:
                    Debug.Log(message);
                    break;
                case 2:
                    Debug.LogWarning(message);
                    break;
                case 3:
                    Debug.LogError(message);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        //#endif
    }

    /// <summary>
    /// Logs a formatted message to the Unity Console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">Format arguments.</param>
    /// <param name="option">1 LogFormat,2 LogWarningFormat,3 LogErrorFormat</param>
    public static void TestLogFormat(int option, string format, params object[] args)
    {
#if (TEST_DEBUG)
        try {
            switch (option) {
                case 1:
                    Debug.LogFormat (format, args);
                    break;
                case 2:
                    Debug.LogWarningFormat (format, args);
                    break;
                case 3:
                    Debug.LogErrorFormat (format, args);
                    break;
            }
        } catch (Exception ex) {
            Debug.Log (ex.Message);
        }
#endif
    }

    /// <summary>
    /// Assert a condition and logs an error message to the Unity console on failure.
    /// </summary>
    /// <param name="condition">Condition you expect to be true.</param>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void TestLogAssert(bool condition, object message)
    {
#if (TEST_DEBUG)
        try {
            Debug.Assert (condition, message);
        } catch (Exception ex) {
            Debug.Log (ex.Message);
        }
#endif
    }

    public static bool IsIpad()
    {
        float ratio = (float)Screen.width / (float)Screen.height;

        if (ratio < 1.55f)
            return true;

        return false;
    }

    public static string GetRomanNumber(int n)
    {
        int[] arabic = new int[13];
        string[] roman = new string[13];
        int i = 0;
        string o = "";

        arabic[0] = 1000;
        arabic[1] = 900;
        arabic[2] = 500;
        arabic[3] = 400;
        arabic[4] = 100;
        arabic[5] = 90;
        arabic[6] = 50;
        arabic[7] = 40;
        arabic[8] = 10;
        arabic[9] = 9;
        arabic[10] = 5;
        arabic[11] = 4;
        arabic[12] = 1;

        roman[0] = "M";
        roman[1] = "CM";
        roman[2] = "D";
        roman[3] = "CD";
        roman[4] = "C";
        roman[5] = "XC";
        roman[6] = "L";
        roman[7] = "XL";
        roman[8] = "X";
        roman[9] = "IX";
        roman[10] = "V";
        roman[11] = "IV";
        roman[12] = "I";

        while (n > 0)
        {
            while (n >= arabic[i])
            {
                n = n - arabic[i];
                o = o + roman[i];
            }
            i++;
        }
        return o;
    }

    public static bool IsAround16To10()
    {
        float ratio = (float)Screen.width / (float)Screen.height;

        if (ratio >= 1.55f && ratio <= 1.62f)
            return true;

        return false;
    }
    /// <summary>
    /// 字符串转为UniCode码字符串
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string StringToUnicode(string s)
    {
        char[] charbuffers = s.ToCharArray();
        byte[] buffer;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < charbuffers.Length; i++)
        {
            buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
            sb.Append(String.Format("/u{0:X2}{1:X2}", buffer[1], buffer[0]));
        }
        return sb.ToString();
    }
    /// <summary>
    /// Unicode字符串转为正常字符串
    /// </summary>
    /// <param name="srcText"></param>
    /// <returns></returns>
    public static string UnicodeToString(string srcText)
    {
        string dst = "";
        string src = srcText;
        int len = srcText.Length / 6;
        for (int i = 0; i <= len - 1; i++)
        {
            string str = "";
            str = src.Substring(0, 6).Substring(2);
            src = src.Substring(6);
            byte[] bytes = new byte[2];
            bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
            bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
            dst += Encoding.Unicode.GetString(bytes);
        }
        return dst;
    }

    public static string RemoveEmojiFromString(string oriStr)
    {
        if (string.IsNullOrEmpty(oriStr))
            return oriStr;

        return Regex.Replace(oriStr, @"\p{Cs}", "");
    }

    public static Vector2 GetUISizeInWorld(RectTransform uiRectTrans)
    {
        Vector3[] corners = new Vector3[4];
        uiRectTrans.GetWorldCorners(corners);
        Vector3 leftBotCorner = corners[0];
        Vector3 rightTopCorner = corners[2];

        Camera camera = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_UICAMERA)
            .GetComponent<Camera>();

        Vector3 leftBotScreenCorner = camera.WorldToScreenPoint(leftBotCorner);
        Vector3 rightTopScreenCorner = camera.WorldToScreenPoint(rightTopCorner);

        float width = rightTopScreenCorner.x - leftBotScreenCorner.x;
        float height = rightTopScreenCorner.y - leftBotScreenCorner.y;

        return new Vector2(width, height);
    }

    public static void ForceLayout(GameObject obj)
    {
        RectTransform transform = obj.transform as RectTransform;
        if (transform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
        }
    }

    public static void SetGrayScale(Transform node, string path, float grayScale)
    {
        Material grayScaleMat = iResourceManager.Load<Material>("Materials/UI-Grayscale");
        Image icon = node.FindAChild<Image>(path);
        icon.material = GameObject.Instantiate<Material>(grayScaleMat);
        icon.material.SetFloat("_GrayscaleAmount", grayScale);
    }

    public static string GenerateUUID()
    {
        string uuid = System.Guid.NewGuid().ToString("N");
        return uuid;
    }

    public static void AddButtonClick(Transform parent, string path, UnityAction action )
    {
        Button com = parent.FindAChild<Button>(path);
        if (com != null && com.onClick != null)
        {
            com.onClick.RemoveAllListeners();
            com.onClick.AddListener(() => {
                try
                {
                    action.Invoke();
                    //if (ConfigManager.ButtonInterval <= 0) {
                    //    action.Invoke();
                    //}
                    //else {
                    //    // 防止连点
                    //    if (ContinuousEventBlocker.Trigger("ButtonClick" + com.GetInstanceID(), 100)) {
                    //        action.Invoke();
                    //    }
                    //}
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }
    }

    public static void SetText(Transform parent, string path, string text) {
        Text com = parent.FindAChild<Text>(path);
        if (com != null)
        {
            com.text = text;
        }
    }

    public static void SetImage(Transform parent, string path, string spriteName)
    {
        Image com = parent.FindAChild<Image>(path);
        if (com != null)
        {
            com.sprite = iResourceManager.LoadSprite(spriteName);
        }
    }

}
