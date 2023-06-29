using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class NitroShield : BasicNitroEngine
{
    [SerializeField] private int _shieldAmountPercent = 20; 
    // [SerializeField] private int _electroDebuffPercent = 30;
    [SerializeField] private DamageStat damageStat = new DamageStat(0, ELEMENT.electro, 30);
    public GameObject shield;
    private StatModifier statModifier = new StatModifier(0, StatModType.Flat);
    // private DamageStat damageStat;

    public override void Equip()
    {
        base.Equip();
        PlayerCtrl.Instance.onCollideWithEnemy += EmptyShieldToDealDmg;
        PlayerCtrl.Instance.onUsingNitro += NitroBoostShield;
        shield = transform.GetChild(0).gameObject;
    }

    public override void Unequip()
    {
        PlayerCtrl.Instance.onCollideWithEnemy -= EmptyShieldToDealDmg;
        PlayerCtrl.Instance.onUsingNitro -= NitroBoostShield;
        base.Unequip();
    }
    void EmptyShieldToDealDmg(IDmgable target)
    {
        if(PlayerCtrl.Instance.carMovement.isUsingNitro)
        {
            int dmg = PlayerCtrl.Instance.carStat.shieldStat.currentValue;
            if(dmg <= 0) return; 

            PlayerCtrl.Instance.carStat.TakeDamage(dmg, ELEMENT.electro);
            
            target?.TakePreCalculateDamage(damageStat.SetDamage(dmg));
            shield.SetActive(false);
        }
    }

    void NitroBoostShield()
    {
        statModifier.ChangeValue(PlayerCtrl.Instance.carStat.hpStat.maxValue * _shieldAmountPercent/100f / 3f);
        PlayerCtrl.Instance.carStat.shieldStat.MaxValueAddFlat(statModifier);

        shield.SetActive(true);
        Invoke(nameof(RemoveBonusShield), PlayerCtrl.Instance.carStat.nitroDuration);
    }
    void RemoveBonusShield()
    {
        PlayerCtrl.Instance.carStat.shieldStat.MaxValueRemoveFlat(statModifier);
        shield.SetActive(false);
    }
    public override string GetStats()
    {
        StringBuilder sb = new StringBuilder(base.GetStats());
        sb.Append("Using nitro will create a shield equals to " + _shieldAmountPercent + "% " + "max health." + "\n");
        sb.Append("Deal collide damage(nitro) will convert all shield to deal electro damage." + "\n");
        return sb.ToString();
    }
}
