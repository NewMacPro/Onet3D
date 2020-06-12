using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AllSceneCorutine : UnitySingleton<AllSceneCorutine> {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Coroutine ExcuteTask(IEnumerator task)
    {
        return StartCoroutine(task);
    }

	public Coroutine WaitForAWhile(Action task, float seconds)
    {
        return StartCoroutine(_WaitForAWhile(task, seconds));
    }

    public void Stop(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
	private IEnumerator _WaitForAWhile(Action task, float sec)
    {
        yield return new WaitForSeconds(sec);

        if (task != null)
            task();
    }
}