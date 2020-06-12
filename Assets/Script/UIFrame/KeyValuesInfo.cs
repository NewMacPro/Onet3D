using System;
using System.Collections.Generic;

[Serializable]
public class KeyValuesInfo
{
    //语言信息
    public List<KeyValuesNode> ConfigInfo = new List<KeyValuesNode>();
}

[Serializable]
public class KeyValuesNode
{
    //键
    public string Key = null;
    //值
    public string Value = null;

    public KeyValuesNode(string Key, string Value)
    {
        this.Key = Key;
        this.Value = Value;
    }
}

