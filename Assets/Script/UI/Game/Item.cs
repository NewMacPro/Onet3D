using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class Item : MonoBehaviour
{
    const int TOTAL_TIME = 40;
    const int TIP_TIME = 10;
	public int itemType;
    private Image image;
    private Image bg;
    private Button uiBtn;
    private RectTransform rect;
	public Point pos;
    public GameUI gameUI;
    private GameObject checkMark;
    private TextTimer textTimer;
    private int totalSce = TOTAL_TIME;
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
	public void SetItemType(int typeIndex , int type)
	{
		itemType = type;
        string name = GalleryModel.galleryImageName[typeIndex];
        if (itemType == -1) {
            gameObject.SetActive(false);
        }
        Debug.Log("img_" + name + "_" + type);
        image.sprite = iResourceManager.LoadSprite("img_" + name +"_" + type);
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

    public void IsBomb(bool isBomb, int bombTime = TOTAL_TIME) 
    {
        ViewUtils.SetActive(gameObject.transform, "Bg/Bomb", isBomb);
        this.isBomb = isBomb;
        if (isBomb)
        {
            nowSec = bombTime > 0 ? bombTime : TOTAL_TIME;
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
        if (nowSec < TIP_TIME)
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
