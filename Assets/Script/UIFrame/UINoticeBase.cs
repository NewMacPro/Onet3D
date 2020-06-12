public class UINoticeBase: UIBase
{
    public string uiPopType;    // 弹出的窗口父类型
    public bool needPopNext = true;
    override public void Destroy()
    {
        base.Destroy();
        // PopUpManager.Instance.DoPopAction(uiPopType);
        if (needPopNext)
            PopUpManager.Instance.DoPopAction(uiPopType);
    }
}
