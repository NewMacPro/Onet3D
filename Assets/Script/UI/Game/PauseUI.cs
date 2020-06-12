﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseUI : UIBase
{
    private UnityAction<string> callBack;
    public static void Create(UnityAction<string> cb)
    {
        PauseUI ui = new PauseUI();
        ui.Init(cb);
        
    }

    void Init(UnityAction<string> cb)
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        callBack = cb;
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("PauseUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        ViewUtils.AddButtonClick(root, "ContinueBtn", OnClickContinueBtn);
        ViewUtils.AddButtonClick(root, "HomeBtn", OnClickHomeBtn);
        ViewUtils.AddButtonClick(root, "ReStarBtn", OnClickReStartBtn);
        ViewUtils.AddButtonClick(root, "Sound/SwitchBtn", OnClickSoundBtn);
        ViewUtils.AddButtonClick(root, "Music/SwitchBtn", OnClickMusicBtn);
    }

    void Refresh()
    {

    }

    void OnClickContinueBtn() 
    {
        Close();
        callBack(GameModel.BACK_GAME_CONTIUE);
    }

    void OnClickHomeBtn()
    {
        Close();
        callBack(GameModel.BACK_GAME_CLOSE);
    }

    void OnClickReStartBtn()
    {
        Close();
        callBack(GameModel.BACK_GAME_RESTART);
    }

    void OnClickSoundBtn() { 
        
    }

    void OnClickMusicBtn()
    {

    }
}