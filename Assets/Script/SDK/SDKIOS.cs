using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class SDKIOS : SDKDefault
{

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void EvaluateOc();
    public override void Share()
    {
        CoroutineHelper.Instance.WaitForEndOfFrame(() =>
        {
            string shareText = "I've reached level " + SaveModel.player.level + ", so interesting! Come and play Onet 3D with me! https://play.google.com/store/apps/details?id=com.connetly.puzzle.game";
            new NativeShare().SetSubject("Subject goes here").SetText(shareText).Share();

        });
    }

    public override void Evaluate()
    {
        EvaluateOc();
    }
#endif
}
