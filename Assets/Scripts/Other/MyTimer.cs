using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    private float _startTime;
    private float _cooldown;
    private float _timeLeft;
    private bool _isStop = true;
    public float currentTime => _timeLeft;
    public float cooldown => _cooldown;
    
    public MyTimer(float cooldown)
    {
        this._cooldown = cooldown;
    }

    public void StartTimer()
    {
        _isStop = false;
        _startTime = Time.time;
        SetTimeLeft();
    }

    public void SetTimeLeft()
    {
        _timeLeft = _cooldown;
    }
    //Put this function in Update in order MyTimer to work
    public void TimerUpdate()
    {
        if (!_isStop)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _timeLeft = 0;
                _isStop = true;
            }
        }
    }
    public void StartWithTime(float cd)
    {
        _cooldown = cd;
        StartTimer();
    }
    public bool IsStop()
    {
        // _isStop = (_targetTime < Time.time);
        return _isStop;
    }
}
