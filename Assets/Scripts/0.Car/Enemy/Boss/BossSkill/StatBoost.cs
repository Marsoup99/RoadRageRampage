using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoost : BossSkill
{
    [Header("Skill setting")]
    public GameObject effectVFX;
    protected override void OnEnable()
    {
        base.OnEnable();
        effectVFX.SetActive(false);
    }
    public override void Skill()
    {
        effectVFX.SetActive(true);
        AI.weaponTimer *= 0.5f;
        AI.weapon.IncreaseNumberOfBullet();
        Activated();
    }
}
