using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroDebuff : DebuffStatus
{
    // [Range(0, 100)]
    [SerializeField] private StatModifier _nitroSlowPercent = new StatModifier(-50, StatModType.PercentAdd);

    void OnDisable()
    {
        if(carCtr != null)
            carCtr.carBuffDebuff.isElectrocute = false;
        carCtr = null;
    }

    public override void Apply(CarCtr target, int weaponDmg)
    {
        base.Apply(target, weaponDmg);
        // if(target == null) 
        // {
        //     StopApply();
        //     return;
        // }
        target.carStat.nitroPowerStat.MaxValueAddPercent(_nitroSlowPercent);
        Invoke(nameof(StopApply), _duration);
    }


    public override void ResetDebuff()
    {
        CancelInvoke();
        Invoke(nameof(StopApply), _duration);
    }

   public override void StopApply()
    {
        if(carCtr != null)
            carCtr.carStat.nitroPowerStat.MaxValueRemove(_nitroSlowPercent);
        CancelInvoke();
        BackToPool();
    }
}
