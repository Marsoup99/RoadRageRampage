using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarWeapon : MonoBehaviour
{
    [SerializeField] private CarCtr _carCtr;
    public CarCtr carCtr => _carCtr;
    public event Action<Transform> onBulletHit;
    private IWeaponize myWeapon;
    public Transform[] gunPosition;
    public GameObject defaultGun;

    void Reset()
    {
        _carCtr = transform.GetComponentInParent<CarCtr>();
        Debug.Log(this + " loaded " + _carCtr);
    }

    void OnEnable()
    {
        InputManagerSingleton.Instance.onTap += OnFireButtonDown;
        
    }

    void OnDisable()
    {
        InputManagerSingleton.Instance.onTap -= OnFireButtonDown;
    }
    public void OnBulletHit(Transform enemy)
    {
        onBulletHit?.Invoke(enemy);
    }

    public void OnFireButtonDown()
    {
        if(myWeapon == null) myWeapon = GetComponentInChildren<IWeaponize>(false);
        if(myWeapon.WeaponActivate())
            PlayerCtrl.Instance.onFireBullet?.Invoke();
    }

    public void AsignWeapon(IWeaponize wp)
    {
        myWeapon = wp;
        defaultGun.SetActive(false);
    }
    public void RemoveAsignWeapon()
    {
        myWeapon = null;
        defaultGun.SetActive(true);
        myWeapon = defaultGun.GetComponent<IWeaponize>();
    }
}   
