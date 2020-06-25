using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HintUI : UIBase
{
    public static HintUI Create(string content)
    {
        MessageCenter.SendMessage(MyMessageType.HINT_UI, MyMessage.CLOSE);
        HintUI hintUI = new HintUI();
        hintUI.Init(content);
        return hintUI;
    }

    private string content;
    private Coroutine coroutine;

    public void Init(string content)
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.Front;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.Simple;
        this.CreateAndAttachRoot("HintUI");
        this.content = content;
        Attach();

    }
    private void Attach()
    {
        ViewUtils.SetText(this.root, "Content", this.content);
        MessageCenter.AddMsgListener(MyMessageType.HINT_UI, this.OnMessage);
        coroutine = CoroutineHelper.Instance.WaitForSeconds(2f, () =>
        {
            Close();
        });
    }
    private void OnMessage(KeyValuesUpdate kv)
    {
        if (kv.Key == MyMessage.CLOSE)
        {
            Close();
        }
    }

    public override void OnDestroyRoot()
    {
        MessageCenter.RemoveMsgListener(MyMessageType.HINT_UI, this.OnMessage);
        CoroutineHelper.Instance.StopCoroutine(coroutine);
    }


}
