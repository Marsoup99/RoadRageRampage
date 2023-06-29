using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class DeathTimerUI : MonoBehaviour
{
    public MyTimer target;
    public TextMeshProUGUI cooldownText;
    private float _lastValue;
    private float _fadingTime = 2f;
    private bool isFadeIn = true;
    private Tweener _tween;
    public void SetTargetTimer(MyTimer target)
    {
        this.target = target;
        FadeOut();
    }
    void LateUpdate()
    {
        cooldownText.text = target.currentTime.ToString("0.00");
        if(_lastValue == target.currentTime)
        {
            if(isFadeIn) FadeOut();
        } 
        else 
        {
            if(!isFadeIn) FadeIn();
        }
        _lastValue = target.currentTime;
    }
    private void FadeIn()
    {
        isFadeIn = true;
        if(_tween != null)_tween.Kill();
        transform.DOScale(Vector3.one, _fadingTime/10);
    }

    private void FadeOut()
    {
        isFadeIn = false;
        _tween = transform.DOScale(Vector3.zero, _fadingTime);
    }
}
