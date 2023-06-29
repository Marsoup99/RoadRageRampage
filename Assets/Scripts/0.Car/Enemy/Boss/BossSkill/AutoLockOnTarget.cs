using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLockOnTarget : BossSkill
{   
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform target;
    public override void Skill()
    {
        target = PlayerCtrl.Instance.transform;
        Activated();
    }

    void LateUpdate()
    {
        if(target != null)
        {
            weapon.LookAt(target);
        }
    }
}
