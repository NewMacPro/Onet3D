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
        ViewUtils.AddButtonClick(root, "CloseBtn" , BackAView);
    }

    void Refresh()
    {

    }
}