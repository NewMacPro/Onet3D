using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIBase
{
    private int starValue;
    private int timeValue;
    private int starAddGoldValue = 0;
    private int timeAddGoldValue = 0;
    private int totalAddGoldValue = 0;
    private float timer = 0;
    public static void Create(int star , int time)
    {
        WinUI ui = new WinUI();
        ui.Init(star , time);
    }

    void Init(int star , int time)
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        starValue = star;
        timeValue = time;
        timer = 0;
        CreateAndAttachRoot("WinUI");
        Attach();
        Refresh();
    }

    void Attach()
    {

        //3颗星星转换为1金币，6S转换为1金币
        starAddGoldValue = Mathf.FloorToInt(starValue / 3);
        timeAddGoldValue = Mathf.FloorToInt(timeValue / 6);
        totalAddGoldValue = starAddGoldValue + timeAddGoldValue;

        SaveModel.AddGold(totalAddGoldValue);
        ViewUtils.AddButtonClick(root, "ReStartBtn", OnClickReStartBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickAdBtn);
        ViewUtils.AddButtonClick(root, "EvaluateBtn", OnClickEvaluateBtn);
        ViewUtils.AddButtonClick(root, "NoAdBtn", OnClickNoAdBtn);
        ViewUtils.AddButtonClick(root, "ShareBtn", OnClickShareBtn);

        ViewUtils.SetText(root, "TitleText", "YOU WIN！");
        ViewUtils.SetText(root, "CheckPointText", "level" + SaveModel.player.level);

        ViewUtils.SetText(root, "StarBg/StarValue", starValue.ToString());
        ViewUtils.SetText(root, "StarBg/GoldValue", "+" + starAddGoldValue);

        ViewUtils.SetText(root, "TimeBg/TimeValue", timeValue + "s");
        ViewUtils.SetText(root, "TimeBg/GoldValue", "+" + timeAddGoldValue);
        ViewUtils.SetText(root, "AdBtn/Text", "" + totalAddGoldValue);
        ViewUtils.SetText(root, "ReStartBtn/Text", "CONTINUE");

        //统计
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "level" + SaveModel.player.level;
        param["ispassed"] = "true";
        param["getstars"] = starValue.ToString();
        param["lefttime"] = timeValue.ToString();
        param["getcoins"] = totalAddGoldValue.ToString();

        ShowLuckyRewards();
        FBstatistics.LogEvent("gamelevel", param);
        ShowEvaluate();
        
    }

    void OnUpdate() {
        timer += Time.deltaTime;
    }

    void Refresh()
    {

    }

    void ShowEvaluate()
    {
        if (SaveModel.player.level == Const.EVALUETE_LEVEL)
        {
            SDKInterface.Instance.Evaluate();
        }
    }

    void OnClickReStartBtn() {
        if (SaveModel.player.level >= 2)
        {
            IronsoucrManager.Instance.ShowInterstitial();
        }
        Close();
        GameUI.Create();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param["action"] = "close";
        param["time"] = "" + (int)timer;
        FBstatistics.LogEvent("gameend" , param);
    }

    void OnClickAdBtn() {
        IronsoucrManager.Instance.ShowRewardedVideo(() =>
        {
            SaveModel.AddGold(totalAddGoldValue);
            MessageCenter.SendMessage(MyMessageType.GAME_UI, MyMessage.REFRESH_RES);
            ViewUtils.SetActive(root, "AdBtn", false);
        });

        Dictionary<string, object> param = new Dictionary<string, object>();
        param["action"] = "whatad";
        param["time"] = "" + (int)timer;
        FBstatistics.LogEvent("gameend" , param);
    }

    void OnClickEvaluateBtn()
    {
        SDKInterface.Instance.Evaluate();
    }

    void OnClickNoAdBtn() {
        FBstatistics.LogEvent("clickremovead");
        SaveModel.RemoveAD();
        IronsoucrManager.Instance.DestroyBanner();
    }

    void OnClickShareBtn() {
        SDKInterface.Instance.Share();
    }

    void ShowLuckyRewards()
    {
        int luckyRewardsGameNum = Config.Instance.commonNode.GetInt("luckyRewardsGameNum");
        if (GameManager.Instance.gameNum >= luckyRewardsGameNum)
        {
            GameManager.Instance.gameNum = 0;
            SurpriseRewardUI.Create();
        }
    }
}