using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIBase
{
    public static void Create()
    {
        PauseUI ui = new PauseUI();
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
        CreateAndAttachRoot("PauseUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        ViewUtils.AddButtonClick(root, "ContinueBtn", OnClickContinueBtn);
        ViewUtils.AddButtonClick(root, "HomeBtn", OnClickHomeBtn);
        ViewUtils.AddButtonClick(root, "ReStarBtn", OnClickReStarBtn);
        ViewUtils.AddButtonClick(root, "ContinueBtn", Close);
        ViewUtils.AddButtonClick(root, "ContinueBtn", Close);
    }

    void Refresh()
    {

    }

    void OnClickContinueBtn() 
    {
        Close();
    }

    void OnClickHomeBtn()
    {
        Close();
        UIManager.GetInstance().ShowLobbyView();
    }

    void OnClickReStarBtn()
    {
        Close();
        GameUI.Create();
    }
}