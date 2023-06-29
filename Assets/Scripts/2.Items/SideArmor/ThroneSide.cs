using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ThroneSide : BasicSideArmor
{
    public DamageStat dmg = new DamageStat(30, ELEMENT.normal, 0);
    public string user = "Player";
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(user)) return;
        col.GetComponentInChildren<IDmgable>()?.TakePreCalculateDamage(dmg);
    }
    public override string GetStats() 
    {
        StringBuilder sb = new StringBuilder(base.GetStats());
        sb.Append("These throne are not for decoration!" + "\n");
        return sb.ToString();
    }
}
