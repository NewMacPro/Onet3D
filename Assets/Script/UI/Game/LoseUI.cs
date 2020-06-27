using System.Collections;
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
        ViewUtils.AddButtonClick(root, "EvaluateBtn", OnClickEvaluateBtn);
        ViewUtils.AddButtonClick(root, "NoAdBtn", OnClickNoAdBtn);
        ViewUtils.AddButtonClick(root, "ShareBtn", OnClickShareBtn);

        
        ViewUtils.SetText(root, "AdBtn/Text", addGoldValue.ToString());
        ViewUtils.SetText(root, "ReStartBtn/Text", "RESTART");
        ViewUtils.SetText(root, "TitleText", "YOU LOSE!");
        ViewUtils.SetText(root, "CheckPointText", "level" + SaveModel.player.level);
        ViewUtils.SetText(root, "TimeOutText", "Time is up!");

        Dictionary<string,object> param = new Dictionary<string,object>();
        param["name"] = "level" + SaveModel.player.level;
        param["ispassed"] = "false";
        param["getstars"] = "";
        param["lefttime"] = "";
        param["getcoins"] = "";

        FBstatistics.LogEvent("gamelevel", param);
    }

    void Refresh()
    {

    }

    void OnClickReStartBtn() 
    {
        Close();
        GameUI.Create();
        FBstatistics.LogEvent("clickreplay");
    }

    void OnClickAdBtn() 
    {
        IronsoucrManager.Instance.ShowRewardedVideo(() =>
        {
            SaveModel.AddGold(addGoldValue);
            MessageCenter.SendMessage(MyMessageType.GAME_UI, MyMessage.REFRESH_RES);
            ViewUtils.SetActive(root, "AdBtn", false);
        });
        FBstatistics.LogEvent("watchad");
    }

    void OnClickEvaluateBtn()
    {

    }

    void OnClickNoAdBtn()
    {
        FBstatistics.LogEvent("clickremovead");
        SaveModel.RemoveAD();
        IronsoucrManager.Instance.DestroyBanner();
    }

    void OnClickShareBtn()
    {

    }
}