using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SimpleStat
{
    public float BaseValue;
    public float currentValue {get; private set;}
    private List<StatModifier> _addMods;

    public SimpleStat(float value)
    {
        _addMods = new List<StatModifier>();
        currentValue = value;
        BaseValue = value;
    }
    public void MaxValueAddPercent(StatModifier v)
    {
        _addMods.Add(v);
        CalculateFinalValue();
    }
    
    public void MaxValueRemove(StatModifier v)
    {
        _addMods.Remove(v);
        CalculateFinalValue();
    }
    public void CalculateFinalValue()
    {
        currentValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < _addMods.Count; i++)
        {
            sumPercentAdd += _addMods[i].Value;
        }
        currentValue *= 1 + sumPercentAdd / 100f;
    }

    public void Reset()
    {
        if(_addMods == null)
            _addMods = new List<StatModifier>();
        else _addMods.Clear();
        currentValue = BaseValue;
    }
}

[System.Serializable]
public class HpStat
{   
    public int BaseMaxValue;
    public int maxValue {get; private set;}
    public int currentValue {get; private set;}
    public ELEMENT weakType {get; private set;}
    public Action onValueChange;
    // private List<Action> _listenersList;
    private List<StatModifier> _addModsFlat;

    public void SetWeakType(ELEMENT type)
    {
        weakType = type;
    }
    public void ResetValue()
    {
        _addModsFlat = new List<StatModifier>();
        maxValue = BaseMaxValue;
        currentValue = BaseMaxValue;

        onValueChange?.Invoke();
    }
    public void MaxValueAddFlat(StatModifier sm)
    {
        if(sm.Type != StatModType.Flat) return;
        _addModsFlat.Add(sm);
        currentValue += (int)sm.Value;
        CalculateFinalValue();
    }
    
    public void MaxValueRemoveFlat(StatModifier sm)
    {
        _addModsFlat.Remove(sm);
        CalculateFinalValue();
    }

    private void CalculateFinalValue()
    {
        maxValue = BaseMaxValue;
        for (int i = 0; i < _addModsFlat.Count; i++)
        {
            maxValue += (int)_addModsFlat[i].Value;
        }
        if(currentValue > maxValue) currentValue = maxValue;

        onValueChange?.Invoke();
    }
    
    public void Heal(int v)
    {
        currentValue += v;
        if(currentValue > maxValue) currentValue = maxValue;

        onValueChange?.Invoke();
    }

    public void SetCurrentValue(int currentValue)
    {
        this.currentValue = currentValue;
        if(this.currentValue > maxValue) this.currentValue = maxValue;
        onValueChange?.Invoke();
    }
    public void TakeDamage(float dmg, ELEMENT type)
    {
        if (type != ELEMENT.normal)
        {
            if(this.weakType == type) dmg *= (1 + 0.25f);
            else dmg *= 0.75f;
        }
        currentValue -= (int)dmg;

        if(currentValue <= 0) currentValue = 0;
        onValueChange?.Invoke();
    }
}