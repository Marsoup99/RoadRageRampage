using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FlameThrower : MonoBehaviour, IWeaponize, IItem
{
    [Header("Stat")]
    [SerializeField] protected CarWeapon _carWeapon;
    [SerializeField] private GameObject flameGO;
    [Range(0,2)]
    [SerializeField] private int _desiredPosition = 0;
    [SerializeField] private int _wpPower = 20;
    [SerializeField] private float _fireRate = 5;
    [SerializeField] private float _cooldown = 2;
    [Header("Flame Setting")]
    [SerializeField] private int _flameDuration = 2;
    [SerializeField] private float _flameRange = 5;
    [SerializeField] private int _elementPercent = 20;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] private ELEMENT type = ELEMENT.fire;
    [SerializeField] private LayerMask _hitLayer;
    private bool _isFiring = false;
    private float _tickTimer;
    private MyTimer _cooldownTimer;
    private MyTimer _durationTimer;
    private DamageStat _weaponStat;
    [SerializeField] private AudioClip sound;
    public void Equip()
    {
        if(_carWeapon == null)
            _carWeapon = GetComponentInParent<CarWeapon>();
        _carWeapon.AsignWeapon(this);

        transform.parent = _carWeapon.gunPosition[_desiredPosition]; 
        transform.localPosition = Vector3.zero;

        flameGO.transform.parent = _carWeapon.carCtr.transform;
        flameGO.SetActive(false);
        _isFiring = false;
    }

    public void Unequip()
    {
        _carWeapon.RemoveAsignWeapon();
        Destroy(this.gameObject);
    }

    void Start()
    {
        //Setting up timer for coolDown and flametick
        _cooldownTimer = new MyTimer(_cooldown);
        _durationTimer = new MyTimer(_flameDuration);
        _weaponStat = new DamageStat(_wpPower, type, _elementPercent);
    }
    public bool WeaponActivate()
    {
        if(!_cooldownTimer.IsStop() | _isFiring) return false;
        // if(_timer > 0 | _isFiring) return;

        _isFiring = true;
        _durationTimer.StartTimer();
        _tickTimer = 0;

        flameGO.SetActive(true);

        //PlaySound
        if(sound != null)
            SoundManager.Instance?.sfxPlayer.PlayAudioAtPos(sound, transform.position, this.transform);
            
        return true;
    }
    void Update()
    {
        if(!_isFiring) 
        {
            _cooldownTimer.TimerUpdate();
            return;
        }
        DealDamage();
    }
    private void DealDamage()
    {
        _durationTimer.TimerUpdate();
        _tickTimer -= Time.deltaTime;
        if(_tickTimer <= 0)
        {
            _tickTimer = 1f/_fireRate;
            RaycastHit _hit;
            if(Physics.Raycast(_firePoint.position, _carWeapon.carCtr.transform.forward, out _hit, _flameRange, _hitLayer))
            {
                //Deal Dmg;
                _hit.collider.GetComponentInParent<IDmgable>().TakePreCalculateDamage(_weaponStat);
                OnHit(_hit.transform);
            }
            
            //Check duration timer
            if(_durationTimer.IsStop()) StopFire();

            PlayerCtrl.Instance.onFireBullet?.Invoke();
        }
    }
    
    private void StopFire()
    {
        _isFiring = false;
        _cooldownTimer.StartTimer();

        flameGO.SetActive(false);
    }
    public virtual void OnHit(Transform enemy)
    {
        _carWeapon.OnBulletHit(enemy);
    }

    public DamageStat GetDamageStat()
    {
        return _weaponStat;
    }
    public virtual string GetStats()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Fire Rate: " + _fireRate + "\n");
        if(_elementPercent > 0)
        {
            sb.Append("Element: " + type + " - " + _elementPercent + "\n");
        }
        sb.Append("Damage: " + _wpPower + "\n");
        sb.Append("Flame duration: " + _flameDuration + "\n");
        sb.Append("Range: " + _flameRange + "m" + "\n");
        sb.Append("Cooldown: " + _cooldown + "\n");

        return sb.ToString();
    }
}
