using System.Collections.Generic;
using System;
using UnityEngine;

public class PopUpManager : Singleton<PopUpManager>
{

    public PopUpManager()
    {
        actionsListMap.Put("Global", new List<Action>());
        actionsListMap.Put("League", new List<Action>());
        actionsListMap.Put("Ladder", new List<Action>());
        actionsListMap.Put("MailView", new List<Action>());
    }

    private Map<string, List<Action>> actionsListMap = new Map<string, List<Action>>();

    public void RegisterAction(string viewType, Action action)
    {
        if (string.IsNullOrEmpty(viewType))
        {
            Debug.LogWarning("ViewType can not be null");
        }
        List<Action> actionList = actionsListMap[viewType];
        actionList.Add(action);
    }

    public void DoPopAction(string viewType)
    {
        if (string.IsNullOrEmpty(viewType))
        {
            Debug.LogWarningFormat("Unknown ViewType [{0}]", viewType);
            return;
        }
        // 优先处理局部弹窗，后处理全局弹窗
        List<Action> actionList = actionsListMap[viewType];
        if (actionList.Count > 0)
        {
            Action act = actionList[0];
            act();
            actionList.Remove(act);
            return;
        }

        // 全局弹窗最后处理，否则链式窗口不好弄
        actionList = actionsListMap["Global"];
        if (actionList.Count > 0)
        {
            Action act = actionList[0];
            act();
            actionList.Remove(act);
        }
    }
}
