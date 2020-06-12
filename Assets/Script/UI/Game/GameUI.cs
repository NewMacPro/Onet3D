using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase
{
    /**背景*/
    private GameObject itemContent;
    /**行号*/
    private int row = 8;
    /**列号*/
    private int col = 5;
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

    private int _score;

    public static void Create()
    {
        GameUI ui = new GameUI();
        ui.Init();
    }

    void Init() {
        Redisplay();
    }

    public override void Redisplay()
    {
        CreateAndAttachRoot("GameUI");
        Attach();
        Refresh();
        InitGame();
    }

    void Attach() {
        scoreText = root.FindAChild<Text>("TopArea/Star/Text");
        levelText = root.FindAChild<Text>("TopArea/Level/Text");
        goldText = root.FindAChild<Text>("TopArea/Gold/Text");
        timeText = root.FindAChild<Text>("TopArea/Time/Text");
        resetText = root.FindAChild<Text>("ResetBtn/Text");
        changeImageText = root.FindAChild<Text>("ImageBtn/Text");
        hintText = root.FindAChild<Text>("HintBtn/Text");

        ViewUtils.AddButtonClick(root, "PauseBtn", OnClickPause);
        ViewUtils.AddButtonClick(root, "ResetBtn", OnClickReset);
        ViewUtils.AddButtonClick(root, "ImageBtn", OnClickImage);
        ViewUtils.AddButtonClick(root, "HintBtn", OnClickHint);
    }

    void Refresh()
    {

    }

    void InitGame() {
        ClearScore();
        InitLevelType();
        InitStartPos();

        RandomType();
        CreateItems();
        CheckHaveCanLink();

    }

    private void ClearScore() {
        _score = 0;
        scoreText.text = "得分:" + _score;
    }

    private void InitStartPos()
    {
        Vector2 bgSize = itemContent.GetComponent<RectTransform>().sizeDelta;
        itemSize = bgSize.x / col;
        startX = itemContent.transform.position.x - (bgSize.x / 2) - itemSize / 2;
        startY = itemContent.transform.position.y + (bgSize.y / 2) + itemSize / 2;
    }

    private void InitLevelType() {
        int levelType = 1;
        itemContent = root.Find("ItemContent" + levelType).gameObject;
        for (int i = 1; i <= 4; i++)
        {
            ViewUtils.SetActive(root, "ItemContent" + i, false);
        }
        ViewUtils.SetActive(root, "ItemContent" + levelType, true);
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
        itemScript.hasItem = type == -1 ? 0 : 1;
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
                List<Point> pathList = GameModel.CheckLink(item1.pos, item2.pos , itemList);
                bool isClear = pathList.Count != 0;
                if (isClear)
                {
                    HideTwoItem(pathList);
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
        item1.hasItem = 0;
        item2.hasItem = 0;
        AddScore(pathList.Count);
        item1.fly();
        item2.fly();
        //Destroy (item1.gameObject);
        //Destroy (item2.gameObject);
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
                        if (item1.hasItem == 0 || item2.hasItem == 0)
                        {
                            continue;
                        }
                        if (item1.itemType != item2.itemType)
                        {
                            continue;
                        }
                        if (GameModel.CheckLink(itemList[i][j].pos, itemList[k][l].pos , itemList).Count != 0)
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
            OnClickReset();
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        scoreText.text = "得分:" + _score;
    }

    private void GameFinish() { 
    
    }

    void OnClickPause() {
        PauseUI.Create();
    }

    void OnClickReset()
    {
        typeList = new List<int>();
        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemList[i].Count; j++)
            {
                if (itemList[i][j].hasItem == 1)
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
                if (itemList[i][j].hasItem == 1)
                {
                    itemList[i][j].SetItemType(typeList[0]);
                    typeList.RemoveAt(0);
                }
            }
        }

        AllItemCancleClick();
        CheckHaveCanLink();
    }

    void OnClickImage()
    {

    }

    void OnClickHint()
    {
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
        GameModel.CheckLink(a, b , itemList);
        //TODO
    }
}