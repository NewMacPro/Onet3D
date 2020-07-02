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
            string shareText = "I've reached level " + SaveModel.player.level + ", so interesting! Come and play Onet 3D with me! https://play.google.com/store/apps/details?id=com.connetly.puzzle.game"
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
