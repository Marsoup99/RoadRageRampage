using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAI : EnemySimpleAI
{
    // public enum STATE
    // {
    //     Fight,
    //     StayFar,
    //     StayStill
    // }
    // private STATE currentState = STATE.Fight;
    [Header("State distance")]
    [SerializeField] private EnemyDistanceStateSO FightStateDis;
    [SerializeField] private EnemyDistanceStateSO SkillStateDis;
    [Header("Boss skill")]
    [SerializeField] private float _checkFeq = 10f;
    [SerializeField] private BossSkill[] bossSkills;
    private HpStat _hpStat;
    private float _checkHpPercent;
    
    [Header("Boss Music")]
    [SerializeField] private AudioClip BossMusic;

    void OnEnable()
    {
        //PlayBossMusic
        if(BossMusic != null)
            SoundManager.Instance?.bgPlayer.PlayBossMusic(BossMusic);
    }
    void OnDisable()
    {
        //Stop boss music
        SoundManager.Instance?.bgPlayer.PlayBG(GameManager.Instance.LevelManager.currentMap);
    }
    protected override void Start()
    {
        base.Start();
        enemyDistanceStateSO = FightStateDis;

        carCtr.carStat.hpStat.onValueChange += CheckHealth;
        _hpStat = carCtr.carStat.hpStat;
        _checkHpPercent = 100f - _checkFeq;
    }
    public void CheckHealth()
    {
        if((float)_hpStat.currentValue/(float)_hpStat.maxValue < _checkHpPercent / 100f)
        {
            // Debug.Log((float)_hpStat.currentValue/(float)_hpStat.maxValue);
            _checkHpPercent -= _checkFeq;
            foreach (BossSkill bs in bossSkills)
            {
                if (bs.ActivateSkill((float)_hpStat.currentValue/(float)_hpStat.maxValue))
                {
                    ActivatingSkill();
                }
            }
        }
    }
    public void ActivatingSkill()
    {
        enemyDistanceStateSO = SkillStateDis;
        weapon.canFire = false;
    }
    public void SkillActivated()
    {
        foreach(BossSkill bs in bossSkills)
        {
            if(bs.isActivating) return;
        }
        enemyDistanceStateSO = FightStateDis;
        weapon.canFire = true;

        CheckingState();
    }
}
