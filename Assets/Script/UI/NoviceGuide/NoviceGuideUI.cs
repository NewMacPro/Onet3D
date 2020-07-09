using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoviceGuideUI : UIBase
{
    public static void Create()
    {
        NoviceGuideUI ui = new NoviceGuideUI();
        ui.Init();

    }

    private int state = 0;
    private Animator ani;
    void Init()
    {
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("NoviceGuide");
        Attach();
        Refresh();
    }

    void Attach()
    {
        ani = root.GetComponent<Animator>();
        ViewUtils.SetText(root, "Title", "HOW TO PLAY");
        ViewUtils.SetText(root, "ErrTipLabel", "The connection 3 or more inflection poins is not allowed.");
        ViewUtils.SetText(root, "TipLabel", "Click to match the same block.");
        ViewUtils.AddButtonClick(root, "Step1/Item2", OnClickStepBtn);
        ViewUtils.AddButtonClick(root, "Step1/Item3", OnClickStepBtn);
        ViewUtils.AddButtonClick(root, "Step2/Item1", OnClickStepBtn);
        ViewUtils.AddButtonClick(root, "Step2/Item4", OnClickStepBtn);
        ViewUtils.AddButtonClick(root, "Step3/Item2", OnClickStepBtn);
        ViewUtils.AddButtonClick(root, "Step3/Item3", OnClickStepBtn);
        ViewUtils.AddButtonClick(root, "OKBtn", OnClickOKBtn);
        SaveModel.GetPlayer().guide = true;
        SaveModel.ForceStorageSave();
    }

    void Refresh()
    {

    }

    void OnClickStepBtn()
    {
        state++;
        ani.SetInteger("State", state);
    }

    void OnClickOKBtn()
    {
        GameUI.Create();
    }
}