using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class ElementalFrontArmor : FrontArmorItems
{
    [SerializeField] private ELEMENT type;
    [SerializeField] private float _increaseDmgPercent = 50;
    public override void Equip()
    {
        base.Equip();
        PlayerCtrl.Instance.carStat.collisionDmgWPStat.SetIncreaseDmgAgainstDebuff(type, _increaseDmgPercent);
    }

    public override void Unequip()
    {
        PlayerCtrl.Instance.carStat.collisionDmgWPStat.RemoveIncreaseDmgAgainstDebuff();
        base.Unequip();
    }

    public override string GetStats()
    {
        StringBuilder sb = new StringBuilder(base.GetStats());
        sb.Append("Increase collide damage against ");
        if(type == ELEMENT.fire) sb.Append("burning enemy by ");
        else if(type == ELEMENT.electro) sb.Append("electrocuted enemy by ");
        else if(type == ELEMENT.toxic) sb.Append("decay enemy by ");

        sb.Append(_increaseDmgPercent + "%" + "\n");

        return sb.ToString();
    }
}
