using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SideGun : BasicSideArmor
{
    public BasicWeaponStat gun1, gun2;
    void Reset()
    {
        gun1 = GetComponentsInChildren<BasicWeaponStat>()[0];
        gun2 = GetComponentsInChildren<BasicWeaponStat>()[1];
    }
    public override void Equip()
    {
        base.Equip();
        PlayerCtrl.Instance.onFireBullet += Fire;
    }
    public override void Unequip()
    {
        base.Unequip();
        PlayerCtrl.Instance.onFireBullet -= Fire;
    }
    
    
    private void Fire()
    {
        gun1.WeaponActivate();
        gun2.WeaponActivate();
    }

    public override string GetStats() 
    {
        StringBuilder sb = new StringBuilder(base.GetStats());
        sb.Append("Side guns." + "\n");
        return sb.ToString();
    }
}
