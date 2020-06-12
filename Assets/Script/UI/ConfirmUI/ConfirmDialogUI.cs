using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmDialogUI : UIBase {
    public static ConfirmDialogUI Create() {
        ConfirmDialogUI confirmDialogUI = new ConfirmDialogUI();
        confirmDialogUI.Init();
        return confirmDialogUI;
    }

    private UnityAction<bool> callBack;

    public void Init() {
        this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
        this.CreateAndAttachRoot("ConfirmDialogUI");

        ViewUtils.AddButtonClick(this.root, "BtnClose", this.onClickBtnClose);
        ViewUtils.AddButtonClick(this.root, "BtnConfirm", this.onClickBtnConfirm);
       
    }

    private void onClickBtnClose() {
        this.Close();
        if (this.callBack!=null) {
            this.callBack(false);
        }
    }

    private void onClickBtnConfirm() {
        this.Close();
        if (this.callBack != null) {
            this.callBack(true);
        }
    }

    public ConfirmDialogUI SetCallBack(UnityAction<bool> callBack) {
        this.callBack = callBack;
        return this;
    }

    public ConfirmDialogUI SetContent(string msg) {
        Text content = root.FindAChild<Text>("TextContent");
        content.text = msg;
        return this;
    }

    public ConfirmDialogUI SetTitle(string msg) {
        Text title = root.FindAChild<Text>("TextTitle");
        title.text = msg;
        return this;
    }

    void test() {
        //ConfirmDialogUI.Create().SetContent("123").SetCallBack(delegate (bool selected) {
        //    Debug.Log(selected);
        //});
    }
}
