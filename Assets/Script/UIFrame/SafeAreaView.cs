using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class SafeAreaView : MonoBehaviour
{
    // 一下整出N个区域
    public RectTransform[] areas;
    public RectTransform[] maskAreas;

    // 设计分辨率
    public float matchWidth = 1080f;    // UI单位
    public float matchHeight = 2160f;   // UI单位

    public float minRatio = 9f / 18f;    // 限制最小宽高比
    public float maxRatio = 9f / 16f;   // 限制最大宽高比
    public float maxShowWidthRatio = 2.2f;   

    public float scale = 1;   

    private int lastWidth = 0;
    private int lastHeight = 0;

    private void Awake()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        ResetSafeArea();
    }

    private void Update()
    {
        if (lastWidth != Screen.width || lastHeight != Screen.height)
        {
            Debug.Log("Screen resolution changes to: " + Screen.width + "x" + Screen.height);
            ResetSafeArea();
        }
        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }

    private Rect GetSafeArea()
    {
        float x, y, w, h;
        x = 0;
        y = 0;
        w = Screen.width;
        h = Screen.height;
        return new Rect(x, y, w, h);
    }

    private void ResetSafeArea()
    {
        if (areas == null || areas.Length == 0)
        {
            return;
        }
        //Debug.LogFormat("MinRatio : {0}, maxRatio: {1}", minRatio, maxRatio);

        // 实际屏幕宽高比，像素单位比值
        float screenRatio = (float)Screen.width / Screen.height;

        // 安全区域，像素单位
        Rect safeArea = GetSafeArea();
#if UNITY_EDITOR
        // iPhoneX
        if (Screen.width == 1624 && Screen.height == 750)
        {
            safeArea.xMin = 88;
            safeArea.xMax = Screen.width - 68;
        }
#endif
        // 测试
        //safeArea.xMin = 100f;
        //safeArea.xMax = Screen.width - 200;

        // safeArea 宽度可能会小于Screen.width，为了保证设计宽度值matchWidth不变，所以要整体缩小
        scale = safeArea.width / Screen.width;    // 整体缩放值

        // 按宽度适配的实际高度值，UI单位
        float height = matchWidth / screenRatio;
        // 因为整体缩小了，所以高度需要放大才能重新填满屏幕
        height /= scale;

        float minHeight = matchWidth / maxRatio;    // 实际最小高度值，UI单位
        float maxHeight = matchWidth / minRatio;    // 实际最大高度值，UI单位
                                                    // Debug.LogFormat("MinHeight: {0}, MaxHeight: {1}, CurHeight: {2}", minHeight, maxHeight, height);
        if (height < minHeight)
        {
            // 屏幕太扁，需要增加高度到minHeight，这样上下显示区域就会超出屏幕，所以需要整体缩小
            scale *= height / minHeight;
            height = minHeight;
        }
        else if (height > maxHeight)
        {
            // 屏幕太窄，需要减小高度到maxHeight
            height = maxHeight;
        }

        // 因为safeArea宽度左右可能间隔不一样，所以中心坐标需要有偏移
        float left = safeArea.xMin;
        float right = Screen.width - safeArea.xMax;
        Vector2 center = new Vector2((left - right) / Screen.width * matchWidth * 0.5f, 0);

        //Debug.LogFormat("Final wdith: {0}, height: {1}, scale: {2}, ratio: {3}", matchWidth, height, scale, matchWidth / height);
        Vector2 areaSize = new Vector2(matchWidth, height);
        Vector3 areaScale = new Vector3(scale, scale, 1);

        foreach (RectTransform area in areas)
        {
            area.anchorMin = new Vector2(0.5f, 0.5f);
            area.anchorMax = new Vector2(0.5f, 0.5f);

            area.sizeDelta = areaSize;
            area.localScale = areaScale;
            area.anchoredPosition = center;
        }

        StartCoroutine(ResetMaskArea(center, areaSize, areaScale));
    }

    private IEnumerator ResetMaskArea(Vector2 areaPos, Vector2 areaSize, Vector3 areaScale)
    {
        if (maskAreas == null || maskAreas.Length == 0)
        {
            yield break;
        }
        yield return new WaitForEndOfFrame();

        areaSize.x *= areaScale.x;
        areaSize.y *= areaScale.y;

        foreach (RectTransform maskArea in maskAreas)
        {
            Vector2 maskSize = maskArea.rect.size;

#if !UNITY_IOS
            float left = (maskSize.x - areaSize.y * maxShowWidthRatio + areaPos.x) * 0.5f;
            float right = (maskSize.x - areaSize.y * maxShowWidthRatio - areaPos.x) * 0.5f;
            RectTransform leftMask = maskArea.Find("Left") as RectTransform;
            RectTransform rightMask = maskArea.Find("Right") as RectTransform;
            SetWidth(leftMask, left);
            SetWidth(rightMask, right);
#endif

            float top = (maskSize.y - areaSize.y + areaPos.y) * 0.5f;
            float bottom = (maskSize.y - areaSize.y - areaPos.y) * 0.5f;

            RectTransform topMask = maskArea.Find("Top") as RectTransform;
            RectTransform bottomMask = maskArea.Find("Bottom") as RectTransform;

            SetHeight(topMask, top);
            SetHeight(bottomMask, bottom);
        }
    }

    private void SetWidth(RectTransform transform, float width)
    {
        if (width < 0)
            width = 0;
        Vector2 size = transform.sizeDelta;
        size.x = width;
        transform.sizeDelta = size;
    }

    private void SetHeight(RectTransform transform, float height)
    {
        Vector2 size = transform.sizeDelta;
        size.y = height;
        transform.sizeDelta = size;
    }

}
