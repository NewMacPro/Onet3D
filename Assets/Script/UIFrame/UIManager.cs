#region 模块信息
// **********************************************************************
// Copyright (C) 2017 The company name
//
// 文件名(File Name):             MyUIManager.cs
// 作者(Author):                  Tom
// 创建时间(CreateTime):           Wednesday, 12 April 2017
// 修改者列表(modifier):
// 模块描述(Module description):   ui管理类
// **********************************************************************
#endregion
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;

public class UIManager : MonoBehaviour
{

    //本类实例
    private static UIManager _Instance = null;
    //缓存所有已经打开的“UI窗体预设(Prefab)”
    //参数含义： 第1个string 表示“窗体预设”名称，后一个BaseUI 表示对应的“窗体预设”
    private static Dictionary<string, UIBase> _DicALLUIForms = new Dictionary<string, UIBase>();
    //"栈"当前所有UI
    private static List<UIBase> _StaNormalUIForms = new List<UIBase>();
    //    private Stack<UIBase> _StaNormalUIForms;

    private static List<UIBase> _StaPopUpForms = new List<UIBase>();
    //当前显示状态的UI窗体集合
    private static Dictionary<string, UIBase> _DicCurrentShowUIForms = new Dictionary<string, UIBase>();
    // - !!!! - UI根节点 ======================================================
    private Transform _CanvasTransform = null;
    //普通全屏界面节点
    private Transform _CanTransformNormal = null;
    //固定界面节点
    private Transform _CanTransformFixed = null;
    //最前界面节点
    private Transform _CanTransformFront = null;
    //中间界面节点
    private Transform _CanTransformMiddle = null;
    //弹出模式节点
    private Transform _CanTransformPopUp = null;
    //新手引导节点
    private Transform _CanTransformNovice = null;
    //数据等待中节点
    private Transform _CanTransformWaiting = null;


    public Transform fadeOutBlackImage;
    // private Transform _Bg = null;

    //UI脚本节点（加载各种管理脚本的节点）
    //    private Transform _CanTransformUIScripts = null;

    //大厅ui的名字，大厅ui界面不销毁
    private const string LobbyUIName = "MainUI";
    // - !!!! - UI摄像机 ======================================================
    public Camera uiCamera;
    // 用于3d对象的摄像机，特别用于renderTexture
    public Camera ui3DCamera;

    // 大厅背景
    public GameObject hallBg;

    public Canvas uiCanvas;
    public UIBase lastNormalUiBase;
    //等待板
    public WaitingPanel _waitingPanel;

    //canvas 的设计尺寸
    public static Vector2 canvasReferenceResolution;

    //基础节点渲染顺序字典
    private Dictionary<UIFormsType, int> baseNodeOrderDic;

