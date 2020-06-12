using System;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPanel : MonoBehaviour {

    int reference;
    Image waitingBg;
    Transform loadingImg;
    public Image loadingImageComp;
    public Image loadingImageCompInside;
    string contentText;
    Text contentLabel;
    private long showTime = -1;
    Transform loadingImgGroup;
    private bool _inited = false;

    // Use this for initialization
    void Start() {
        this.Init();
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake() {

    }

    private void OnEnable() {
        //if (loadingImageComp != null)
        //    loadingImageComp.sprite = iResourceManager.LoadSprite("lodingImg");

        //if (loadingImageCompInside != null)
        //    loadingImageCompInside.sprite = iResourceManager.LoadSprite("loading_2");
    }

    private void OnDisable() {
    }

    // Update is called once per frame
    void Update() {
        if (reference <= 0) {
            reference = 0;
            if (gameObject.activeSelf) {
                gameObject.SetActive(false);
            }
            contentText = "";
            showTime = -1;
        }
        else {
            if (showTime < DateTime.Now.Ticks && showTime > 0) {
                if (!waitingBg.gameObject.activeSelf)
                    waitingBg.gameObject.SetActive(true);
                if (!loadingImgGroup.gameObject.activeSelf)
                    loadingImgGroup.gameObject.SetActive(true);
                if (!contentLabel.gameObject.activeSelf)
                    contentLabel.gameObject.SetActive(true);
            }
        }
        contentLabel.text = contentText;
    }

    private void Init() {
        waitingBg = transform.FindAChild<Image>("ImageMask");
        loadingImg = transform.FindAChild("ImageLoading");
        contentLabel = transform.FindAChild<Text>("TextContent");
        loadingImgGroup = transform.FindAChild("ImageLoadingGroup");
        waitingBg.gameObject.SetActive(true);
        loadingImg.gameObject.SetActive(true);
        contentLabel.gameObject.SetActive(true);
        loadingImgGroup.gameObject.SetActive(true);
        _inited = true;
    }

    public void Show(string content, bool showLogoBg = false, bool needDelayShow = true) {
        if (!_inited) {
            this.Init();
        }
        reference++;
        gameObject.SetActive(true);
        transform.Find("ImageLogo").gameObject.SetActive(showLogoBg);
        if (showLogoBg) {
            //contentLabel.gameObject.SetActive(false);
            // loadingImg.localPosition = Vector3.zero;
        }
        float delayTime = Config.Instance.displayWaitingPanelDelay;
        if (reference == 1 && needDelayShow && delayTime > 0) {
            waitingBg.gameObject.SetActive(false);
            loadingImgGroup.gameObject.SetActive(false);
            //contentLabel.gameObject.SetActive(false);
            showTime = DateTime.Now.Ticks + (long)(delayTime * 10000000.0f);
        }

        contentText = content;
    }

    public void SetConent(string content) {
        contentText = content;
    }

    public void TrueHide() {
        reference = 0;
        // LuaUtil.Log("TrueHide :reference:" + reference);
    }

    public void Hide() {
        reference--;
        if (reference <= 0)
            reference = 0;
        // LuaUtil.Log("reference:" + reference);
    }
}

