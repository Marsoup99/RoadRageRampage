using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class ElementRelicItems : Item
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
        PlayerCtrl.Instance.damageCalculator.AddModifierForElementType(_modifier, type);
    }

    public override void Unequip()
    {
        PlayerCtrl.Instance.damageCalculator.RemoveModifierForElementType(_modifier, type);
        Destroy(this.gameObject);
    }
    public override string GetStats()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Increase " + _modifier.Value + "% ");
        if(type == ELEMENT.fire) sb.Append("fire damage.");
        else if(type == ELEMENT.electro) sb.Append("electro damage.");
        else if(type == ELEMENT.toxic) sb.Append("toxic damage.");
        sb.Append("\n");
        return sb.ToString();
    }
}
