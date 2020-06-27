using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
