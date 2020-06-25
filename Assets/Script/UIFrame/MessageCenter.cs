///***
// *
// *    Title: UI框架
// *           主题： 消息中心
// *    Description:
// *          功能：  负责本UI框架，以及整个项目的消息传递工作。
// *    Date: 2017
// *    Version: 0.1版本
// *    Modify Recoder:
// *
// *
// */
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class MessageCenter
{
    /// <summary>
    /// 委托：消息传递
    /// </summary>
    /// <param name="kv">消息类型与数值</param>
    public delegate void DelMessageDelivery(KeyValuesUpdate kv);

    //<所要监听的类型，监听到以后要执行的委托>
    public static Dictionary<int, DelMessageDelivery> dicMessages = new Dictionary<int, DelMessageDelivery>();

    /// <summary>
    /// 添加消息监听
    /// </summary>
    /// <param name="messageType">消息类型</param>
    /// <param name="handler">消息委托</param>
    public static void AddMsgListener(int messageType, DelMessageDelivery handler)
    {
        if (!dicMessages.ContainsKey(messageType))
        {
            dicMessages.Add(messageType, null);
        }
        dicMessages[messageType] += handler;
    }

    /// <summary>
    /// 取消指定消息监听
    /// </summary>
    /// <param name="messageType">消息类型</param>
    /// <param name="handler">消息委托</param>
    public static void RemoveMsgListener(int messageType, DelMessageDelivery handler)
    {
        if (dicMessages.ContainsKey(messageType))
        {
            dicMessages[messageType] -= handler;
        }
    }

    public static void RemoveMsgListener(int messageType)
    {

        if (dicMessages.ContainsKey(messageType))
        {
            dicMessages[messageType] = null;
            dicMessages.Remove(messageType);
        }
    }


    /// <summary>
    /// 取消所有的消息监听
    /// </summary>
    public static void RemoveAllMsgListener()
    {
        dicMessages.Clear();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="kv"></param>
    public static void SendMessage(int messageType, KeyValuesUpdate kv)
    {
        DelMessageDelivery del;
        if (dicMessages.TryGetValue(messageType, out del))
        {
            if (del != null)
            {
                del(kv);
            }
        }
    }

    public static void SendMessage(int messageType, int msgName, object msgContent = null)
    {
        KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
        MessageCenter.SendMessage(messageType, kvs);
    }
}//Class_end

/// <summary>
/// 键值更新
/// 功能：配合委托，实现委托数据传递
/// </summary>
public class KeyValuesUpdate
{
    //键
    private int _Key;
    //值
    private object _Values;

    /* 只读属性 */
    public int Key
    {
        get { return _Key; }
    }
    public object Values
    {
        get { return _Values; }
    }

    public KeyValuesUpdate(int key, object Values)
    {
        _Key = key;
        _Values = Values;
    }
}//Class_end
