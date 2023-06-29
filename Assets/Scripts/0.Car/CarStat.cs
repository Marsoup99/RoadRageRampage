using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStat : MonoBehaviour, IDmgable
{
    [SerializeField] protected CarCtr _carCtrl;
    [SerializeField] protected UIHealthBar healthBarUI;

    [Header("Basic car stat")]
    // [SerializeField] private int _hp = 100;
    // [SerializeField] private int _electricShield;
    // [SerializeField] private int _armorShield;
    [SerializeField] private int _speedAcceleration = 10;
    // [SerializeField] private int _nitroPower = 24; 
    [SerializeField] private float _nitroDuration = 2;

    public int speedAcceleration => _speedAcceleration;
    public float nitroDuration => _nitroDuration;

    [Header("Collide damage stat")]
    [SerializeField] private int _collisionDmg = 75;

    public HpStat hpStat;
    public HpStat shieldStat;
    public HpStat armorStat;
    public SimpleStat dmgTakenMult; //Final multiply damage for hard mode or something??
    public SimpleStat nitroPowerStat;
    public Stat collisionDmgStat;
    public DamageStat collisionDmgWPStat;

    public bool isDead {get; protected set;} = false;
    protected virtual void Reset()
    {
        _carCtrl = GetComponent<CarCtr>();

        healthBarUI = GetComponentInChildren<UIHealthBar>();
    }
    protected virtual void OnEnable()
    {
        isDead = false;
        if(hpStat != null) 
        {
            hpStat.ResetValue();
            shieldStat.ResetValue();
            armorStat.ResetValue();

            dmgTakenMult.Reset();
            nitroPowerStat.Reset();
        }
        _carCtrl.carMovement.SetCarStat(speedAcceleration, nitroDuration);
        
    }
    void Awake()
    {
        //fire mean hp, electro mean shield and toxic mean armor
        hpStat.SetWeakType(ELEMENT.fire);
        shieldStat.SetWeakType(ELEMENT.electro);
        armorStat.SetWeakType(ELEMENT.toxic);

        dmgTakenMult = new SimpleStat(100);
        // nitroPowerStat = new SimpleStat(_nitroPower);

        //Collide dmg setup
        collisionDmgStat = new Stat(_collisionDmg);
        collisionDmgWPStat = new DamageStat(collisionDmgStat.Value, ELEMENT.normal, 0);
        
        //hpStat action subcribe
        healthBarUI.SetUIBars(hpStat, shieldStat, armorStat);
    }

    public virtual DamageStat GetCollisionDmgStat(float colMuiltiply)
    {
        return collisionDmgWPStat.SetDamage(collisionDmgStat.Value * colMuiltiply);
    }
    public virtual void TakePreCalculateDamage(DamageStat weaponStat)
    {
        float dmg = weaponStat.damage;
        
        if(Random.value * 100 < weaponStat.percent)
        {
            _carCtrl.carBuffDebuff.ApplyDebuffEffect(weaponStat.type, (int)dmg);
        }
        TakeDamage(dmg, weaponStat.type);
    }
    public virtual void TakeDamage(float dmg, ELEMENT type)
    {
        _carCtrl.ShakeTheCar(1);

        float finaldmg = dmg * dmgTakenMult.currentValue/100f;
        if(shieldStat.currentValue > 0) 
        {
            shieldStat.TakeDamage(finaldmg, type);
        }
        else if(armorStat.currentValue > 0)
        {
            armorStat.TakeDamage(finaldmg, type);
        } 
        else 
        {
            hpStat.TakeDamage(finaldmg, type);
            if(hpStat.currentValue <= 0)
            {
                Dead();
            }
        }
        // SetBarUI();
    }

    public virtual void Dead()
    {
        if(isDead) return;
        
        isDead = true;
        EffectManager.Instance.SpawnExplosiveEffect(this.transform.position);
        
        //Play Sound
        SoundManager.Instance?.sfxPlayer.PlayExplosiveSFX(transform.position);
    }

}
