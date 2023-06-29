using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculate : MonoBehaviour
{
    [SerializeField] private Stat _fireDamage;
    [SerializeField] private Stat _elecDamage;
    [SerializeField] private Stat _toxicDamage;
    [SerializeField] private Stat _normalDamage;
    [SerializeField] private Stat _finalDamageMulti;

    [SerializeField] private Stat _damageToBurningEnemy;
    [SerializeField] private Stat _damageToElectrocuteEnemy;
    [SerializeField] private Stat _damageToDecayEnemy;
    
    
    void Awake()
    {
        _fireDamage = new Stat(100);
        _elecDamage = new Stat(100);
        _toxicDamage = new Stat(100);
        _normalDamage = new Stat(100);
        _finalDamageMulti = new Stat(100);

        _damageToBurningEnemy = new Stat(0);
        _damageToElectrocuteEnemy = new Stat(0);
        _damageToDecayEnemy = new Stat(0);
    }
    public float CalculateDamageDeal(DamageStat wpStat, CarCtr target)
    {
        float damageDeal = 0;
        int elementType = (int)wpStat.type;
        if(elementType == 0) damageDeal += wpStat.damage * _normalDamage.Value / 100f;
        else if(elementType == 1) damageDeal += wpStat.damage * _fireDamage.Value / 100f;
        else if(elementType == 2) damageDeal += wpStat.damage * _elecDamage.Value / 100f;
        else if(elementType == 3) damageDeal += wpStat.damage * _toxicDamage.Value / 100f;
        
        if(target.carBuffDebuff.isBurn) damageDeal *= 1 + _damageToBurningEnemy.Value / 100f;
        if(target.carBuffDebuff.isElectrocute) damageDeal *= 1 + _damageToElectrocuteEnemy.Value / 100f;
        if(target.carBuffDebuff.isDecay) damageDeal *= 1 + _damageToDecayEnemy.Value / 100f;

        if(wpStat.increaseDmgAgainstDebuff != null)
        {
            elementType = (int) wpStat.increaseDmgAgainstDebuff.type;

            if(elementType == 1 && target.carBuffDebuff.isBurn)
                damageDeal *= 1 + wpStat.increaseDmgAgainstDebuff.increasePercent /100f;

            else if(elementType == 2 && target.carBuffDebuff.isElectrocute)
                damageDeal *= 1 + wpStat.increaseDmgAgainstDebuff.increasePercent /100f;

            else if(elementType == 3 && target.carBuffDebuff.isDecay)
                damageDeal *= 1 + wpStat.increaseDmgAgainstDebuff.increasePercent /100f;
        }
        
        damageDeal *= _finalDamageMulti.Value /100f;

        // Debug.Log(damageDeal);
        return damageDeal;
    }
    public void AddModifierForElementType(StatModifier mod, ELEMENT type)
    {
        if((int)type == 0) _normalDamage.AddModifier(mod);
        else if((int)type == 1) _fireDamage.AddModifier(mod);
        else if((int)type == 2) _elecDamage.AddModifier(mod);
        else if((int)type == 3) _toxicDamage.AddModifier(mod);
    }

    public void RemoveModifierForElementType(StatModifier mod, ELEMENT type)
    {
        if((int)type == 0) _normalDamage.RemoveModifier(mod);
        else if((int)type == 1) _fireDamage.RemoveModifier(mod);
        else if((int)type == 2) _elecDamage.RemoveModifier(mod);
        else if((int)type == 3) _toxicDamage.RemoveModifier(mod);
    }

    public void AddIncreseaDamageAgainstDebuffEnemy(StatModifier mod, ELEMENT type)
    {
        if((int)type == 1) _damageToBurningEnemy.AddModifier(mod);
        else if((int)type == 2) _damageToElectrocuteEnemy.AddModifier(mod);
        else if((int)type == 3) _damageToDecayEnemy.AddModifier(mod);
        // Debug.Log(_damageToBurningEnemy.Value);
    }

    public void RemoveIncreseaDamageAgainstDebuffEnemy(StatModifier mod, ELEMENT type)
    {
        if((int)type == 1) _damageToBurningEnemy.RemoveModifier(mod);
        else if((int)type == 2) _damageToElectrocuteEnemy.RemoveModifier(mod);
        else if((int)type == 3) _damageToDecayEnemy.RemoveModifier(mod);
    }
}
