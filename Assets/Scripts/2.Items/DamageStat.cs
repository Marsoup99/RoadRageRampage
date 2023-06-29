
//Class for weapon Stat (damage, element type, element percent)
public class DamageStat
{
    public float damage {get; private set;}
    public ELEMENT type {get; private set;}
    public float percent {get; private set;}
    public IncreaseDmgAgainstDebuff increaseDmgAgainstDebuff {get; private set;}
    public DamageStat(float damage, ELEMENT type, float percent)
    {
        this.damage = damage;
        this.type = type;
        if(percent > 100) percent = 100;
        else if(percent < 0) percent = 0;
        this.percent = percent;
    }

    public DamageStat(float damage) : this(damage, ELEMENT.normal, 0) {}
    public DamageStat SetDamage(float damage)
    {
        this.damage = damage;
        return this;
    }

    public void SetIncreaseDmgAgainstDebuff(ELEMENT type, float increasePercent)
    {
        increaseDmgAgainstDebuff = new IncreaseDmgAgainstDebuff(type, increasePercent);
    }

    public void RemoveIncreaseDmgAgainstDebuff()
    {
        increaseDmgAgainstDebuff = null;
    }
}
