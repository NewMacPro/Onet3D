using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SDKInterface
{

    public delegate void ShareHandler();
    public ShareHandler onShare;

    private static SDKInterface _instance;

    public static SDKInterface Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR || UNITY_STANDLONE
                _instance = new SDKDefault();
#elif UNITY_ANDROID
                _instance = new SDKAndroid();
#elif UNITY_IOS
                _instance = new SDKIOS();
#endif
            }

            return _instance;
        }
    }
    public virtual void Evaluate() { Debug.Log("Not supported"); }
    public virtual void Share() { Debug.Log("Not supported"); }
}