    //界面间z轴距离
    public float viewZGap = -200;
    /// <summary>
    /// 得到本类实例
    /// </summary>
    /// <returns></returns>
    public static UIManager GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new GameObject("_UIManager").AddComponent<UIManager>();
            DontDestroyOnLoad(_Instance);
        }
        return _Instance;
    }

    void Awake()
    {
        //初始化项目开始必须的资源加载
        InitRootCanvasLoading();

        //得到UI根节点、及其重要子节点
        _CanvasTransform = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;
        //得到普通全屏界面节点、固定界面节点、弹出模式节点、UI脚本节点
        _CanTransformNormal = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_NORMAL_NODE_NAME);
        _CanTransformPopUp = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_POPUP_NODE_NAME);
        _CanTransformFixed = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_FIXED_NODE_NAME);
        _CanTransformWaiting = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_WAITING_NODE_NAME);
        _CanTransformFront = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_FRONT_NODE_NAME);
        _CanTransformMiddle = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_MIDDLE_NODE_NAME);
        _CanTransformNovice = UnityHelper.FindTheChild(_CanvasTransform.gameObject, SysDefine.SYS_CANVAS_NOVICE_NODE_NAME);
        // _Bg = UnityHelper.FindTheChild(_CanvasTransform.gameObject, "UIBgEffect");
        //初始化“UI窗体预设”路径数据
        InitUIFormsPathsData();
        SetBaseNodeOrderDic();
        InitWaitingPanel();
    }

    void InitWaitingPanel() {
        if (_waitingPanel == null) {
            _waitingPanel = Instantiate(iResourceManager.Load<WaitingPanel>("Prefabs/UIBase/WaitingPanel"));
            _waitingPanel.transform.SetParent(_CanTransformWaiting, false);
        }

    }

    void SetBaseNodeOrderDic()
    {
        baseNodeOrderDic = new Dictionary<UIFormsType, int>();
        baseNodeOrderDic.Add(UIFormsType.Normal, _CanTransformNormal.GetComponent<Canvas>().sortingOrder);
        baseNodeOrderDic.Add(UIFormsType.PopUp, _CanTransformPopUp.GetComponent<Canvas>().sortingOrder);
        baseNodeOrderDic.Add(UIFormsType.Fixed, _CanTransformFixed.GetComponent<Canvas>().sortingOrder);
        baseNodeOrderDic.Add(UIFormsType.Waiting, _CanTransformWaiting.GetComponent<Canvas>().sortingOrder);
        baseNodeOrderDic.Add(UIFormsType.Front, _CanTransformFront.GetComponent<Canvas>().sortingOrder);
        baseNodeOrderDic.Add(UIFormsType.Middle, _CanTransformMiddle.GetComponent<Canvas>().sortingOrder);
        baseNodeOrderDic.Add(UIFormsType.NoviceGuide, _CanTransformMiddle.GetComponent<Canvas>().sortingOrder);
    }

    public int GetBaseNodeSortingOrder(UIFormsType type)
    {
        return baseNodeOrderDic[type];
    }

    void Update()
    {
        //使得当前正常ui 可进行update
        if (_StaNormalUIForms == null || _StaNormalUIForms.Count < 1)
            return;

        UIBase uiBase = _StaNormalUIForms.Peek();
        if (uiBase.IsShowing())
            uiBase.Update();

        if (_StaPopUpForms == null || _StaPopUpForms.Count < 1)
            return;

        UIBase popUIBase = _StaPopUpForms.Peek();
        if (popUIBase.IsShowing())
            popUIBase.Update();
    }

    /// <summary>
    /// 初始化项目开始必须的资源加载
    /// </summary>
    private void InitRootCanvasLoading()
    {
        //创建或者找到UICanvas
        GameObject existCanvas = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);

        if (existCanvas != null)
            uiCanvas = existCanvas.GetComponent<Canvas>();
        else
        {
            uiCanvas = Instantiate(iResourceManager.Load<Canvas>(SysDefine.SYS_PATH_CANVAS));
            //CanvasScaler canvasScaler = uiCanvas.GetComponent<CanvasScaler>();
            //if (canvasScaler != null)
            //{
            //    float matchWidthOrHeight = 0;
            //    if (Math.Round((double)Screen.width / Screen.height, 1) > Math.Round(16.0f / 9.0f, 1))
            //    {
            //        matchWidthOrHeight = 1;
            //    }
            //    canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
            //}
        }

        canvasReferenceResolution = uiCanvas.GetComponent<UnityEngine.UI.CanvasScaler>().referenceResolution;
        DontDestroyOnLoad(uiCanvas);
        //创建或者找到UICamera
        GameObject existUICamera = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_UICAMERA);

        if (existUICamera != null)
            uiCamera = existUICamera.GetComponent<Camera>();
        else
            uiCamera = Instantiate(iResourceManager.Load<Camera>(SysDefine.SYS_PATH_UICAMERA));

        uiCamera.depth = SysDefine.SYS_UICAMERA_DEPTH_INCREMENT;
        uiCanvas.worldCamera = uiCamera;

        ui3DCamera = uiCanvas.transform.FindAChild<Camera>("UI3DMaskCamera");
        DontDestroyOnLoad(uiCamera);

    }

    private void ResetUiCamera()
    {
        uiCamera.gameObject.SetActive(false);
        uiCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// 初始化“UI窗体预设”路径数据
    /// </summary>
    public void InitUIFormsPathsData()
    {
    }

    /// <summary>
    /// Shows the user interface form.
    /// 界面如果存在则重新显示界面，不再attch root
    /// </summary>
    /// <returns><c>true</c>如果此ui不存在返回true，存在返回false<c>false</c> otherwise.</returns>
    /// <param name="uiBase">User interface base.</param>
    public bool ShowUIForm(UIBase uiBase)
    {
        // _Bg.gameObject.SetActive(SceneManager.GetActiveScene().name == "Hall");

        if (uiBase.root != null)
            GameObject.Destroy(uiBase.root.gameObject);

        if (uiBase.CurrentUIType.IsClearPopUp)
        {
            ClearPopUpStack();
        }

        if(uiBase.CurrentUIType.IsClearAllOtherView)
        {
            ClearAllShowForms();
        }

        UIBase existUiBase;
        if (_DicALLUIForms.TryGetValue(uiBase.uiName, out existUiBase))
        {
            if (uiBase.CurrentUIType.UIForms_ShowMode == UIFormsShowMode.PopUp)
                uiBase.uiName = uiBase.uiName + "&&" + uiBase.GetHashCode();
            else if (uiBase.CurrentUIType.UIForms_ShowMode == UIFormsShowMode.Normal)
            {
                //existUiBase = uiBase;
                ExitUIFormsCache(uiBase.uiName);
            }
        }
        //判断不同的窗体显示模式，分别进行处理
        switch (uiBase.CurrentUIType.UIForms_ShowMode)
        {
            case UIFormsShowMode.Normal:
                DestroyAllUINormalRoots();
                ClearFixedUIFrom();
                EnterUIFormsCache(uiBase);
                break;
            case UIFormsShowMode.Mixed:
                EnterUIFormToCacheMix(uiBase);
                break;
            case UIFormsShowMode.HideOther:
                EnterUIFormToCacheHideOther(uiBase);
                break;
            case UIFormsShowMode.PopUp:
                PushPopUpForms(uiBase);
                break;
            case UIFormsShowMode.NoviceGuide:
                ShowNoviceGuideView(uiBase);
                break;
            case UIFormsShowMode.Simple:
                ShowSimpleView(uiBase);
                break;
            default:
                break;
        }

        return true;
    }

    public void ShowSimpleView(UIBase uIBase)
    {
        if (_DicALLUIForms.ContainsKey(uIBase.uiName))
            _DicALLUIForms.Remove(uIBase.uiName);
            
        _DicALLUIForms.Add(uIBase.uiName, uIBase);
        AttachUIRoot(uIBase);
    }

    public void ShowNoviceGuideView(UIBase uiBase)
    {
        UIBase popUI = _StaPopUpForms.Peek();
        if (popUI != null && popUI.IsShowing() && popUI.CurrentUIType.IsNeedHideWhenGuiding)
        {
            popUI.Hide();
        }
        AttachNoviceRoot(uiBase);
    }

    public void AttachNoviceRoot(UIBase uiBase)
    {
        _DicALLUIForms.Add(uiBase.uiName, uiBase);
        AttachUIRoot(uiBase);
    }

    /// <summary>
    /// normalUI加入栈
    /// </summary>
    /// <param name="uIBase"></param>
    public void AddNormalUIToStack(UIBase uiBase)
    {
        if (uiBase.CurrentUIType.UIForms_Type != UIFormsType.Normal)
            return;

        if (_DicALLUIForms.ContainsKey(uiBase.uiName))
        {
            RemoveFromStack(uiBase);
            _DicALLUIForms.Remove(uiBase.uiName);
        }

        _DicALLUIForms.Add(uiBase.uiName, uiBase);
        _StaNormalUIForms.Push(uiBase);
    }

    public void EnterUIFormToCacheMix(UIBase uiBase)
    {
        // BindingUIToCurrentActiveUI(uiBase);
        AttachUIRoot(uiBase);
        uiBase.Show();

        if (_DicALLUIForms.ContainsKey(uiBase.uiName))
            _DicALLUIForms[uiBase.uiName].Close();
        _DicALLUIForms.Add(uiBase.uiName, uiBase);

        // if(uiBase.CurrentUIType.CanPopMessage && !NoviceGuideMgr.isGuiding)
        //     PopMsgView();
    }

    public void AddToFrontNode(Transform trans)
    {
        trans.SetParent(_CanTransformFront, false);
    }

    public void ReDispalyUIForm(string uiName)
    {
        UIBase uiBase;
        if (_DicALLUIForms.TryGetValue(uiName, out uiBase))
        {
            uiBase.Redisplay();
        }
    }

    /// <summary>
    /// 从prefab中创建ui的gameobject对象，黏贴到root节点上
    /// </summary>
    /// <param uiBase="基本ui对象"></param>
    public void AttachUIRoot(UIBase uiBase)
    {
        CloseLastNormalUI(uiBase);
        //ViewUtils.ShowWaiting();
        string strUIFormPath = null;
        //得到UI窗体的路径

        string prefabName = uiBase.uiName.Contains("&&") ? uiBase.uiName.Substring(0, uiBase.uiName.IndexOf("&&")) : uiBase.uiName;
        strUIFormPath = IndexManager.Instance.getPrefabPath(prefabName);
        //加载指定路径的“UI窗体”
        if (!string.IsNullOrEmpty(strUIFormPath))
        {
            uiBase.root = Instantiate(iResourceManager.Load<Transform>(strUIFormPath));
            uiBase.root.gameObject.SetActive(true);
        }

        if (uiBase.root == null)
        {
            Debug.LogError(uiBase.uiName + " root 为空. strUIFormPath: " + strUIFormPath);
            return;
        }

        //添加monobehaviour基本方法代理
        BehaviourDelegate monoBase = uiBase.root.gameObject.AddComponent<BehaviourDelegate>();
        monoBase.uIBase = uiBase;
        monoBase.RegisterOnDestory(uiBase.OnDestroyRoot);

        uiBase.root.tag = "_TagView";

        if (_CanvasTransform != null && uiBase.root != null)
        {

            switch (uiBase.CurrentUIType.UIForms_Type)
            {
                case UIFormsType.Normal:
                    uiBase.root.SetParent(_CanTransformNormal, false);
                    break;
                case UIFormsType.Fixed:
                    uiBase.root.SetParent(_CanTransformFixed, false);
                    break;
                case UIFormsType.PopUp:
                    uiBase.root.SetParent(_CanTransformPopUp, false);
                    RectTransform rect = uiBase.root.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, -200);
                    break;
                case UIFormsType.Front:
                    uiBase.root.SetParent(_CanTransformFront, false);
                    break;
                case UIFormsType.Middle:
                    uiBase.root.SetParent(_CanTransformMiddle, false);
                    break;
                case UIFormsType.NoviceGuide:
                    uiBase.root.SetParent(_CanTransformNovice, false);
                    break;
                default:
                    break;
            }
        }

        ResetUISortingOrder(uiBase);
    }


    //设置界面的渲染顺序，如果内部有sortingGroup控件，设置为统一值
    private void ResetUISortingOrder(UIBase uiBase)
    {
        //获取此界面的canvas组件
        Canvas viewUiCanvas = uiBase.root.GetComponent<Canvas>();
        if (viewUiCanvas == null)
        {
            viewUiCanvas = uiBase.root.gameObject.AddComponent<Canvas>();
            viewUiCanvas.overrideSorting = true;
            uiBase.root.gameObject.AddComponent<GraphicRaycaster>();
        }

        int baseOrder = GetBaseNodeSortingOrder(uiBase.CurrentUIType.UIForms_Type);

        if (uiBase.CurrentUIType.UIForms_Type == UIFormsType.PopUp)
        {
            viewUiCanvas.sortingOrder = baseOrder + 1 + _StaPopUpForms.Count * 5;
            uiBase.root.localPosition = new Vector3(uiCanvas.transform.root.localPosition.x, uiCanvas.transform.root.localPosition.y, -100 + _StaPopUpForms.Count * viewZGap);
        }
        else
        {
            viewUiCanvas.sortingOrder = baseOrder;
        }

        UnityEngine.Rendering.SortingGroup[] sgArray = uiBase.root.GetComponentsInChildren<UnityEngine.Rendering.SortingGroup>(true);

        for(int i = 0; i < sgArray.Length; i++)
            sgArray[i].sortingOrder = viewUiCanvas.sortingOrder;
    }


    public bool CloseOrReturnUIForms(string uiNameStr)
    {
        UIBase uiBase;
        if (!_DicALLUIForms.TryGetValue(uiNameStr, out uiBase))
        {
            Debug.LogWarning("没有找到此UI界面 name:" + uiNameStr);
            return false;
        }

        /* 判断不同的窗体显示模式，分别进行处理 */
        switch (uiBase.CurrentUIType.UIForms_ShowMode)
        {
            case UIFormsShowMode.Normal:
                ExitUIFormsCache(uiNameStr);
                break;
            case UIFormsShowMode.PopUp:
                PopPopUpForms(uiBase);
                break;
            case UIFormsShowMode.Mixed:
                ExitMixUIFormsCache(uiNameStr);
                break;
            case UIFormsShowMode.HideOther:
                ExitUIFormsFromCacheAndShowOther();
                break;
            case UIFormsShowMode.NoviceGuide:
                CloseGuide(uiBase);
                break;
            case UIFormsShowMode.Simple:
                CloseSimple(uiBase);
                break;
            default:
                break;
        }
        return true;
    }

    void CloseSelf(UIBase uiBase)
    {
            
    }

    void CloseGuide(UIBase uiBase)
    {
        _DicALLUIForms.Remove(uiBase.uiName);
        uiBase.Destroy();
    }

    void CloseSimple(UIBase uiBase)
    {
        _DicALLUIForms.Remove(uiBase.uiName);
        uiBase.Destroy();
    }
    void ExitMixUIFormsCache(string uiName)
    {
        UIBase uiBase;
        if (!_DicALLUIForms.TryGetValue(uiName, out uiBase))
        {
            Debug.LogError("没有找到此UI界面 name:" + uiName);
            return;
        }

        _DicALLUIForms.Remove(uiName);
        DestroyUIForm(uiBase);
    }

    void ExitUIFormsCache(string uiName)
    {
        if (_StaNormalUIForms.Count <= 0)
            return;

        UIBase existUiBase = _StaNormalUIForms.Find(v => v.uiName == uiName);
        _StaNormalUIForms.Remove(existUiBase);

        if (_DicALLUIForms.TryGetValue(uiName, out existUiBase))
        {
            _DicALLUIForms.Remove(uiName);
            DestroyUIForm(existUiBase);
            return;
        }
    }

    void ExitUIFormsFromCacheAndShowOther()
    {
        if (_StaNormalUIForms.Count <= 1)
            return;

        UIBase curUI = PopFromNormalUIFroms();
        DestroyUIForm(curUI);

        UIBase lastUI = _StaNormalUIForms.Peek();
        if (lastUI != null)
        {
            if (lastUI.root == null)
                ShowUIForm(lastUI);
            else
                lastUI.Redisplay();
        }
    }

    /// <summary>
    /// UI窗体入栈
    /// 功能1： 判断栈里是否已经有窗体，有则“冻结”
    ///     2： 先判断“UI预设缓存集合”是否有指定的UI窗体,有则处理。
    ///     3： 指定UI窗体入"栈"
    /// </summary>
    /// <param name="uiBase"></param>
    private void PushPopUpForms(UIBase uiBase)
    {
        AttachUIRoot(uiBase);
        _DicALLUIForms.Add(uiBase.uiName, uiBase);
            
        HideShowingPopView();
        _StaPopUpForms.Add(uiBase);
        //显示弹出框需要的mask
        uiBase.Show();

        if (uiBase.CurrentUIType.IsHideNormalView)
        GetCurrentNormalView().Hide();

        if (uiBase.CurrentUIType.NeedBind && GetCurrentNormalView() != null)
            GetCurrentNormalView().BindUIForm(uiBase);

        if (uiBase.CurrentUIType.LinkUIName != "" && GetCurrentNormalView() != null)
            GetCurrentNormalView().LinkUIForm(uiBase);

        if (uiBase.CurrentUIType.PopUpNeedAnime)
        {
            uiBase.root.DOPunchScale(Vector3.one * 0.1f, 0.3f, 8);
        }
    }


    /// <summary>
    /// PopupUI窗体出栈逻辑
    /// </summary>
    private void PopPopUpForms(UIBase uIBase)
    {
        if (_StaPopUpForms.Count < 1)
            return;
        UIBase topUIForms = null;
        /* 出栈逻辑 */
        if (_StaPopUpForms.Contains(uIBase))
        {
            _StaPopUpForms.Remove(uIBase);
            topUIForms = uIBase;
        }
        else
            topUIForms = _StaPopUpForms.Pop();
        _DicALLUIForms.Remove(topUIForms.uiName);
        topUIForms.Destroy();

        if (_StaPopUpForms.Count >= 1)
        {
            RedisplayPopupForms();
        }
        else if (topUIForms.CurrentUIType.IsRefreshCurrentNormalViewWhenIsLastPopClosed)
        {
            //如果是最后一个弹出窗，刷新主界面
            RefreshCurrentView();
        }
        UIBase currentUIBase = GetCurrentNormalView();
        if (currentUIBase != null)
        {
            UIBase topHideNormalViewType = _StaPopUpForms.Find((v) =>
            {
                return v.CurrentUIType.IsHideNormalView;
            });
            bool isLastHideNormalViewType = topHideNormalViewType == null || (topHideNormalViewType.CurrentUIType.LinkUIName != "" && topHideNormalViewType.CurrentUIType.LinkUIName != currentUIBase.uiName);
            if (_StaPopUpForms.Count == 0 || (topUIForms.CurrentUIType.IsHideNormalView && isLastHideNormalViewType))
            {
                currentUIBase.Show();
            }
        }
            
    }

    private bool RedisplayPopupForms()
    {
        for (int i = _StaPopUpForms.Count - 1; i >= 0; i--)
        {
            UIBase uiBase = _StaPopUpForms[i];

            if (!uiBase.CurrentUIType.IsOverAllPop && !IsCurrentViewLeagueView() 
                || (uiBase.CurrentUIType.LinkUIName != "" 
                && uiBase.CurrentUIType.LinkUIName != GetCurrentNormalView().uiName))
                continue;

            uiBase.root.SetAsLastSibling();
            uiBase.Redisplay();
            return true;
        }

        return false;
    }

    private void EnterUIFormsCache(UIBase uiBase)
    {
        if (uiBase.CurrentUIType.IsLobbyView)
            ClearNormalCache();

        RemoveFromStack(uiBase);

        AttachUIRoot(uiBase);
        RemoveFadeOutBlackImage();
        uiBase.Show();
        AddNormalUIToStack(uiBase);

        ManagePopupForms(uiBase);

    }

    public void RemoveFadeOutBlackImage()
    {
        if (fadeOutBlackImage != null)
            GameObject.Destroy(fadeOutBlackImage.gameObject);
    }

    private void ManagePopupForms(UIBase uiBase)
    {

        UIBase topUIForms = _StaPopUpForms.Peek();
        if (topUIForms != null && topUIForms.IsShowing())
            topUIForms.Hide();

    }

    /// <summary>
    /// 加载UI窗体到“当前显示窗体集合”缓存中,且隐藏其他正在显示的页面
    /// </summary>
    /// <param name="strUIFormsName"></param>
    private void EnterUIFormToCacheHideOther(UIBase uiBase)
    {
        AttachUIRoot(uiBase);

        foreach (UIBase BaseUIFormItem in _DicALLUIForms.Values)
        {
            BaseUIFormItem.Hide();
        }

        _DicALLUIForms.Add(uiBase.uiName, uiBase);
        _StaNormalUIForms.Push(uiBase);
    }

    /// <summary>
    /// 根据指定UI窗体名称，加载到“所有UI窗体”缓存中。
    /// </summary>
    /// <param name="strUIFormName">UI窗体名称</param>
    /// <returns></returns>
    private UIBase LoadUIFormsToAllUIFormsCatch(string strUIFormName)
    {
        UIBase baseUI;                             //UI窗体

        //判断“UI预设缓存集合”是否有指定的UI窗体,否则新加载窗体
        _DicALLUIForms.TryGetValue(strUIFormName, out baseUI);
        if (baseUI == null)
        {
            //加载指定路径的“UI窗体”
            //            baseUI = LoadUIForms(strUIFormName);
            _StaNormalUIForms.Push(baseUI);
        }

        return baseUI;
    }

    /// <summary>
    /// 清空弹出窗体的栈，并且摧毁窗体
    /// </summary>
    private void ClearPopUpStack()
    {
        for (int i = 0; i < _StaPopUpForms.Count; i++)
        {
            UIBase uiBase = _StaPopUpForms.Pop();
            uiBase.Destroy();
        }
    }

    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    private void ClearAllShowForms()
    {
        //foreach(var uiName in _DicALLUIForms.Keys){
        //    UIBase uiBase = _DicALLUIForms[uiName];
        //    uiBase.Destroy();
        //}
        List<string> keys = new List<string>(_DicALLUIForms.Keys);
        for (int i = keys.Count - 1; i > -1; i--)
        {
            UIBase uiBase = _DicALLUIForms[keys[i]];
            uiBase.Destroy();
        }
    }

    private void DestroyUIForm(UIBase uiBase)
    {
        _DicALLUIForms.Remove(uiBase.uiName);
        uiBase.Destroy();
    }

    private List<UIBase> GetCurrentShowingUIForms()
    {
        List<UIBase> currentShowingViews = new List<UIBase>();
        foreach (UIBase item in _DicALLUIForms.Values)
        {
            if (item.IsShowing())
                currentShowingViews.Add(item);
        }

        return currentShowingViews;
    }

    //清除别的uiRoot
    private void DestroyAllUINormalRoots()
    {
        foreach (UIBase uibase in _StaNormalUIForms)
        {
            if (uibase.CurrentUIType.IsLobbyView)
            {
                uibase.Hide();
            }
            else
            {
                uibase.Destroy();
            }
        }
    }

    private void ClearFixedUIFrom()
    {
        List<UIBase> uiList = new List<UIBase>();
        foreach (var key in _DicALLUIForms.Keys)
        {
            UIBase uiBase = _DicALLUIForms[key];
            if (uiBase.CurrentUIType.UIForms_Type == UIFormsType.Fixed)
                uiList.Add(uiBase);
        }

        foreach (UIBase item in uiList)
        {
            _DicALLUIForms.Remove(item.uiName);
            item.Destroy();
        }
    }

    public void ClearPopUIByType(Type type)
    {
        List<UIBase> uiList = _StaPopUpForms.FindAll(v => v.GetType().Name == type.Name);

        foreach (UIBase item in uiList)
        {
            _StaPopUpForms.Remove(item);
            _DicALLUIForms.Remove(item.uiName);
            item.Destroy();
        }
    }

    public void ClearPopUI()
    {
        foreach (UIBase item in _StaPopUpForms)
        {
            _StaPopUpForms.Remove(item);
            _DicALLUIForms.Remove(item.uiName);
            item.Destroy();
        }
    }

    public void DestroyAndClearNormalUI()
    {
        foreach (UIBase uiBase in _StaNormalUIForms)
        {
            if (uiBase.CurrentUIType.IsLobbyView)
                continue;

            uiBase.Destroy();
            uiBase.CloseAndClearLinkedUI();
            _DicALLUIForms.Remove(uiBase.uiName);
        }

        for (int i = _StaNormalUIForms.Count - 1; i > 0; i--)
        {
            var item = _StaNormalUIForms[i];
            if (!item.CurrentUIType.IsLobbyView)
            {
                _StaNormalUIForms.Remove(item);
            }
        }
    }

    public void DestroyAllUIForm()
    {
        UIBase lobbyUI = null;

        List<string> keys = new List<string>(_DicALLUIForms.Keys);

        foreach (var key in keys)
        {
            if (!_DicALLUIForms.ContainsKey(key))
                continue;
                
            if (key == LobbyUIName)
            {
                lobbyUI = _DicALLUIForms[key];
                continue;
            }

            _DicALLUIForms[key].Destroy();
        }

        _StaNormalUIForms.Clear();
        _StaPopUpForms.Clear();
        _DicALLUIForms.Clear();
        if (lobbyUI != null)
        {
            _DicALLUIForms.Add(LobbyUIName, lobbyUI);
            _StaNormalUIForms.Push(lobbyUI);
        }
    }

    public void ClearNormalCache()
    {
        foreach (UIBase uiBase in _StaNormalUIForms)
        {
            _DicALLUIForms.Remove(uiBase.uiName);
        }

        _StaNormalUIForms.Clear();
        _DicCurrentShowUIForms.Clear();
        MessageCenter.RemoveAllMsgListener();
    }

    public void ClearAllCache()
    {
        _DicALLUIForms.Clear();
        _StaNormalUIForms.Clear();
        _StaPopUpForms.Clear();
        _DicCurrentShowUIForms.Clear();
        MessageCenter.RemoveAllMsgListener();
    }

    public void ShowLobbyView()
    {
        MainUI.Create();
    }

    public bool IsShowingPopView()
    {
        if (_StaPopUpForms.Count == 0)
            return false;

        UIBase popview = _StaPopUpForms.Peek();
        return (popview != null && popview.root != null);
    }

    public bool IsShowingPopView(string name)
    {
        if (_StaPopUpForms.Count == 0)
            return false;

        foreach (var item in _StaPopUpForms)
        {
            if (item.uiName.Contains(name))
                return true;
        }

        return false;
    }

    public bool TryGetAView<T>(out T view) where T : UIBase
    {
        if (_DicALLUIForms.Count == 0)
        {
            view = null;
            return false;
        }

        foreach (var item in _DicALLUIForms.Values)
        {
            if (item is T)
            {
                view = (T)item;
                return true;
            }
        }

        view = null;
        return false;
    }

    public bool HasShowingPopupView()
    {
        UIBase uiBase = _StaPopUpForms.Find(v => v.root != null && v.root.gameObject.activeSelf);
        return uiBase != null;
    }

    public bool IsInLeagueView()
    {
        if (_StaNormalUIForms.Count == 0)
            return false;

        UIBase normalView = _StaNormalUIForms.Peek();
        return (normalView != null && normalView.root != null && normalView.uiName == LobbyUIName);
    }

    private UIBase PopFromNormalUIFroms()
    {
        if (_StaNormalUIForms.Count < 1)
            return null;

        UIBase uibase = _StaNormalUIForms.Pop();

        return uibase;
    }

    public void BackAView()
    {
        if (_StaNormalUIForms.Count <= 1)
        {
            UIBase uibase = PopFromNormalUIFroms();
            uibase.Destroy();
            ShowLobbyView();
            return;
        }

        lastNormalUiBase = PopFromNormalUIFroms();

        UIBase nowBase = _StaNormalUIForms.Peek();

        nowBase.Redisplay();
        
        if (nowBase.CurrentUIType.IsLobbyView)
        {
            CloseLastNormalUI(nowBase);
            RemoveFadeOutBlackImage();
        }
    }

    public void BackAViewToTop() {
        if (_StaNormalUIForms.Count <= 1) {            
            return;
        }
        UIBase nowBase = _StaNormalUIForms.Peek();
        nowBase.BackAView();
        BackAViewToTop();
    }

    public void ClearNormalStackExceptLobby()
    {
        // var lobbyUi = _StaNormalUIForms.Find(v => v.CurrentUIType.IsLobbyView);
        GetCurrentNormalView().Close();
        for(int i = _StaNormalUIForms.Count - 1; i > 0; i--)
        {
            var item = _StaNormalUIForms[i];
            if (!item.CurrentUIType.IsLobbyView)
            {
                _StaNormalUIForms.Remove(item);
            }
        }
    }

    public void CloseLastNormalUI(UIBase curUiBase)
    {
        if (curUiBase.CurrentUIType.UIForms_ShowMode != UIFormsShowMode.Normal)
            return;

        if (lastNormalUiBase == null)
            return;

        if (_DicALLUIForms.ContainsKey(lastNormalUiBase.uiName))
            _DicALLUIForms.Remove(lastNormalUiBase.uiName);

        lastNormalUiBase.Destroy();
        lastNormalUiBase = null;
    }

    public void CloseBindedUiForms(UIBase uiBase)
    {
        // 关闭掉所有在此界面上的界面
        foreach (UIBase popUi in uiBase.BindedPopUpUIForm)
        {
            popUi.Destroy();
            _StaPopUpForms.Remove(popUi);
            _DicALLUIForms.Remove(popUi.uiName);
        }

        uiBase.BindedPopUpUIForm.Clear();
    }

    private void RemoveFromStack(UIBase uiBase)
    {
        if (_StaNormalUIForms.Count > 0 && _StaNormalUIForms.Peek().uiName == uiBase.uiName)
        {
            PopFromNormalUIFroms();
        }
    }


    public WaitingPanel ShowWaitingPanel(string content, bool showLogoBg = false, bool needDelayShow = true) {        
        _waitingPanel.Show(content, showLogoBg, needDelayShow);
        return _waitingPanel;
    }

    public void HideShowingPopView()
    {
        if (_StaPopUpForms.Count > 0 && _StaPopUpForms.Peek().IsShowing())
        {
            if (!_StaPopUpForms.Peek().CurrentUIType.NoHidePopUp)
                _StaPopUpForms.Peek().Hide();
        }
    }

    public void HideWaitingPanel(bool trueHide = false) {
        if (_waitingPanel == null)
            return;

        if (trueHide) {
            _waitingPanel.TrueHide();
            return;
        }
        _waitingPanel.Hide();
    }

    public bool IsCurrentViewLeagueView()
    {
        UIBase uiBase = GetCurrentNormalView();
        if (uiBase == null)
            return false;

        return uiBase.CurrentUIType.IsLobbyView;
    }

    public void SetTransToNormalNode(Transform trans)
    {
        trans.SetParent(_CanTransformNormal, false);
    }

    //无条件弹出窗口
    public void ForcePopupView(UIBase uiBase)
    {
        uiBase.CurrentUIType.IsForcedPop = true;
        uiBase.root.SetAsLastSibling();
        _StaPopUpForms.Remove(uiBase);
        _StaPopUpForms.Add(uiBase);
        uiBase.Redisplay();
    }

    //刷新当前的界面
    public void RefreshCurrentView()
    {
        if (_StaNormalUIForms.Count < 1)
            return;

        UIBase uiBase = _StaNormalUIForms.Peek();
        uiBase.Redisplay();
    }

    public UIBase GetCurrentNormalView()
    {
        if (_StaNormalUIForms.Count < 1)
            return null;
        return _StaNormalUIForms.Peek();
    }

    public UIBase GetLobbyView()
    {
        return _DicALLUIForms[LobbyUIName];
    }

    private const float CloseAnimaDurration = 0.15f;
    private Sequence closeDotween;
    public void FadOutClose(Action closeAction)
    {
        if (closeDotween != null && closeDotween.IsPlaying())
            return;

        fadeOutBlackImage = ViewUtils.CreatePrefab("BackImage").transform;
        UIManager.GetInstance().AddToFrontNode(fadeOutBlackImage);
        closeDotween = DOTween.Sequence();
        closeDotween.Append(fadeOutBlackImage.GetComponent<Image>().DOColor(new Vector4(0, 0, 0, 0.7f), CloseAnimaDurration))
            .AppendCallback(delegate () {
                closeAction();
            });

    }

    //需要mask 3d object的情况 设置启用ui3dCamera
    //public void SetUI3dCamera(RectTransform renderContent)
    //{
    //    Vector2 RTPanelSize = ViewUtils.GetUISizeInWorld(renderContent);

    //    RenderTexture rt = new RenderTexture((int)RTPanelSize.x, (int)RTPanelSize.y, 2400);
    //    ui3DCamera.targetTexture = rt;

    //    RawImage image = renderContent.GetComponent<RawImage>();
    //    image.texture = rt;
    //    ui3DCamera.transform.position = new Vector3(renderContent.position.x, renderContent.position.y, ui3DCamera.transform.position.z);

    //    float sizee = UIManager.GetInstance().uiCamera.orthographicSize * RTPanelSize.y / (float)Screen.height;
    //    ui3DCamera.orthographicSize = sizee;
    //}

    public void SetHallBgActive(bool value)
    {
        if (hallBg != null)
            hallBg.SetActive(value);
    }

    public void Destroy()
    {
        GameObject.Destroy(uiCamera.gameObject);
        GameObject.Destroy(_CanvasTransform.gameObject);
        GameObject.Destroy(this.gameObject);
    } 


}
