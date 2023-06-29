using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicDebuff : DebuffStatus
{
    // [Range(0, 100)]
    // [SerializeField] private int _dmgMultiplyPercent = 10; 
    [SerializeField] private StatModifier _dmgMultiplyPercent = new StatModifier(10, StatModType.PercentAdd);

    void OnDisable()
    {
        if(carCtr != null)
            carCtr.carBuffDebuff.isDecay = false;
        carCtr = null;
    }

    public override void Apply(CarCtr target, int weaponDmg)
    {
        base.Apply(target, weaponDmg);
        // if(_targetCarCtrl == null) 
        // {
        //     StopApply();
        //     return;
        // }
        
        carCtr.carStat.dmgTakenMult.MaxValueAddPercent(_dmgMultiplyPercent);
        Invoke(nameof(StopApply), _duration);
    }


    public override void ResetDebuff()
    {
        CancelInvoke();
        Invoke(nameof(StopApply), _duration);
    }

   public override void StopApply()
    {
        carCtr?.carStat.dmgTakenMult.MaxValueRemove(_dmgMultiplyPercent);
        CancelInvoke();
        BackToPool();
    }
}
