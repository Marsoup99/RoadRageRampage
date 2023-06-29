public interface IDmgable 
{
    public void TakeDamage(float damage, ELEMENT type);
    public void TakePreCalculateDamage(DamageStat weaponStat);
}
