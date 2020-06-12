using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebornUI : UIBase
{
    public static void Create()
    {
        RebornUI ui = new RebornUI();
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
        CreateAndAttachRoot("RebornUI");
        Attach();
        Refresh();
    }

    void Attach()
    {

    }

    void Refresh()
    {

    }
}