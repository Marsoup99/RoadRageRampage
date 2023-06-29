using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[System.Serializable]
public class Stat
{
		public float BaseValue;
        private bool isDirty = false;
		private float _value;
        public float lastValue;
		public float Value {
			get {
                if(!isDirty) return lastValue; 
                else 
                {
                    isDirty = true;
                    CalculateFinalValue();
                    isDirty = false;
				    return _value;
                }
			}
		}

		private List<StatModifier> modsFlat;
        private List<StatModifier> modsPercentAdd;
        private List<StatModifier> modsPercentMult;

		public Stat(float baseValue)
		{
            modsFlat = new List<StatModifier>();
            modsPercentAdd = new List<StatModifier>();
            modsPercentMult = new List<StatModifier>();
			BaseValue = baseValue;
            lastValue = _value = baseValue;
		}

		public virtual void AddModifier(StatModifier mod)
		{
            isDirty = true;
			if(mod.Type == StatModType.Flat) modsFlat.Add(mod);
            else if(mod.Type == StatModType.PercentAdd) modsPercentAdd.Add(mod);
            else if(mod.Type == StatModType.PercentMult) modsPercentMult.Add(mod);
            CalculateFinalValue();
		}

		public virtual void RemoveModifier(StatModifier mod)
		{
            isDirty = true;
			if(mod.Type == StatModType.Flat) modsFlat.Remove(mod);
            else if(mod.Type == StatModType.PercentAdd) modsPercentAdd.Remove(mod);
            else if(mod.Type == StatModType.PercentMult) modsPercentMult.Remove(mod);
            CalculateFinalValue();
		}

		public virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;
            
            for (int i = 0; i < modsFlat.Count; i++)
            {
                finalValue += modsFlat[i].Value;
            }
            for (int i = 0; i < modsPercentAdd.Count; i++)
            {
                sumPercentAdd += modsPercentAdd[i].Value;
            }

            finalValue *= 1 + sumPercentAdd / 100f;

            for (int i = 0; i < modsPercentMult.Count; i++)
            {
                finalValue *= modsPercentMult[i].Value;
            }
            lastValue = finalValue;
            _value = finalValue;
            return finalValue;
        }
	}
