using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNotEnoughUI : UIBase
{
    public static GoldNotEnoughUI Create()
    {
        GoldNotEnoughUI goldNotEnoughUI = new GoldNotEnoughUI();
        goldNotEnoughUI.Init();
        return goldNotEnoughUI;
    }

    private int gold;
    public void Init()
    {
        this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        this.CreateAndAttachRoot("GoldNotEnoughUI");
        Attach();
    }

    public void Attach()
    {
        ViewUtils.AddButtonClick(this.root, "CloseBtn", this.Close);
        ViewUtils.AddButtonClick(this.root, "AdBtn", this.OnClickADBtn);
        gold = Config.Instance.commonNode.GetInt("adGold");
        ViewUtils.SetText(root, "AdBtn/Text", "+" + gold);
        ViewUtils.SetText(root, "TitleText", "GOLD SHORTAGE");
        ViewUtils.SetText(root, "HintText", "Do you need more gold coins!");
    }

    private void OnClickADBtn()
    {
        IronsoucrManager.Instance.ShowRewardedVideo(() =>
        {
            SaveModel.AddGold(gold);
            MessageCenter.SendMessage(MyMessageType.GAME_UI, MyMessage.REFRESH_RES);
            Close();
        });
    }
}
