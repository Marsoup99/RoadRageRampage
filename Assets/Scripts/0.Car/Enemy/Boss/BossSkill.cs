using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkill : MonoBehaviour
{
    public EnemyBossAI AI;
    public bool isActivating = false;
    public float activateThreshHold = 33;
    public float activateTimes = 2;
    private int _activatedCount;
    protected float _activateHpPercent;
    protected virtual void OnEnable()
    {
        _activateHpPercent = 100 - activateThreshHold;
        _activatedCount = 0;
    }
    public bool ActivateSkill(float percent)
    {
        if(_activatedCount < activateTimes && percent <= _activateHpPercent/100)
        {
            _activatedCount ++;
            _activateHpPercent -= activateThreshHold;
            isActivating = true;
            Skill();
            return true;
        }
        return false;
    }

    public abstract void Skill();
    
    protected void Activated()
    {
        isActivating = false;
        AI.SkillActivated();
    }
}
