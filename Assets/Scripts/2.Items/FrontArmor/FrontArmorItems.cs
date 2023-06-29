using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class FrontArmorItems : Item
{
    [Header("Stat")]
    [SerializeField] private StatModifier _increaseDmgModifier;
    [SerializeField] private StatModifier _increaseMaxHp = new StatModifier(100, StatModType.Flat);
    public override void Equip()
    {
        PlayerCtrl.Instance.carStat.collisionDmgStat.AddModifier(_increaseDmgModifier);
        PlayerCtrl.Instance.carStat.hpStat.MaxValueAddFlat(_increaseMaxHp);
    }

    public override void Unequip()
    {
        PlayerCtrl.Instance.carStat.collisionDmgStat.RemoveModifier(_increaseDmgModifier);
        PlayerCtrl.Instance.carStat.hpStat.MaxValueRemoveFlat(_increaseMaxHp);
        Destroy(this.gameObject);
    }
    public override string GetStats()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Increase collide damage by " + _increaseDmgModifier.Value + "% "+"\n");
        if(_increaseMaxHp.Value > 0) sb.Append("Increase max health by " + _increaseMaxHp.Value +"\n");
        return sb.ToString();
    }
}
