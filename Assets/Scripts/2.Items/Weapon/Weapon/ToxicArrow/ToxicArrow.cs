using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArrow : BasicWeaponStat
{
    [SerializeField] Animator ani;
    [SerializeField] GameObject ArrowPrefab;

    protected override void Fire()
    {
        base.Fire();
        ani.Play("Bow");
        ArrowPrefab.SetActive(false);
    }
    public void ReloadArrow()
    {
        ArrowPrefab.SetActive(true);
    }
}
