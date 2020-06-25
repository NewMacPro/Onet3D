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
        ViewUtils.SetText(root, "Value", "x"+this.gold);
        ViewUtils.AddButtonClick(this.root, "", this.Close);
    }
}
