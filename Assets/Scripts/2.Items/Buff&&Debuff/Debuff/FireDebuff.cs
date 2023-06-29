using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : DebuffStatus
{
    [SerializeField] private int _baseTickDmg;
    [Range(0, 1)]
    [SerializeField] private float _dmgPercent = 0.2f; 
    [SerializeField] private int _tickPerSec = 2;
    private float _tickCountDown;
    private MyTimer _durationTimer;

    void Awake() 
    {
        _durationTimer = new MyTimer(_duration);
    }
    void OnDisable()
    {
        if(carCtr != null)
            carCtr.carBuffDebuff.isBurn = false;
        carCtr = null;
    }
    public override void Apply(CarCtr target, int weaponDmg)
    {
        base.Apply(target, weaponDmg);
        
        _baseTickDmg =(int)(_dmgPercent * weaponDmg);

        _durationTimer.StartTimer();
        // DealDmg();
    }

    void Update()
    {
        if(_durationTimer.IsStop()) 
        {
            StopApply();
            return;
        }
        
        _durationTimer.TimerUpdate();
        _tickCountDown -= Time.deltaTime;
        if(_tickCountDown <= 0)
        {
            _tickCountDown = 1f/_tickPerSec;
            DealDmg();
        }
    }
    private void DealDmg()
    {
        // if(carCtr == null) 
        // {
        //     StopApply();
        //     return;
        // }
        carCtr?.carStat.TakeDamage(_baseTickDmg, ELEMENT.fire);
        // if(_durationTimer.IsStop())
        // {
        //     StopApply();
        //     return;
        // }
        // Invoke(nameof(DealDmg), 1f/_tickPerSec);
    }

    public override void ResetDebuff()
    {
        _durationTimer.StartTimer();
        // CancelInvoke();
        DealDmg();
    }

    public override void StopApply()
    {
        // CancelInvoke();
        BackToPool();
    }
}
