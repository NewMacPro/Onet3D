using System;
using System.Collections.Generic;
using UnityEngine;
public class SDKAndroid : SDKDefault
{

#if UNITY_ANDROID

    public override void Share()
    {
        CoroutineHelper.Instance.WaitForEndOfFrame(() =>
        {

            new NativeShare().SetSubject("Subject goes here").SetText("Share!").Share();

        });
    }

    public override void Evaluate()
    {
        AndroidJavaClass ujc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mainActivity = ujc.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass jc = new AndroidJavaClass("com.connetly.puzzle.game.Evaluate");
        jc.CallStatic("evaluate", mainActivity);
    }
#endif
}
