using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BasicSideArmor : Item
{
    [SerializeField] private StatModifier _decreaseCollideDmg = new StatModifier(-30, StatModType.PercentAdd);
    public override void Equip()
    {
        PlayerCtrl.Instance.carStat.nitroPowerStat.MaxValueAddPercent(_decreaseCollideDmg);
    }
    public override void Unequip()
    {
        PlayerCtrl.Instance.carStat.nitroPowerStat.MaxValueRemove(_decreaseCollideDmg);
    }
    public override string GetStats() 
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Decrease collide damage taken by " + -_decreaseCollideDmg.Value + "% "+"\n");
        return sb.ToString();
    }
}
