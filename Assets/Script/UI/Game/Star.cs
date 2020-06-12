using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
    private GameObject lineU;
    private GameObject lineD;
    private GameObject lineL;
    private GameObject lineR;
    private Vector3 flyPos = Vector3.zero;
    private bool flyFlag = false;
    private float time = 0.5f;
    private float timer = 0;

    void Awake()
    {

    }
    public void initLine(int index, List<Point> pointList, int height , Vector3 flyPos)
    {

        lineU = gameObject.transform.Find("LineU").gameObject;
        lineD = gameObject.transform.Find("LineD").gameObject;
        lineL = gameObject.transform.Find("LineL").gameObject;
        lineR = gameObject.transform.Find("LineR").gameObject;

        hideAllLine();
        Vector2 size = lineU.transform.GetComponent<RectTransform>().sizeDelta;
        lineU.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(height / 2,size.y);
        lineD.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(height / 2,size.y);
        lineL.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(height / 2,size.y);
        lineR.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(height / 2,size.y);
        if (index != 0)
        {
            checkShowLine(pointList[index], pointList[index - 1]);
        }

        if (index != pointList.Count-1)
        {
            checkShowLine(pointList[index], pointList[index + 1]);
        }

        flyFlag = true;
        this.flyPos = flyPos;
    }

    private void hideAllLine() {
        lineU.SetActive(false);
        lineD.SetActive(false);
        lineL.SetActive(false);
        lineR.SetActive(false);
    }

    private void checkShowLine(Point now , Point next) {
        if (now.x > next.x)
        {
            gameObject.transform.Find("LineU").gameObject.SetActive(true);
        }
        if (now.x < next.x)
        {
            gameObject.transform.Find("LineD").gameObject.SetActive(true);
        }
        if (now.y > next.y)
        {
            gameObject.transform.Find("LineL").gameObject.SetActive(true);
        }
        if (now.y < next.y)
        {
            gameObject.transform.Find("LineR").gameObject.SetActive(true);
        }
    }

    void Update() {
        if (flyFlag)
        {
            timer += Time.deltaTime;
            if (timer >= time)
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

    public void DestroyThis() {
        Destroy(this.gameObject);
    }
}

