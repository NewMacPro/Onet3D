using DG.Tweening;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase
{
    const float ITEM_MOVE_TIME = 0.2f;
    /**背景*/
    private GameObject itemContent;
    /**行号*/
    private int row;
    /**列号*/
    private int col;
    /**item宽*/
    private float itemSize = 0;
    /**item起始位置x*/
    private float startX = 0;
    /**item起始位置y*/
    private float startY = 0;
    /**item类型列表*/
    private List<int> typeList = new List<int>();
    /**item列表*/
    private List<List<Item>> itemList;
    /**可连接的列表*/
    private List<List<Item>> canLinkList = new List<List<Item>>();
    /**已点击的列表*/
    private List<Item> clickList = new List<Item>();
    private List<Item> tipItem = new List<Item>();
    private Text scoreText;
    private Text levelText;
    private Text goldText;
    private Text timeText;
    private Text resetText;
    private Text changeImageText;
    private Text hintText;
    private TextTimer textTimer;
    private Item bombItem;
    private bool haveBomb;
    private bool IsTiming;

    private int resetPrice = 120;
    private int changeImagePrice = 30;
    private int hintPrice = 60;
    private int nowLevel;
    private LevelConfig config;
    private int moveType = 0;
    private List<LineItem> tipItemList = new List<LineItem>();
    private int bgIndex = 1;
    private int galleryId = 0; //图集种类
    private CurrentLevel currentLevel;
    private Image viewMask;
    private Coroutine coroutine;
    private int _score;

    public static void Create()
    {
        GameUI ui = new GameUI();
        ui.Init();
    }

    void Init()
    {
        initCurrentLevel();
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("GameUI");
        Attach();
        InitGame();
    }

    void Attach()
    {
        MessageCenter.AddMsgListener(MyMessageType.GAME_UI , OnMessage);
        scoreText = root.FindAChild<Text>("TopArea/Star/Text");
        levelText = root.FindAChild<Text>("TopArea/Level/Text");
        goldText = root.FindAChild<Text>("TopArea/Gold/Text");
        timeText = root.FindAChild<Text>("TopArea/Time/Text");
        resetText = root.FindAChild<Text>("ResetBtn/Text");
        changeImageText = root.FindAChild<Text>("ImageBtn/Text");
        hintText = root.FindAChild<Text>("HintBtn/Text");
        viewMask = root.FindAChild<Image>("ViewMask");
        textTimer = timeText.AddComponent<TextTimer>();

        ViewUtils.AddButtonClick(root, "PauseBtn", OnClickPause);
        ViewUtils.AddButtonClick(root, "ResetBtn", OnClickReset);
        ViewUtils.AddButtonClick(root, "ImageBtn", OnClickImage);
        ViewUtils.AddButtonClick(root, "Gold", OnClickGoldDebug);
        ViewUtils.AddButtonClick(root, "Level", OnClickLevelDebug);
        ViewUtils.AddButtonClick(root, "HintBtn", () =>
        {
            OnClickHint(true);
        });

        ViewUtils.SetText(root, "ResetBtn/Text", resetPrice.ToString());
        ViewUtils.SetText(root, "ImageBtn/Text", changeImagePrice.ToString());
        ViewUtils.SetText(root, "HintBtn/Text", hintPrice.ToString());
        ViewUtils.SetTextColor(root, "ResetBtn/Text",  SaveModel.CheckGold(resetPrice, false) ? Color.white : Color.red);
        ViewUtils.SetTextColor(root, "ImageBtn/Text", SaveModel.CheckGold(changeImagePrice, false) ? Color.white : Color.red);
        ViewUtils.SetTextColor(root, "HintBtn/Text", SaveModel.CheckGold(hintPrice, false) ? Color.white : Color.red);
        if (GameManager.Instance.showInterstitial && SaveModel.player.level > 4)
        {
            IronsoucrManager.Instance.ShowInterstitial();
            GameManager.Instance.showInterstitial = false;
        }
    }

    void OnMessage(KeyValuesUpdate kv)
    {
        if (kv.Key == MyMessage.TIME_OUT)
        {
            GameOver();
        }
        if (kv.Key == MyMessage.REFRESH_RES)
        {
            RefreshGold();
            AddScore(1);
        }
    }

    void initCurrentLevel()
    {
        currentLevel = SaveModel.player.currentLevel;
        if (currentLevel.level != SaveModel.player.level)
        {
            currentLevel = new CurrentLevel();
            SaveModel.player.currentLevel = currentLevel;
            currentLevel.level = SaveModel.player.level;
        }
    }

    void Refresh()
    {
        InitGame();
    }
    void InitGame()
    {
        InitScore();
        InitConfig();
        InitLevel();
        RefreshGold();
        InitStartPos();
        InitTime();

        CreateItems();
        CheckHaveCanLink();
        AutoHint();
        Guide();
    }

    void InitScore()
    {
        ClearScore();
        if (currentLevel.level == SaveModel.player.level)
        {
            AddScore(currentLevel.star, false);
        }
    }

    void InitConfig()
    {
        nowLevel = SaveModel.player.level;
        config = Config.Instance.GetLevelConfigByLevel(nowLevel);
        LevelSize sizeConfig = Config.Instance.GetLevelSizeConfigById(config.size);
        row = sizeConfig.row;
        col = sizeConfig.col;
        haveBomb = config.haveBomb;
        moveType = config.moveType == 5 ? 0 : config.moveType;
    }
    void RestartGame()
    {
        GameUI.Create();
    }

    private void ClearScore()
    {
        _score = 0;
        scoreText.text = _score.ToString();
    }

    private void InitStartPos()
    {
        Vector2 bgSize = itemContent.GetComponent<RectTransform>().sizeDelta;
        itemSize = bgSize.x / col;
        startX = itemContent.transform.position.x - (bgSize.x / 2) - itemSize / 2;
        startY = itemContent.transform.position.y + (bgSize.y / 2) + itemSize / 2;
    }

    private void InitLevel()
    {
        string levelSize = config.size;
        itemContent = root.Find("ItemContent" + levelSize).gameObject;
        for (int i = 1; i <= 4; i++)
        {
            ViewUtils.SetActive(root, "ItemContent" + i, false);
        }
        ViewUtils.SetActive(root, "ItemContent" + levelSize, true);

        ViewUtils.SetText(root, "TopArea/Level/Text", nowLevel.ToString());
        bgIndex = Random.Range(1, 7);
        ViewUtils.SetImage(root, "Bg", "img_bg_00" + bgIndex);
    }

    private void RefreshGold()
    {
        ViewUtils.SetText(root, "TopArea/Gold/Text", SaveModel.player.gold.ToString());
        ViewUtils.SetTextColor(root, "ResetBtn/Text", SaveModel.CheckGold(resetPrice, false) ? Color.white : Color.red);
        ViewUtils.SetTextColor(root, "ImageBtn/Text", SaveModel.CheckGold(changeImagePrice, false) ? Color.white : Color.red);
        ViewUtils.SetTextColor(root, "HintBtn/Text", SaveModel.CheckGold(hintPrice, false) ? Color.white : Color.red);
    }

    private void InitTime()
    {
        int time = config.time;
        if (currentLevel.level == SaveModel.player.level
            && currentLevel.levelRemainTime > 0)
        {
            time = currentLevel.levelRemainTime;
        }
        textTimer.setTimeBySeconds(time);
        textTimer.setCallback(GameOver);
        textTimer.setUpdataCallback(() =>
        {
            int t = Mathf.FloorToInt(textTimer.getTime() / 10000);
            if (Mathf.Abs(t - currentLevel.levelRemainTime)>1)
	        {
                currentLevel.levelRemainTime = t;
                SaveModel.ForceStorageSave();
	        };
        });
        IsTiming = false;
    }

    private void GameOver()
    {
        CoroutineHelper.Instance.Stop(coroutine);
        SaveModel.ClearCurrentLevel();
        StartTiming(false);
        RebornUI.Create(BackToGame);

    }

    private void StartTiming(bool isTiming)
    {
        if (isTiming == IsTiming)
        {
            return;
        }
        IsTiming = isTiming;
        if (IsTiming)
        {
            textTimer.startTiming();
        }
        else
        {
            textTimer.stopTiming();
        }
    }


    private void RandomType()
    {
        for (int i = 0; i < row * col * 0.5; i++)
        {
            int maxCount = GalleryModel.GetGalleryById(galleryId).typeCount;
            int type = Random.Range(1, maxCount + 1);
            typeList.Add(type);
            typeList.Add(type);
        }
        RandomSort1(typeList);
    }

    private void RandomSort1(List<int> list)
    {
        int count = list.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int r = Random.Range(i + 1, count - 1);
            Swap(list, i, r);
        }
    }

    private void Swap(List<int> list, int i, int j)
    {

        int tmp = (int)list[i];
        list[i] = list[j];
        list[j] = tmp;
    }

    /**创建item*/
    private void CreateItems()
    {
        bool newGame = false;
        int totalRow = row + 2;
        int totalCol = col + 2;
        Debug.Log(currentLevel.galleryId);
        if (!SaveModel.IsGalleryUnlock(currentLevel.galleryId))
        {
            currentLevel.galleryId = -1;
        }
        galleryId = currentLevel.galleryId == -1 ? GalleryModel.GetRandomGallery() : currentLevel.galleryId;
        currentLevel.galleryId = galleryId;
        if (currentLevel.itemTypeList.Count != totalRow * totalCol)
        {
            newGame = true;
            RandomType();
        }
        Vector2 bombPos = new Vector2(-1 , -1);
        if (haveBomb)
        {
            bombPos = newGame ? 
                new Vector2(Random.Range(1, col), Random.Range(1, row)) :
                new Vector2(currentLevel.bobmPos % totalCol, Mathf.Floor(currentLevel.bobmPos / totalCol));
        }
        int index = 0;
        itemList = new List<List<Item>>();
        for (int i = 0; i < totalRow; i++)
        {
            List<Item> tmp = new List<Item>();

            for (int j = 0; j < totalCol; j++)
            {
                Item itemScript;
                int type = -1;
                if (!newGame)
                {
                    type = currentLevel.itemTypeList[i * totalCol + j];
                }
                else if (i != 0 && i != row + 1 && j != 0 && j != col + 1)//第一行和最后一行
                {
                    type = (int)typeList[index];
                    index++;
                }
                bool isBomb = j == bombPos.x && i == bombPos.y;
                itemScript = CreateItem(i, j, type, isBomb);
                tmp.Add(itemScript);
                if (isBomb)
                {
                    bombItem = itemScript;
                }
            }
            itemList.Add(tmp);
        }
        SaveModel.ResetItemList(itemList);
    }

    private Item CreateItem(int i, int j, int type, bool isBomb)
    {
        GameObject item = ViewUtils.CreatePrefabAndSetParent(itemContent.transform, "GameItem");
        item.transform.localPosition = GetItemPos(i, j);
        Item itemScript = item.AddComponent<Item>();
        itemScript.IsBomb(isBomb, currentLevel.bobmTime);
        itemScript.SetItemSize(itemSize);
        itemScript.SetItemType(galleryId, type);
        itemScript.gameUI = this;
        itemScript.pos = new Point(i, j);
        itemScript.hasItem = type != -1;
        itemScript.SetImageBg(bgIndex);
        return itemScript;
    }

    private Vector3 GetItemPos(int i, int j)
    {
        float xPos = startX + j * itemSize;
        float yPos = startY - i * itemSize;
        if (i == 0)
        {
            yPos = yPos - itemSize / 4;
        }
        if (i == row + 1)
        {
            yPos = yPos + itemSize / 4;
        }

        if (j == 0)
        {
            xPos = xPos + itemSize / 4;
        }
        if (j == col + 1)
        {
            xPos = xPos - itemSize / 4;
        }

        return new Vector3(xPos, yPos, 0);
    }

    public void ClickItem(Item item)
    {
        ClearTip();
        AutoHint();
        if (clickList.Count == 1)
        {
            Item item1 = clickList[0];
            if (item1 == item)
            {
                return;
            }
            clickList.Add(item);
            Item item2 = clickList[1];
            AllItemCancleClick();
            if (item1.itemType == item2.itemType)
            {
                List<Point> pathList = GameModel.CheckLink(item1.pos, item2.pos, itemList);
                bool isClear = pathList.Count != 0;
                if (isClear)
                {
                    StartTiming(true);
                    HideTwoItem(pathList);
                }
                else
                {
                    clickList.Clear();
                    item.OnClickItem();
                }
            }
            else
            {
                clickList.Clear();
                item.OnClickItem();
            }
        }
        else
        {
            clickList.Clear();
            clickList.Add(item);
        }
    }

    private void ClearTip()
    {
        if (tipItemList.Count > 0)
        {
            foreach (LineItem li in tipItemList)
            {
                li.DestroyThis();
            }
            tipItemList.Clear();
        }
        if (tipItem.Count > 0)
        {
            foreach (Item li in tipItem)
            {
                li.StopTwinkle();
            }
            tipItem.Clear();
        }
    }

    private void HideTwoItem(List<Point> pathList)
    {
        CreateAllStar(pathList);
        Item item1 = clickList[0];
        Item item2 = clickList[1];
        item1.hasItem = false;
        item2.hasItem = false;
        item1.fly();
        item2.fly();
        //Destroy (item1.gameObject);
        //Destroy (item2.gameObject);
        DoMoveAni();
        clickList.Clear();
        pathList.Clear();
        AddScore(pathList.Count);

        AudioManager.Instance.PlaySingle("Sound/clear");

        if (GameModel.IsFinish(itemList))
        {
            SaveModel.ResetItemList(null);
            GameFinish();
            return;
        }
        SaveModel.ResetItemList(itemList);
        CheckHaveCanLink();
    }

    private void CreateAllStar(List<Point> pathList)
    {
        Transform node = this.root.FindAChild("StarNode");
        for (int i = 0; i < pathList.Count; i++)
        {
            Item item = itemList[pathList[i].x][pathList[i].y];
            GameObject starItem = ViewUtils.CreatePrefabAndSetParent(itemContent.transform, "StarItem");
            starItem.transform.localPosition = item.transform.localPosition;
            Star star = starItem.AddComponent<Star>();
            star.initLine(i, pathList, Mathf.CeilToInt(itemSize + 1), node.position, item);
        }
    }

    private void CreateTipLine(List<Point> pathList)
    {
        tipItemList.Clear();
        for (int i = 0; i < pathList.Count; i++)
        {
            Item item = itemList[pathList[i].x][pathList[i].y];
            GameObject tipItem = ViewUtils.CreatePrefabAndSetParent(itemContent.transform, "TipItem");
            tipItem.transform.localPosition = item.transform.localPosition;
            tipItem.transform.SetSiblingIndex(1);
            LineItem line = tipItem.AddComponent<LineItem>();
            line.initLine(i, pathList, Mathf.CeilToInt(itemSize + 1), item);
            tipItemList.Add(line);
        }
    }

    private void AllItemCancleClick()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemList[i].Count; j++)
            {
                itemList[i][j].CancleClick();
            }
        }
    }

    private void CheckHaveCanLink()
    {
        canLinkList.Clear();
        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemList[i].Count; j++)
            {
                for (int k = i; k < itemList.Count; k++)
                {
                    for (int l = 0; l < itemList[k].Count; l++)
                    {
                        if (k == i && l <= j)
                        {
                            continue;
                        }
                        Item item1 = itemList[i][j];
                        Item item2 = itemList[k][l];
                        if (!item1.hasItem || !item2.hasItem)
                        {
                            continue;
                        }
                        if (item1.itemType != item2.itemType)
                        {
                            continue;
                        }
                        if (GameModel.CheckLink(item1.pos, item2.pos, itemList).Count != 0)
                        {
                            List<Item> tempList = new List<Item>();
                            tempList.Add(item1);
                            tempList.Add(item2);
                            canLinkList.Add(tempList);
                        }
                    }
                }
            }
        }
        if (canLinkList.Count == 0)
        {
            Debug.Log("需要洗牌");
            ResetCard();
        }
    }

    public void AddScore(int score, bool showAni = true)
    {
        _score += score;
        currentLevel.star = _score;
        scoreText.text = _score.ToString();;
        SaveModel.ForceStorageSave();
    }

    private void GameFinish()
    {
        CoroutineHelper.Instance.Stop(coroutine);
        SaveModel.ClearCurrentLevel();
        StartTiming(false);
        int useTime = config.time - (int)textTimer.getTime() / 10000;
        GameManager.Instance.gameNum++;
        WinUI.Create(_score, useTime);
        SaveModel.LevelUp();
    }

    void OnClickPause()
    {
        StartTiming(false);
        PauseUI.Create(BackToGame);
        FBstatistics.LogEvent("gamepause");
    }

    void ResetCard()
    {
        typeList.Clear();
        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemList[i].Count; j++)
            {
                if (itemList[i][j].hasItem)
                {
                    typeList.Add(itemList[i][j].itemType);
                }
            }
        }

        RandomSort1(typeList);

        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemList[i].Count; j++)
            {
                if (itemList[i][j].hasItem)
                {
                    itemList[i][j].SetItemType(galleryId, typeList[0]);
                    typeList.RemoveAt(0);
                }
            }
        }

        AllItemCancleClick();
        CheckHaveCanLink();
    }
    void OnClickReset()
    {
        Dictionary<string,object> param = new Dictionary<string,object>();
        param["name"] = "refresh";
        FBstatistics.LogEvent("clicktool", param);

        if (!SaveModel.CheckGold(resetPrice))
        {
            return;
        }
        SaveModel.UseGold(resetPrice);
        RefreshGold();
        ResetCard();
        SaveModel.ResetItemList(itemList);

    }

    void OnClickImage()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "changepicture";
        FBstatistics.LogEvent("clicktool", param);

        if (GalleryModel.alreadyGalleryData.Count < 2)
        {
            return;
        }
        if (!SaveModel.CheckGold(changeImagePrice))
        {
            return;
        }
        SaveModel.UseGold(changeImagePrice);
        RefreshGold();
        int nowType = galleryId;
        int whileCount = 0;
        int whileMaxCount = 1000 ;
        while (nowType == galleryId && whileCount < whileMaxCount)
        {
            whileCount++;
            galleryId = GalleryModel.GetRandomGallery();
        }
        currentLevel.galleryId = galleryId;
        SaveModel.ResetItemList(itemList);
        viewMask.color = Color.white;
        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemList[i].Count; j++)
            {
                Item item = itemList[i][j];
                item.SetAlpha(0);
            }
        }
        viewMask.DOFade(0, 0.5f).OnComplete(() =>
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                for (int j = 0; j < itemList[i].Count; j++)
                {
                    Item item = itemList[i][j];
                    item.ChangeGallery(galleryId, item.itemType);
                }
            }
        });
    }

    void OnClickHint(bool needGold)
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param["name"] = "prompt";
        FBstatistics.LogEvent("clicktool", param);

        if (tipItemList.Count > 0)
        {
            return;
        }
        if (needGold)
        {
            if (!SaveModel.CheckGold(hintPrice))
            {
                return;
            }
            SaveModel.UseGold(hintPrice);

        }
        ClearTip();
        RefreshGold();
        AllItemCancleClick();
        int rand = Random.Range(0, canLinkList.Count);
        Item item1 = canLinkList[rand][0];
        Item item2 = canLinkList[rand][1];
        item1.HintItem();
        item2.HintItem();
        CreatLine(item1.pos, item2.pos);
        tipItem.Add(item1);
        tipItem.Add(item2);
    }

    void AutoHint()
    {
        if (SaveModel.player.level > 1)
        {
            return;
        }
        CoroutineHelper.Instance.Stop(coroutine);
        coroutine = CoroutineHelper.Instance.WaitForSeconds(3f, () =>
        {
            OnClickHint(false);
        });
    }

    public void CreatLine(Point a, Point b)
    {
        List<Point> pathList = GameModel.CheckLink(a, b, itemList);
        CreateTipLine(pathList);
    }

    void BackToGame(string param)
    {
        if (param == GameModel.BACK_GAME_CLOSE)
        {
            BackToMainUI();
        }
        else if (param == GameModel.BACK_GAME_CONTIUE)
        {
            StartTiming(true);
            if (SaveModel.player.level > 4)
            {
                IronsoucrManager.Instance.ShowInterstitial();
            }
        }
        else if (param == GameModel.BACK_GAME_RESTART)
        {
            currentLevel.level = -1;
            SaveModel.ForceStorageSave();
            RestartGame();
        }
        else if (param == GameModel.BACK_GAME_FAIL)
        {
            LoseUI.Create();
        }
        else if (param == GameModel.BACK_GAME_ADDTIME)
        {
            textTimer.startTimingBySeconds(120);
            currentLevel = SaveModel.player.currentLevel;
            currentLevel.levelRemainTime = Mathf.FloorToInt(textTimer.getTime() / 10000);
            currentLevel.level = SaveModel.player.level;
            currentLevel.star = _score;
            SaveModel.ResetItemList(itemList);
        }
    }

    void BackToMainUI()
    {
        CoroutineHelper.Instance.Stop(coroutine);
        GameManager.Instance.showInterstitial = true;
        UIManager.GetInstance().ShowLobbyView();
    }

    public void MoveVet(bool toMin = false)
    {
        List<List<Item>> listCol = new List<List<Item>>();
        foreach (List<Item> items in itemList)
        {
            foreach (Item it in items)
            {
                List<Item> its = null;
                if (listCol.Count > it.pos.y)
                {
                    its = listCol[it.pos.y];
                }
                else
                {
                    its = new List<Item>();
                    listCol.Add(its);
                }
                its.Add(it);
            }
        }
        for (int i = 0; i < listCol.Count; i++)
        {
            //if (i != point1.y && i != point2.y)
            //{
            //    continue;
            //}
            List<Item> items = listCol[i];
            List<int> noneList = new List<int>();
            for (int j = 0; j < items.Count; j++)
            {
                int index = toMin ? j : items.Count - j - 1;
                Item it = items[index];
                if (it.itemType == -1)
                {
                    continue;
                }
                if (it.hasItem)
                {
                    if (noneList.Count <= 0)
                    {
                        continue;
                    }
                    itemList[it.pos.x][i] = itemList[noneList[0]][i];
                    items[it.pos.x] = itemList[noneList[0]][i];
                    itemList[noneList[0]][i] = it;
                    items[noneList[0]] = it;
                    it.transform.DOLocalMove(GetItemPos(noneList[0], i), ITEM_MOVE_TIME).SetDelay(Const.STAR_STAY_TIME);
                    //it.transform.localPosition = GetItemPos(noneList[0], i);
                    noneList.Add(it.pos.x);
                    //itemList[it.pos.x][i].pos.x = it.pos.x;
                    //it.pos.x = noneList[0];
                    noneList.RemoveAt(0);
                    continue;
                }
                noneList.Add(it.pos.x);
            }
            items = listCol[i];
            for (int j = 0; j < items.Count; j++)
            {
                Item it = items[j];
                it.pos.x = j;
                if (!it.hasItem)
                {
                    it.transform.localPosition = GetItemPos(it.pos.x, it.pos.y);
                }
            }
        }
    }

    public void MoveHor(bool toMin = true)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            //if (i != point1.x && i != point2.x)
            //{
            //    continue;
            //}
            List<Item> items = itemList[i];
            List<int> noneList = new List<int>();
            for (int j = 0; j < items.Count; j++)
            {
                int index = toMin ? j : items.Count - j - 1;
                Item it = items[index];
                if (it.itemType == -1)
                {
                    continue;
                }
                if (it.hasItem)
                {
                    if (noneList.Count <= 0)
                    {
                        continue;
                    }
                    itemList[i][it.pos.y] = itemList[i][noneList[0]];
                    itemList[i][noneList[0]] = it;
                    it.transform.DOLocalMove(GetItemPos(i, noneList[0]), ITEM_MOVE_TIME).SetDelay(Const.STAR_STAY_TIME);
                    //it.transform.localPosition = GetItemPos(i, noneList[0]);
                    noneList.Add(it.pos.y);
                    //itemList[i][it.pos.y].pos.y = it.pos.y;
                    //it.pos.y = noneList[0];
                    noneList.RemoveAt(0);
                    continue;
                }
                noneList.Add(it.pos.y);

            }
            items = itemList[i];
            for (int j = 0; j < items.Count; j++)
            {
                Item it = items[j];
                it.pos.y = j;
                if (!it.hasItem)
                {
                    it.transform.localPosition = GetItemPos(it.pos.x, it.pos.y);
                }
            }
        }
    }

    public void DoMoveAni()
    {
        if (config.moveType == 0)
        {
            return;
        }
        if (config.moveType == 5)
        {
            moveType = moveType % 4 + 1;
        }
        if (moveType == 1)
        {
            MoveVet();
        }
        if (moveType == 2)
        {
            MoveHor();
        }
        if (moveType == 3)
        {
            MoveVet(true);
        }
        if (moveType == 4)
        {
            MoveHor(false);
        }
    }
    private void Guide()
    {
        if (bombItem == null || SaveModel.GetPlayer().bobmGuide)
        {
            return;
        }
        //StartTiming(false);
        GuideUI.Create(bombItem.gameObject, () =>
        {
            //StartTiming(true);
        }, "Get bomb card of connecting to avoid exploading or you will fail!");
        SaveModel.GetPlayer().bobmGuide = true;
        SaveModel.ForceStorageSave();
    }

    public override void OnDestroyRoot()
    {
        CoroutineHelper.Instance.Stop(coroutine);
        MessageCenter.RemoveMsgListener(MyMessageType.GAME_UI , OnMessage);
    }

    private void OnClickGoldDebug()
    {
#if UNITY_EDITOR
        SaveModel.AddGold(10000);
        RefreshGold();
#endif
    }

    private void OnClickLevelDebug()
    {
#if UNITY_EDITOR
        SaveModel.LevelUp();
        GameManager.Instance.gameNum++;
        GameUI.Create();
#endif
    }
}