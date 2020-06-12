using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于给uibase 添加monobehavior的事件相应
public class BehaviourDelegate : MonoBehaviour {
    //uIBase != null && uIBase.uiName == "UpgradeMatchStartingPanel"
	public UIBase uIBase;
	// delegate void BaseActionDelegate();
    Action _OnAwake;
	Action _OnStart;
	Action _OnUpdate;
	Action _OnEnable;
	Action _OnDisable;
	Action _OnDestroy;

    void Awake () {
		if (_OnAwake != null)
			_OnAwake();
	}

	// Use this for initialization
	void Start () {
		if (_OnStart != null)
			_OnStart();
	}

	// Update is called once per frame
	void Update () {
		if (_OnUpdate != null)
			_OnUpdate();
	}

	void OnEnable () {
		if (_OnEnable != null)
			_OnEnable();
	}

	void OnDisable () {
		if (_OnDisable != null)
			_OnDisable();
	}

	void OnDestroy () {
		if (_OnDestroy != null)
			_OnDestroy();
	}

    public void RegisterAwake (Action action) {
		_OnAwake = action;
	}
	public void RegisterStart (Action action) {
		_OnStart = action;
	}

	public void RegisterOnDestory (Action action) {
		_OnDestroy = action;
	}

	public void RegisterOnDisable (Action action) {
		_OnDisable = action;
	}

	public void RegisterOnEnable (Action action) {
		_OnEnable = action;
	}

	public void RegisterOnUpdate (Action action) {
		_OnUpdate = action;
	}

    public void UnregisterAwake()
    {
        _OnAwake = null;
    }
    public void UnregisterStartDelegate()
    {
        _OnStart = null;
    }

    public void UnregisterDestroyDelegate()
    {
        _OnDestroy = null;
    }

    public void UnregisterEnableDelegate()
    {
        _OnEnable = null;
    }

    public void UnregisterDisableDelegate()
    {
        _OnDisable = null;
    }

    public void UnregisterUpdate()
    {
        _OnUpdate = null;
    }

}
