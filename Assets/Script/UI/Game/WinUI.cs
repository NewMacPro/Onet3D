﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIBase
{
    private int starValue;
    private int timeValue;
    private int starAddGoldValue = 0;
    private int timeAddGoldValue = 0;
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
        SaveModel.AddGold(starAddGoldValue + timeAddGoldValue);
        ViewUtils.AddButtonClick(root, "ReStartBtn", OnClickReStartBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickAdBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickEvaluateBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickNoAdBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickShareBtn);

        ViewUtils.SetText(root, "TitleText", "YOU WIN！");
        ViewUtils.SetText(root, "CheckPointText", "level" + SaveModel.player.level);

        ViewUtils.SetText(root, "StarBg/StarValue", starValue.ToString());
        ViewUtils.SetText(root, "StarBg/GoldValue", "+" + starAddGoldValue);

        ViewUtils.SetText(root, "TimeBg/TimeValue", timeValue + "s");
        ViewUtils.SetText(root, "TimeBg/GoldValue", "+" + timeAddGoldValue);
        ViewUtils.SetText(root, "AdBtn/Text", "" + starAddGoldValue + timeAddGoldValue);
        ViewUtils.SetText(root, "ReStartBtn/Text", "CONTINUE");

        //统计
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "level" + SaveModel.player.level;
        param["ispassed"] = "true";
        param["getstars"] = starValue.ToString();
        param["lefttime"] = timeValue.ToString();
        param["getcoins"] = "" + starAddGoldValue + timeAddGoldValue;

        ShowLuckyRewards();
        FBstatistics.LogEvent("gamelevel", param);
        
    }

    void OnUpdate() {
        timer += Time.deltaTime;
    }

    void Refresh()
    {

    }

    void OnClickReStartBtn() {
        Close();
        GameUI.Create();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param["action"] = "close";
        param["time"] = "" + (int)timer;
        FBstatistics.LogEvent("gameend" , param);
    }

    void OnClickAdBtn() {
        SaveModel.AddGold(starAddGoldValue + timeAddGoldValue);
        ViewUtils.SetActive(root, "AdBtn", false);

        Dictionary<string, object> param = new Dictionary<string, object>();
        param["action"] = "whatad";
        param["time"] = "" + (int)timer;
        FBstatistics.LogEvent("gameend" , param);
    }

    void OnClickEvaluateBtn() { 
    
    }

    void OnClickNoAdBtn() {
        FBstatistics.LogEvent("clickremovead");
    }

    void OnClickShareBtn() { 
    
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