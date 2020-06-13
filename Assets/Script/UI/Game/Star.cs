using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Star : LineItem
{
    private Vector3 flyPos = Vector3.zero;
    private bool flyFlag = false;
    private float timer = 0;

    void Awake()
    {

    }
    public void initLine(int index, List<Point> pointList, int height , Vector3 flyPos)
    {
        base.initLine(index, pointList, height);
        flyFlag = true;
        this.flyPos = flyPos;
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
        transform.DOMove(this.flyPos, 0.5f).OnComplete(() =>
        {
            DestroyThis();
        });
        //EventDelegate ed = new EventDelegate();
        //ed.Set(this , "DestroyThis");
        //posTween.enabled = true;
        //posTween.from = this.transform.localPosition;
        //posTween.PlayForward();
        //posTween.onFinished.Add(ed);
    }

}

