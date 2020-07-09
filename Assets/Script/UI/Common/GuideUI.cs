using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuideUI : UIBase
{
    public static GuideUI Create(GameObject oriObj, UnityAction callback, string tip)
    {
        GuideUI ui = new GuideUI();
        ui.Init(oriObj, callback, tip);
        return ui;
    }

    private GameObject oriObj;
    private UnityAction callback;
    private string tip;

    public void Init(GameObject oriObj, UnityAction callback, string tip)
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.NoviceGuide;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.NoviceGuide;
        this.CreateAndAttachRoot("GuideUI");
        this.oriObj = oriObj;
        this.callback = callback;
        this.tip = tip;
        Attach();

    }

    private void Attach()
    {
        ViewUtils.SetText(root, "GuideTipLabel", tip);
        GameObject objClone = GameObject.Instantiate(oriObj);
        objClone.transform.SetParent(root);
        objClone.transform.SetSiblingIndex(1);
        objClone.transform.position = oriObj.transform.position;
        objClone.transform.localScale = oriObj.transform.localScale;
        oriObj.SetActive(false);

        Transform tipNode = root.FindAChild("GuideTip");
        Rect rootRect = root.GetComponent<RectTransform>().rect;
        Rect tipRect = tipNode.GetComponent<RectTransform>().rect;
        Vector3 objPos = objClone.transform.localPosition;
        objPos.y += 200;
        Rect rect = root.GetComponent<RectTransform>().rect;
        float left = -rootRect.width / 2 + tipRect.width / 2;
        float right = -left;
        float top = rootRect.height / 2 - tipRect.height / 2;
        if (objPos.x < left)
        {
            objPos.x = left;
        }
        if (objPos.x > right)
        {
            objPos.x = right;
        }
        if (objPos.y > top)
        {
            objPos.y = top;
        }
        tipNode.localPosition = objPos;

        root.FindAChild<Button>("Button").onClick.AddListener(Close);
    }

    public override void OnDestroyRoot()
    {
        oriObj.SetActive(true);
        if (callback != null)
        {
            callback();
        }
    }


}
