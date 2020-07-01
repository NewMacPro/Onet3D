using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class LoadingUI : MonoBehaviour
{
    public Text label;
    private float currentProgress = 0;
    private float targetProgress = 0;
    void Awake()
    {
        Attach();
        Refresh();
    }

    void Attach()
    {
        StartLoad();
    }

    void Refresh()
    {

    }

    public void StartLoad()
    {
        currentProgress = 0;
        targetProgress = 0;
        StartCoroutine("Loading");
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

