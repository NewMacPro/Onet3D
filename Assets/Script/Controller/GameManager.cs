using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : UnitySingleton<GameManager>
{
    bool init = false;
    public int gameNum = 0;
    public bool showInterstitial = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        if (init)
        {
            return;
        }
        init = true;
        Config.Instance.Init();
        SaveModel.DeSerialize();
        UIManager.GetInstance().ShowLobbyView();
        FBstatistics.Instance.Init();
        InAppPurchasing.Instance.Init();
        IronsoucrManager.Instance.Init();
        IronsoucrManager.Instance.LoadBanner();
        AudioManager.Instance.PlayBGM("Sound/bg");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject currentObject = EventSystem.current.currentSelectedGameObject;
            if (currentObject != null && currentObject.GetComponent<Button>() != null)
            {
                if (currentObject.GetComponent<Button>() != null)
                {
                    AudioManager.Instance.PlaySingle("Sound/click");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
