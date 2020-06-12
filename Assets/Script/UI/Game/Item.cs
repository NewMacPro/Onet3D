using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Item : MonoBehaviour {
	public int itemType;
    private Image image;
    private Image bg;
    private Button uiBtn;
    private Vector2 size;
	public Point pos;
    public GameUI gameUI;
    private GameObject checkMark;

	public bool hasItem;

    //private TweenPosition posTween;
    //private TweenScale scaTween;

	void Awake()
	{
        uiBtn = this.GetComponent<Button>();
        image = transform.Find("Image").GetComponent<Image>();
        bg = transform.Find("Bg").GetComponent<Image>();
        //posTween = this.GetComponent<TweenPosition>();
        //scaTween = this.GetComponent<TweenScale>();
        size = this.GetComponent<RectTransform>().rect.size;
        checkMark = transform.Find("CheckMark").gameObject;
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

    public void SetItemSize(float s) {
        size.Set(Mathf.FloorToInt(s), Mathf.FloorToInt(s));
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
}
