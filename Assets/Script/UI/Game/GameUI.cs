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
    private List<int> typeList;
    /**item列表*/
    private List<List<Item>> itemList;
    /**可连接的列表*/
    private List<List<Item>> canLinkList = new List<List<Item>>();
    /**已点击的列表*/
    private List<Item> clickList = new List<Item>();
    private Text scoreText;
    private Text levelText;
    private Text goldText;
    private Text timeText;
    private Text resetText;
    private Text changeImageText;
    private Text hintText;
    private TextTimer textTimer;
    private bool IsTiming;

    private int resetPrice = 120;
    private int changeImagePrice = 30;
    private int hintPrice = 60;
    private int nowLevel;
    private LevelConfig config;

    private int _score;

    public static void Create()
    {
        GameUI ui = new GameUI();
        ui.Init();
    }

    void Init()
    {
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("GameUI");
        Attach();
        Refresh();
    }

    void Attach()
    {
        scoreText = root.FindAChild<Text>("TopArea/Star/Text");
        levelText = root.FindAChild<Text>("TopArea/Level/Text");
        goldText = root.FindAChild<Text>("TopArea/Gold/Text");
        timeText = root.FindAChild<Text>("TopArea/Time/Text");
        resetText = root.FindAChild<Text>("ResetBtn/Text");
        changeImageText = root.FindAChild<Text>("ImageBtn/Text");
        hintText = root.FindAChild<Text>("HintBtn/Text");
        textTimer = timeText.AddComponent<TextTimer>();

        ViewUtils.AddButtonClick(root, "PauseBtn", OnClickPause);
        ViewUtils.AddButtonClick(root, "ResetBtn", OnClickReset);
        ViewUtils.AddButtonClick(root, "ImageBtn", OnClickImage);
        ViewUtils.AddButtonClick(root, "HintBtn", OnClickHint);

        ViewUtils.SetText(root, "ResetBtn/Text", resetPrice.ToString());
        ViewUtils.SetText(root, "ImageBtn/Text", changeImagePrice.ToString());
        ViewUtils.SetText(root, "HintBtn/Text", hintPrice.ToString());

    }

    void Refresh()
    {
        InitGame();
    }

    void InitGame()
    {
        InitConfig();
        ClearScore();
        InitLevel();
        RefreshGold();
        InitStartPos();
        InitTime();

        RandomType();
        CreateItems();
        CheckHaveCanLink();
    }

    void InitConfig()
    {
        nowLevel = SaveModel.player.level;
        config = Config.Instance.GetLevelConfigByLevel(nowLevel);
        LevelSize sizeConfig = Config.Instance.GetLevelSizeConfigById(config.size);
        row = sizeConfig.row;
        col = sizeConfig.col;
    }
    void RestartGame()
    {
        Refresh();
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
    }

    private void RefreshGold()
    {
        ViewUtils.SetText(root, "TopArea/Gold/Text", SaveModel.player.gold.ToString());
    }

    private void InitTime()
    {
        int time = config.time;
        textTimer.setTimeBySeconds(time);
        textTimer.setCallback(GameOver);
        IsTiming = false;
    }

    private void GameOver()
    {
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
        typeList = new List<int>();
        for (int i = 0; i < row * col * 0.5; i++)
        {
            int type = Random.Range(1, 6);
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
        int index = 0;
        itemList = new List<List<Item>>();
        for (int i = 0; i <= row + 1; i++)
        {
            List<Item> tmp = new List<Item>();

            for (int j = 0; j < col + 2; j++)
            {
                Item itemScript;
                int type = -1;
                //第一行和最后一行
                if (i != 0 && i != row + 1 && j != 0 && j != col + 1)
                {
                    type = (int)typeList[index];
                    index++;
                }
                itemScript = CreateItem(i, j, type);
                tmp.Add(itemScript);
            }
            itemList.Add(tmp);
        }
    }

    private Item CreateItem(int i, int j, int type)
    {
        GameObject item = ViewUtils.CreatePrefabAndSetParent(itemContent.transform, "GameItem");
        item.transform.localPosition = GetItemPos(i, j);
        Item itemScript = item.AddComponent<Item>();
        itemScript.SetItemSize(itemSize);
        itemScript.SetItemType(type);
        itemScript.gameUI = this;
        itemScript.pos = new Point(i, j);
        itemScript.hasItem = type != -1;
        return itemScript;
    }

    private Vector3 GetItemPos(int i, int j)
    {
        float xPos = startX + j * itemSize;
        float yPos = startY - i * itemSize;
        if (i == 0)
        {
            yPos = yPos - itemSize / 2;
        }
        if (i == row + 1)
        {
            yPos = yPos + itemSize / 2;
        }

        if (j == 0)
        {
            xPos = xPos + itemSize / 2;
        }
        if (j == col + 1)
        {
            xPos = xPos - itemSize / 2;
        }

        return new Vector3(xPos, yPos, 0);
    }

    public void ClickItem(Item item)
    {
        if (clickList.Count == 1)
        {
            clickList.Add(item);
            Item item1 = clickList[0];
            Item item2 = clickList[1];
            AllItemCancleClick();
            if (item1.itemType == item2.itemType)
            {
                List<Point> pathList = GameModel.CheckLink(item1.pos, item2.pos, itemList);
                bool isClear = pathList.Count != 0;
                if (isClear)
                {
                    HideTwoItem(pathList);
                    StartTiming(true);
                }
                else
                {
                    clickList.Clear();
                }
            }
            else
            {
                clickList.Clear();
            }
        }
        else
        {
            clickList.Clear();
            clickList.Add(item);
        }
    }

    private void HideTwoItem(List<Point> pathList)
    {
        CreateAllStar(pathList);
        Item item1 = clickList[0];
        Item item2 = clickList[1];
        item1.hasItem = false;
        item2.hasItem = false;
        AddScore(pathList.Count);
        item1.fly();
        item2.fly();
        //Destroy (item1.gameObject);
        //Destroy (item2.gameObject);
        MoveVet(item1.pos, item2.pos);
        clickList.Clear();
        pathList.Clear();

        if (GameModel.IsFinish(itemList))
        {
            GameFinish();
            return;
        }
        CheckHaveCanLink();
    }

    private void CreateAllStar(List<Point> pathList)
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            Item item = itemList[pathList[i].x][pathList[i].y];
            GameObject starItem = ViewUtils.CreatePrefabAndSetParent(itemContent.transform, "StarItem");
            starItem.transform.localPosition = item.transform.localPosition;
            Star star = starItem.AddComponent<Star>();
            star.initLine(i, pathList, Mathf.CeilToInt(itemSize + 1), scoreText.transform.position);
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
                    for (int l = j; l < itemList[k].Count; l++)
                    {
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
                        if (GameModel.CheckLink(itemList[i][j].pos, itemList[k][l].pos, itemList).Count != 0)
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

    public void AddScore(int score)
    {
        _score += score;
        scoreText.text = _score.ToString();
    }

    private void GameFinish()
    {
        StartTiming(false);
        int useTime = config.time - (int)textTimer.getTime() / 10000;
        WinUI.Create(_score, useTime);
        SaveModel.LevelUp();
    }

    void OnClickPause()
    {
        StartTiming(false);
        PauseUI.Create(BackToGame);
    }

    void ResetCard()
    {
        typeList = new List<int>();
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
                    itemList[i][j].SetItemType(typeList[0]);
                    typeList.RemoveAt(0);
                }
            }
        }

        AllItemCancleClick();
        CheckHaveCanLink();
    }
    void OnClickReset()
    {
        if (!SaveModel.CheckGold(resetPrice))
        {
            return;
        }
        SaveModel.UseGold(resetPrice);
        ResetCard();

    }

    void OnClickImage()
    {
        if (!SaveModel.CheckGold(changeImagePrice))
        {
            return;
        }
        SaveModel.UseGold(changeImagePrice);
    }

    void OnClickHint()
    {
        if (!SaveModel.CheckGold(hintPrice))
        {
            return;
        }
        SaveModel.UseGold(hintPrice);
        AllItemCancleClick();
        int rand = Random.Range(0, canLinkList.Count);
        Item item1 = canLinkList[rand][0];
        Item item2 = canLinkList[rand][1];
        item1.HintItem();
        item2.HintItem();
        CreatLine(item1.pos, item2.pos);
    }

    public void CreatLine(Point a, Point b)
    {
        GameModel.CheckLink(a, b, itemList);
        //TODO
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
        }
        else if (param == GameModel.BACK_GAME_RESTART)
        {
            RestartGame();
        }
        else if (param == GameModel.BACK_GAME_FAIL)
        {
            LoseUI.Create();
        }
        else if (param == GameModel.BACK_GAME_ADDTIME)
        {
            textTimer.startTimingBySeconds(120);
        }
    }

    void BackToMainUI()
    {
        UIManager.GetInstance().ShowLobbyView();
    }

    public void MoveVet(Point point1, Point point2, bool toMin = false)
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
            if (i != point1.y && i != point2.y)
            {
                continue;
            }
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

    public void MoveHor(Point point1, Point point2, bool toMin = true)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (i != point1.x && i != point2.x)
            {
                continue;
            }
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
}