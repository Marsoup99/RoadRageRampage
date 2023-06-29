using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BasicWeaponStat : ObjectPooling, IWeaponize, IItem
{   
    [Header("Weapon Stat")]
    [SerializeField] protected CarWeapon _carWeapon;
    [SerializeField] protected Transform _firePoint;
    [Range(0,2)]
    [SerializeField] private int _desiredPosition = 0;
    [SerializeField] protected bool _canFire = true;
    [SerializeField] protected int _wpPower = 5;
    [SerializeField] protected float _fireRate = 1;
    [SerializeField] public float speed = 100;
    [SerializeField] protected ELEMENT type = ELEMENT.normal;
    [SerializeField] protected int _elementPercent = 20;
    
    protected Transform _bulletGO;
    protected MyTimer _timer;
    private DamageStat weaponStat;

    [Header("Audio")]
    [SerializeField] private AudioClip sound;


    protected override void CreateHolder()
    {
        Transform holder = (new GameObject()).transform;
        holder.name = "[PREFAB HOLDER] " + prefab.name;
        holder.parent = transform.root.parent;
        prefabHolder = holder;
    }

    public void Equip()
    {
        if(_carWeapon == null)
            _carWeapon = GetComponentInParent<CarWeapon>();
        _carWeapon.AsignWeapon(this);

        transform.parent = _carWeapon.gunPosition[_desiredPosition]; 
        transform.localPosition = Vector3.zero;
    }

    public void Unequip()
    {
        _carWeapon.RemoveAsignWeapon();
        Destroy(prefabHolder.gameObject);
        Destroy(this.gameObject);
    }
    void Start()
    {
        _timer = new MyTimer(1f/_fireRate);
        weaponStat = new DamageStat(_wpPower, type, _elementPercent);
    }

    // Update is called once per frame
    void Update()
    {
        _timer.TimerUpdate();
    }
    public virtual bool WeaponActivate()
    {
        if(_timer.IsStop() && _canFire) 
        {
            //PlaySound
            if(sound != null)
                SoundManager.Instance?.sfxPlayer.PlayAudioAtPos(sound, transform.position, this.transform);
            Fire();
            return true;
        }
        return false;
    }

    protected virtual void Fire()
    {
        _timer.StartTimer();
        _bulletGO = SpawnObject(_firePoint.position, Quaternion.identity);
    }

    public virtual void OnHit(Transform enemy)
    {
        _carWeapon?.OnBulletHit(enemy);
    }

    public DamageStat GetDamageStat()
    {
        return weaponStat;
    }

    public virtual string GetStats()
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append("Damage: " + _wpPower + "\n");
        if(_elementPercent > 0)
        {
            sb.Append("Element: " + type + " - " + _elementPercent + "%" + "\n");
        }
        sb.Append("Fire Rate: " + _fireRate + "\n");
        sb.Append("Bullet Speed: " + speed + " m/s\n");

        return sb.ToString();
    }
}
