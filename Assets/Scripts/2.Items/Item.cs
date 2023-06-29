using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM{
    weapon = 100,
    nitroEngine = 200,
    sideArmor = 300,
    frontArmor = 400,
    relic = 500, 
}
public enum ITEMRarities{
    Common = 0,
    Rare = 1,
    Unique = 2,
    Legendary = 3,
}
public abstract class Item : MonoBehaviour, IItem
{
    public abstract void Equip();
    public abstract void Unequip();
    public virtual string GetStats() { return string.Empty;}
}
