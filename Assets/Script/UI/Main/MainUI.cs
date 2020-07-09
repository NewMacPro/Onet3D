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
        ViewUtils.SetText(root, "GalleryBtn/Text", "GALLERY");
        ViewUtils.SetText(root, "NoadBtn/Text", "No Ads");
        ViewUtils.SetText(root, "HelpBtn/Text", "HELP");
        ViewUtils.SetActive(root, "ClearBtn", false);
#if UNITY_EDITOR
        ViewUtils.SetActive(root, "ClearBtn", true);
#endif
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
        if (SaveModel.GetPlayer().guide)
        {
            GameUI.Create();
        }
        else
        {
            NoviceGuideUI.Create();
        }
    }
    void OnClickGallery()
    {
        GalleryUI.Create();
        FBstatistics.LogEvent("clickchange");
        
    }
    void OnClickNoAdBtn()
    {
        FBstatistics.LogEvent("clickremovead");
        SaveModel.RemoveAD();
        IronsoucrManager.Instance.DestroyBanner();
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
