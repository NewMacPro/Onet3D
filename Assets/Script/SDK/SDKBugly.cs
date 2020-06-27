using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDKBugly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId("");
        BuglyAgent.EnableExceptionHandler();
#elif UNITY_ANDROID
        BuglyAgent.InitWithAppId ("03f21bf475");
        BuglyAgent.EnableExceptionHandler();
#endif

    }

}
