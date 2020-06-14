﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Save
{
    public string name = "";
    public int level = 1;
    public int gold = 100;
    public bool soundSwitch = true;
    public bool musicSwitch = true;
    public bool notifySwitch = true;
    public CurrentLevel currentLevel = new CurrentLevel();
}

public class CurrentLevel
{
    public int level = 0;
    public List<int> itemTypeList = new List<int>();
    public int bobmPos = -1;
    public int bobmTime = 0;
    public int star = 0;
}
