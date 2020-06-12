using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerHelper : UnitySceneSingleton<SceneManagerHelper> {
    private AsyncOperation async;
    private float currentProgress = 0;
    private float targetProgress = 0;
    private string sceneName;

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void StartLoadScene(string sceneName) {
        this.sceneName = sceneName;
        currentProgress = 0;
        targetProgress = 0;
        UIManager.GetInstance().ShowWaitingPanel("加载中……");
        StartCoroutine("LoadScene");
    }
    IEnumerator LoadScene() {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (currentProgress < 0.9f) {
            targetProgress = async.progress;
            while (currentProgress < targetProgress) {
                currentProgress += 0.05f;
                OnProgress(currentProgress);
                yield return null;
            }
        }

        targetProgress = 1;
        while (currentProgress < targetProgress) {
            currentProgress += 0.01f;
            OnProgress(currentProgress);
            yield return null;
        }
        async.allowSceneActivation = true;
        UIManager.GetInstance().HideWaitingPanel();

    }

    void OnProgress(float progress) {

    }
}
