using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class StatusRelicItems : Item
{
    [SerializeField] private ELEMENT type;
    [SerializeField] private StatModifier _modifier;
    [SerializeField] private int _relicTier = 1;
    private float _baseValue;
    void Awake()
    {
        _baseValue = _modifier.Value;
    }
    public void SetRelicTier(int tier)
    {
        _relicTier = tier;
        _modifier.ChangeValue(_relicTier * _baseValue);
    }
    public override void Equip()
    {
        PlayerCtrl.Instance.damageCalculator.AddIncreseaDamageAgainstDebuffEnemy(_modifier, type);
    }

    public override void Unequip()
    {
        PlayerCtrl.Instance.damageCalculator.RemoveIncreseaDamageAgainstDebuffEnemy(_modifier, type);
        Destroy(this.gameObject);
    }

    public override string GetStats()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Deal " + _modifier.Value + "% " + "more damage to ");
        if(type == ELEMENT.fire) sb.Append("burning enemy.");
        else if(type == ELEMENT.electro) sb.Append("electrocuted enemy.");
        else if(type == ELEMENT.toxic) sb.Append("decay enemy.");
        sb.Append("\n");
        return sb.ToString();
    }
}

