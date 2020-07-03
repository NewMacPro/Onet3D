using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    public static void Create()
    {
        SettingUI ui = new SettingUI();
        ui.Init();
    }

    void Init()
    {
        CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("SettingUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        ViewUtils.AddButtonClick(root, "CloseBtn", Close);
        ViewUtils.AddButtonClick(root, "Sound/SwitchBtn", OnClickSoundBtn);
        ViewUtils.AddButtonClick(root, "Music/SwitchBtn", OnClickMusicBtn);
        ViewUtils.AddButtonClick(root, "Notify/SwitchBtn", OnClickNotifyBtn);
        ViewUtils.AddButtonClick(root, "FeedbackBtn", OnClickFeedbackBtn);
        ViewUtils.AddButtonClick(root, "Score", OnClickScoreBtn);
        ViewUtils.AddButtonClick(root, "Share", OnClickShareBtn);
        ViewUtils.SetText(root, "TitleText", "SETTINGS");
        ViewUtils.SetText(root, "Sound/Text", "Sound");
        ViewUtils.SetText(root, "Music/Text", "Music");
        ViewUtils.SetText(root, "Notify/Text", "Notify");
        ViewUtils.SetText(root, "FeedbackBtn/Text", "FEEDBACK");
        ViewUtils.SetText(root, "ScoreBtn/Text", "SCORE");
        ViewUtils.SetText(root, "ShareBtn/Text", "SHARE");
    }

    void Refresh()
    {
        RefreshMusicSwitch();
        RefreshNotifySwitch();
        RefreshSoundSwitch();
    }

    void RefreshMusicSwitch() {
        ViewUtils.SetActive(root, "Music/SwitchBtn/OnImage", SaveModel.MusicSwith);
        ViewUtils.SetActive(root, "Music/SwitchBtn/OffImage", !SaveModel.MusicSwith);
    }
    void RefreshSoundSwitch()
    {
        ViewUtils.SetActive(root, "Sound/SwitchBtn/OnImage", SaveModel.SoundSwith);
        ViewUtils.SetActive(root, "Sound/SwitchBtn/OffImage", !SaveModel.SoundSwith);
    }
    void RefreshNotifySwitch()
    {
        ViewUtils.SetActive(root, "Notify/SwitchBtn/OnImage", SaveModel.NotifySwitch);
        ViewUtils.SetActive(root, "Notify/SwitchBtn/OffImage", !SaveModel.NotifySwitch);
    }
    void OnClickSoundBtn()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "sound";
        param["action"] = SaveModel.SoundSwith?"off" : "on";
        FBstatistics.LogEvent("clickswitch", param);

        SaveModel.SoundSwith = !SaveModel.SoundSwith;
        RefreshSoundSwitch();
    }
    void OnClickMusicBtn()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "music";
        param["action"] = SaveModel.MusicSwith ? "off" : "on";
        FBstatistics.LogEvent("clickswitch", param);

        SaveModel.MusicSwith = !SaveModel.MusicSwith;
        AudioManager.Instance.SetMusicOn(SaveModel.MusicSwith);
        RefreshMusicSwitch();
    }
    void OnClickNotifyBtn()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "notifs";
        param["action"] = SaveModel.NotifySwitch ? "off" : "on";
        FBstatistics.LogEvent("clickswitch", param);

        SaveModel.NotifySwitch = !SaveModel.NotifySwitch;
        RefreshNotifySwitch();
    }
    void OnClickFeedbackBtn()
    {

    }
    void OnClickScoreBtn()
    {
        SDKInterface.Instance.Evaluate();
    }

    void OnClickShareBtn()
    {
        SDKInterface.Instance.Share();
    }

}