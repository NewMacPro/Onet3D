using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIBase
{
    private int starValue;
    private int timeValue;
    private int addGoldValue = 0;
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
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("WinUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        ViewUtils.AddButtonClick(root, "ReStartBtn", OnClickReStartBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickAdBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickEvaluateBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickNoAdBtn);
        ViewUtils.AddButtonClick(root, "AdBtn", OnClickShareBtn);

        ViewUtils.SetText(root, "TitleText", "你赢了！");
        ViewUtils.SetText(root, "CheckPointText", "关卡" + SaveModel.player.level);
        ViewUtils.SetText(root, "LevelText", "Lv ?");

        ViewUtils.SetText(root, "StarBg/StarValue", starValue.ToString());
        ViewUtils.SetText(root, "StarBg/AddExpValue", "+???EXP");
        ViewUtils.SetText(root, "StarBg/GoldValue", "+??");

        ViewUtils.SetText(root, "TimeBg/TimeValue", timeValue + "s");
        ViewUtils.SetText(root, "TimeBg/AddExpValue", "+???EXP");
        ViewUtils.SetText(root, "TimeBg/GoldValue", "+??");
        ViewUtils.SetText(root, "AdBtn/Text", "???");
        ViewUtils.SetText(root, "ReStartBtn/Text", "下一关");
        
    }

    void Refresh()
    {

    }

    void OnClickReStartBtn() {
        Close();
        GameUI.Create();
    }

    void OnClickAdBtn() {
        SaveModel.AddGold(addGoldValue);
        ViewUtils.SetActive(root, "AdBtn", false);
    }

    void OnClickEvaluateBtn() { 
    
    }

    void OnClickNoAdBtn() { 
    
    }

    void OnClickShareBtn() { 
    
    }
}