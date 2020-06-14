﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : UIBase
{
    private int addGoldValue = 120;
    public static void Create()
    {
        LoseUI ui = new LoseUI();
        ui.Init();
    }

    void Init()
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("LoseUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        ViewUtils.AddButtonClick(root, "ReStartBtn", OnClickReStartBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickAdBtn);

        
        ViewUtils.SetText(root, "AdBtn/Text", addGoldValue.ToString());
        ViewUtils.SetText(root, "ReStartBtn/Text", "RESTART");
        ViewUtils.SetText(root, "TitleText", "YOU LOSE!");
        ViewUtils.SetText(root, "CheckPointText", "level" + SaveModel.player.level);
        ViewUtils.SetText(root, "TimeOutText", "Time is up!");
    }

    void Refresh()
    {

    }

    void OnClickReStartBtn() 
    {
        Close();
        GameUI.Create();
    }

    void OnClickAdBtn() 
    {
        SaveModel.AddGold(addGoldValue);
        ViewUtils.SetActive(root, "AdBtn", false);
    }
}