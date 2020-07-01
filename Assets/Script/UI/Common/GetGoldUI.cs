using DG.Tweening;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoldUI : UIBase
{
    public static GetGoldUI Create(int gold)
    {
        GetGoldUI getGoldUI = new GetGoldUI();
        getGoldUI.Init(gold);
        return getGoldUI;
    }

    private int gold;
    public void Init(int gold)
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        this.CreateAndAttachRoot("GetGoldUI");
        this.gold = gold;
        Attach();
    }

    public void Attach()
    {
        root.FindAChild("Bg").DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        ViewUtils.SetText(root, "Value", "x"+this.gold);
        ViewUtils.AddButtonClick(this.root, "", this.Close);
    }
}
