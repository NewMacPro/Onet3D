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
        ViewUtils.AddButtonClick(root, "CloseBtn", BackAView);
        ViewUtils.AddButtonClick(root, "Sound/SwitchBtn", OnClickSoundBtn);
        ViewUtils.AddButtonClick(root, "Music/SwitchBtn", OnClickMusicBtn);
        ViewUtils.AddButtonClick(root, "Notify/SwitchBtn", OnClickNotifyBtn);
        ViewUtils.AddButtonClick(root, "SupportBtn/Text", OnClickSupportBtn);
        ViewUtils.AddButtonClick(root, "PrivacyBtn/Text", OnClickPrivacyBtn);
        ViewUtils.AddButtonClick(root, "TermsBtn/Text", OnClickTermsBtn);
        ViewUtils.AddButtonClick(root, "DataBtn/Text", OnClickDataBtn);
        ViewUtils.SetText(root, "TitleText", "SETTINGS");
        ViewUtils.SetText(root, "Sound/Text", "Sound");
        ViewUtils.SetText(root, "Music/Text", "Music");
        ViewUtils.SetText(root, "Notify/Text", "Notify");
        ViewUtils.SetText(root, "SupportBtn/Text", "SUPPORT");
        ViewUtils.SetText(root, "PrivacyBtn/Text", "PRIVAC POLICY");
        ViewUtils.SetText(root, "TermsBtn/Text", "TERMS OF SERVICE");
        ViewUtils.SetText(root, "DataBtn/Text", "PERSONAL DATA \nREQUEST");
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
        param["Whether to open"] = SaveModel.SoundSwith?"off" : "On";
        FBstatistics.LogEvent("Click switch", param);

        SaveModel.SoundSwith = !SaveModel.SoundSwith;
        RefreshSoundSwitch();
    }
    void OnClickMusicBtn()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "music";
        param["Whether to open"] = SaveModel.MusicSwith ? "off" : "On";
        FBstatistics.LogEvent("Click switch", param);

        SaveModel.MusicSwith = !SaveModel.MusicSwith;
        RefreshMusicSwitch();
    }
    void OnClickNotifyBtn()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "notifs";
        param["Whether to open"] = SaveModel.NotifySwitch ? "off" : "On";
        FBstatistics.LogEvent("Click switch", param);

        SaveModel.NotifySwitch = !SaveModel.NotifySwitch;
        RefreshNotifySwitch();
    }
    void OnClickSupportBtn()
    {

    }
    void OnClickPrivacyBtn()
    {

    }
    void OnClickTermsBtn()
    {

    }
    void OnClickDataBtn()
    {

    }

}