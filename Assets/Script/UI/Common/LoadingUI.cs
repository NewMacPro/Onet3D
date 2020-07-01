using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

class LoadingUI : MonoBehaviour
{
    const float TIPS_CHANGE_TIME = 2;
    static List<string> TIPS = new List<string>(){"111", "222", "333"};
    public Text label;
    public Text tipsLabel;
    private float currentProgress = 0;
    private float targetProgress = 0;
    private float tipsTime = 0;
    void Awake()
    {
        StartLoad();
        ChangeTip();
    }

    void Update()
    {
        tipsTime += Time.deltaTime;
        if (tipsTime >= TIPS_CHANGE_TIME)
        {
            tipsTime = 0;
            ChangeTip();
        }
    }

    public void StartLoad()
    {
        currentProgress = 0;
        targetProgress = 0;
        StartCoroutine("Loading");
    }

    public void ChangeTip()
    {
        int random = Random.Range(0, TIPS.Count);
        tipsLabel.text = TIPS[random];
    }

    IEnumerator Loading()
    {
        Debug.Log(currentProgress);
        while (currentProgress < 1f)
        {
            float r1 = Random.Range(0f, 0.05f);
            currentProgress += r1;
            float r2 = Random.Range(0f, 0.2f);
            OnProgress(currentProgress);
            yield return new WaitForSeconds(r2);
        }
    }

    void OnProgress(float progress)
    {
        label.text = Mathf.FloorToInt(progress * 100) + "%";
        if (progress >= 1)
        {
            SceneManager.LoadScene("Hall");
        }
    }
}

