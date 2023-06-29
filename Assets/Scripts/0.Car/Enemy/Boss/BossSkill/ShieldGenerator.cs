using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : BossSkill
{
    [Header("Shield setting")]
    public int rate = 50;
    public int amount = 500;
    public GameObject effectVFX;
    private int _amountCount = 0;
    private HpStat _shield;
    private WaitForSeconds halfSec = new WaitForSeconds(0.5f);
    public bool lockFireMode = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        effectVFX.SetActive(false);
    }
    public override void Skill()
    {
        _amountCount = 0;
        _shield = AI.carCtr.carStat.shieldStat;
        StartCoroutine(RegenShield());
        if(!lockFireMode) Activated();
    }
    IEnumerator RegenShield()
    {
        effectVFX.SetActive(true);
        while(_amountCount <= amount)
        {
            _shield.Heal(rate);
            _amountCount += rate;
            yield return halfSec;
        }
        effectVFX.SetActive(false);
        Activated();
    }
}
