using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponize
{ 
    bool WeaponActivate();
    public void OnHit(Transform enemy);
    public DamageStat GetDamageStat();
}
