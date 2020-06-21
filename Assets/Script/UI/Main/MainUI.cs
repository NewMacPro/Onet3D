using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : UIBase
{
    private Text goldText;
    public static void Create()
    {
        MainUI ui = new MainUI();
        ui.Init();
    }

    void Init()
    {
        Redisplay();
    }


    public override void Redisplay()
    {
        CreateAndAttachRoot("MainUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        root.FindAChild<Button>("SetBtn");
        ViewUtils.AddButtonClick(root, "SetBtn", OnClickSetting);
        ViewUtils.AddButtonClick(root, "ClearBtn", OnClickClear);
        ViewUtils.AddButtonClick(root, "PlayBtn", OnClickPlay);
        ViewUtils.AddButtonClick(root, "GalleryBtn", OnClickGallery);
        ViewUtils.AddButtonClick(root, "NoadBtn", OnClickNoAdBtn);
        ViewUtils.AddButtonClick(root, "HelpBtn", OnClickHelp);
        goldText = root.FindAChild<Text>("Gold/Text");
    }

    void Refresh()
    {
        goldText.text = SaveModel.player.gold.ToString();
    }

    void OnClickSetting() {
        SettingUI.Create();
    }

    void OnClickPlay() {
        GameUI.Create();
    }
    void OnClickGallery()
    {
        GalleryUI.Create();
    }
    void OnClickNoAdBtn()
    {
        FBstatistics.LogEvent("clickremovead");
    }
    void OnClickHelp()
    {
        FBstatistics.LogEvent("clickhelp");
        IntroductionUI.Create();
    }
    void OnClickClear()
    {
        SaveModel.CreateSave();
    }
}
