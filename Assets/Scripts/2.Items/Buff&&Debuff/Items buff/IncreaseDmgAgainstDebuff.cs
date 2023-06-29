using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IncreaseDmgAgainstDebuff
{
    public ELEMENT type {get; private set;}
    public float increasePercent{get; private set;} = 20;
    public IncreaseDmgAgainstDebuff(ELEMENT type, float percent)
    {
        this.type = type;
        this.increasePercent = percent;
    }
}
