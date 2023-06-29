using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BasicNitroEngine : Item
{   
    [SerializeField] private StatModifier _increaseNitroPower = new StatModifier(20, StatModType.PercentAdd);
    public override void Equip()
    {
        PlayerCtrl.Instance.carStat.nitroPowerStat.MaxValueAddPercent(_increaseNitroPower);
    }
    public override void Unequip()
    {
        PlayerCtrl.Instance.carStat.nitroPowerStat.MaxValueRemove(_increaseNitroPower);
    }
    public override string GetStats() 
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Increase nitro speed boost by " + _increaseNitroPower.Value + "% "+"\n");
        return sb.ToString();
    }
}
