

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/**
* TextTimer 文本计时器，加在Label上
*/
public class TextTimer: MonoBehaviour
{

    Text _label = null;

    long _target = 0;

    long _delta = 0;

    UnityAction _callback = null;

    bool _timing = false;

    bool _real = false;

    int _timeScale = 1;

    void Awake() {
        _label = transform.GetComponent<Text>();
    }

    public void setCallback(UnityAction callback) {
        _callback = callback;
    }

    public void setTimeScale(int timeScale) {
        _timeScale = timeScale;
    }

    public void setReal(bool real)
    {
        _real = real;
    }

    /**
     * 开始计时
     * @param {*} targetTime 目标时间戳
     */
    public void startTiming(long targetTimeTicks)
    {
        _target = (long)(targetTimeTicks * Math.Pow(10, 7));
        _timing = true;
        refresh();
    }
    
    public void startTimingBySeconds(int IntervalSeconds)
    {
        _target = (long)(DateTime.Now.Ticks + IntervalSeconds * Math.Pow(10, 7));
        _timing = true;
        refresh();
    }

    /**
     * 停止计时
     */
    public void stopTiming() {
        _timing = false;
    }

    /**
    * 停止计时
    */
    public void startTiming()
    {
        _timing = true;
    }
    /**
     * 仅设置时间，不开始计时
     * @param {*} targetTime 目标时间戳
     */
    public void setTime(long targetTimeTicks)
    {
        _target = (long)(targetTimeTicks * Math.Pow(10, 7));
        refresh();
    }

    public void setTimeBySeconds(int IntervalSeconds)
    {
        _target = (long)(DateTime.Now.Ticks + IntervalSeconds * Math.Pow(10, 7));
        refresh();
    }

    public float getTime() {
        return _delta / 1000;
    }

    void Update() {
        int dt = (int)(Time.deltaTime * Math.Pow(10,7));
        dt *= _timeScale;
        if (_timing && _delta > 0)
        {
            _updateLabel();
            if (_real)
            {
                _delta = _target - DateTime.Now.Ticks;
            }
            else
            {
                _delta -= dt;

            }
            if (_delta <= 0 && _callback != null)
            {
                _timing = false;
                _callback();
            }
        }
    }

    void _updateLabel()
    {
        TimeSpan ts = new TimeSpan(_delta);
        _label.text = string.Format("{0}:{1}",ts.TotalMinutes.ToString("00"),ts.TotalSeconds.ToString("00"));
    }

    void refresh() {
        _delta = _target - DateTime.Now.Ticks;
        _updateLabel();
    }

}
