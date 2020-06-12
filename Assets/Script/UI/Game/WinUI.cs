using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIBase
{
    private int starValue;
    private int timeValue;
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

        ViewUtils.SetText(root, "TitleText", "你赢了！");
        ViewUtils.SetText(root, "CheckPointText", "关卡" + SaveModel.player.level);
        ViewUtils.SetText(root, "LevelText", "Lv 6");

        ViewUtils.SetText(root, "StarBg/StarValue", starValue.ToString());
        ViewUtils.SetText(root, "StarBg/AddExpValue", "+32EXP");
        ViewUtils.SetText(root, "StarBg/GoldValue", "+77");

        ViewUtils.SetText(root, "TimeBg/TimeValue", starValue + "s");
        ViewUtils.SetText(root, "TimeBg/AddExpValue", "+32EXP");
        ViewUtils.SetText(root, "TimeBg/GoldValue", "+77");
        ViewUtils.SetText(root, "LevelText", "Lv 6");
    }

    void Refresh()
    {

    }

    void OnClickReStartBtn() { 
    
    }

    void OnClickAdBtn() { 
    
    }
}