using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    const float SCALE_SMALL = 0.8f;
    const float SCALE_BIG = 1f;
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
        image = transform.FindAChild<Image>("Image");
        bg = transform.FindAChild<Image>("Bg");
        //posTween = this.GetComponent<TweenPosition>();
        //scaTween = this.GetComponent<TweenScale>();
        rect = this.GetComponent<RectTransform>();
        checkMark = transform.FindAChild("CheckMark").gameObject;
        ViewUtils.AddButtonClick(gameObject.transform, "", OnClickItem);

        //posTween.enabled = false;
        //scaTween.enabled = false;
    }

    public void MyClick(BaseEventData data)
    {
        Debug.Log("点击");
    }

    /**设置item类型*/
    public void SetItemType(int typeIndex , int type)
	{
        image.name = GalleryModel.GetImgByType(typeIndex, type, out itemType);
        if (itemType == -1)
        {
            gameObject.SetActive(false);
            return;
        }
        image.sprite = iResourceManager.LoadSprite(image.name);
	}

    public void ChangeGallery(int galleryId, int type)
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }
        cg.DOKill();
        cg.alpha = 0;
        SetItemType(galleryId, type);
        float random = Random.Range(0.5f,1.2f);
        cg.DOFade(1, random);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(SCALE_SMALL, 0.1f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(SCALE_BIG, 0.1f);
    }
}
