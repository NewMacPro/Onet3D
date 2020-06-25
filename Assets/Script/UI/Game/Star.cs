using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Star : LineItem
{
    private Vector3 flyPos = Vector3.zero;
    private bool flyFlag = false;
    private float timer = 0;
    private int index;

    void Awake()
    {

    }
    public void initLine(int index, List<Point> pointList, int height , Vector3 flyPos, Item item)
    {
        base.initLine(index, pointList, height, item);
        this.index = index;
        flyFlag = true;
        this.flyPos = flyPos;
        Transform star = transform.FindAChild("Star");
        star.localScale = Vector3.zero;
        star.DOScale(1.2f, 0.3f).OnComplete(() =>
        {
            star.DOScale(1f, 0.1f);
        });
    }

    void Update() {
        if (flyFlag)
        {
            timer += Time.deltaTime;
            if (timer >= Const.STAR_STAY_TIME)
            {
                flyFlag = false;
                fly();
            }
        }
    }

    public void fly()
    {
        hideAllLine();
        transform.DOMove(this.flyPos, Const.STAR_FLY_TIME + Const.STAR_FLY_INTERVAL_TIME * item.pos.x).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            DestroyThis();
            MessageCenter.SendMessage(MyMessageType.GAME_UI, MyMessage.REFRESH_RES);
        });
        //EventDelegate ed = new EventDelegate();
        //ed.Set(this , "DestroyThis");
        //posTween.enabled = true;
        //posTween.from = this.transform.localPosition;
        //posTween.PlayForward();
        //posTween.onFinished.Add(ed);
    }

}

