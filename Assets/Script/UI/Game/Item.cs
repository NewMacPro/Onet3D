using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class Item : MonoBehaviour {
	public int itemType;
    private Image image;
    private Image bg;
    private Button uiBtn;
    private RectTransform rect;
	public Point pos;
    public GameUI gameUI;
    private GameObject checkMark;
    private TextTimer textTimer;
    private int totalSce = 12;//40
    private int showSce = 10;
    private Image progress;
    public float nowSec = 0;
    public float lastSec = 0;

    public bool hasItem;
    public bool isBomb;

    //private TweenPosition posTween;
    //private TweenScale scaTween;

	void Awake()
	{
        uiBtn = this.GetComponent<Button>();
        image = transform.Find("Bg/Image").GetComponent<Image>();
        bg = transform.Find("Bg").GetComponent<Image>();
        //posTween = this.GetComponent<TweenPosition>();
        //scaTween = this.GetComponent<TweenScale>();
        rect = this.GetComponent<RectTransform>();
        checkMark = transform.Find("Bg/CheckMark").gameObject;
        ViewUtils.AddButtonClick(gameObject.transform, "", OnClickItem);
        //posTween.enabled = false;
        //scaTween.enabled = false;
	}

	/**设置item类型*/
	public void SetItemType(int type)
	{
		itemType = type;
        if (itemType == -1) {
            gameObject.SetActive(false);
        }
        image.sprite = iResourceManager.LoadSprite("img_jiaju_00" + type);
	}

    public void SetImageBg(int bgIndex) {
        ViewUtils.SetImage(gameObject.transform, "Bg", "ItemBg" + bgIndex);
    }

    public void SetItemSize(float s) {
        rect.sizeDelta = new Vector2(Mathf.FloorToInt(s), Mathf.FloorToInt(s));
    }

	public void OnClickItem()
	{
        checkMark.SetActive(true);
        //scaTween.enabled = true;
        gameUI.ClickItem(this);
	}

    public void CancleClick(){
        //scaTween.ResetToBeginning();
        checkMark.SetActive(false);
        //scaTween.enabled = false;
        transform.localScale = Vector3.one;
    }

    public void HintItem() {
        //scaTween.enabled = true;
    }

	public void fly()
	{
        gameObject.SetActive(false);
        //posTween.enabled = true;
        //posTween.from = this.transform.localPosition;
        //posTween.to = pos;
        //posTween.PlayForward ();
	}

	public void xiaohui()
	{
		Destroy (this.gameObject);
	}

    public void IsBomb(bool isBomb, int bombTime = 12) 
    {
        ViewUtils.SetActive(gameObject.transform, "Bg/Bomb", isBomb);
        this.isBomb = isBomb;
        if (isBomb)
        {
            nowSec = bombTime > 0 ? bombTime : 12;
            lastSec = nowSec;
            Text text = gameObject.transform.FindAChild<Text>("BombText");
            textTimer = text.AddComponent<TextTimer>();
            textTimer.setTextShow(false);
            textTimer.setFormat("{1}");
            textTimer.startTimingBySeconds(Mathf.RoundToInt(nowSec));
            textTimer.setUpdataCallback(BombCallback);
            textTimer.setCallback(SendGameOverMessage);
            progress = gameObject.transform.FindAChild<Image>("Progress");
            Debug.Log(bombTime);
        }
    }

    void Updata() {
        if (textTimer != null && !hasItem)
        {
            textTimer.stopTiming();
            textTimer = null;
        }
    }

    public void BombCallback() {
        if (textTimer == null) {
            return;
        }
        nowSec = textTimer.getTime() / 10000;
        if (Mathf.Abs(lastSec - nowSec)>1)
        {
            lastSec = nowSec;
            SaveModel.player.currentLevel.bobmTime = Mathf.FloorToInt(nowSec);
            SaveModel.ForceStorageSave();
        }
        progress.fillAmount = nowSec / totalSce;
        if (nowSec < showSce)
        {
            textTimer.setTextShow(true);
        }
    }

    public void SendGameOverMessage() {
        isBomb = false;
        ViewUtils.SetActive(gameObject.transform, "Bg/Bomb", false);
        MessageCenter.SendMessage(MyMessageType.GAME_UI, MyMessage.TIME_OUT, null);
        textTimer.stopTiming();
    }
}
