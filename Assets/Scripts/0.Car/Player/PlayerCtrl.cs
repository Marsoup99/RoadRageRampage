using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCtrl : CarCtr
{
    public static PlayerCtrl Instance { get; private set; }
    public DamageCalculate damageCalculator;
    public PlayerItems playerItems;
    public CarWeapon carWeapon;
    //Events
    public Action<IDmgable> onCollideWithEnemy;
    public Action onUsingNitro;
    public Action onFireBullet;

    protected override void Reset()
    {
        base.Reset();
        damageCalculator = GetComponentInChildren<DamageCalculate>();
        Debug.Log(this + " loaded " + damageCalculator);

        playerItems = GetComponentInChildren<PlayerItems>();
        Debug.Log(this + " loaded " + playerItems);

        carWeapon = GetComponentInChildren<CarWeapon>();
        Debug.Log(this + " loaded " + carWeapon);
    }
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one PlayerCtrl");
            // Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void ResetPlayerPosition()
    {
        carMovement.currentLaneSide = 1;
        carMovement.ChangeLane(LANE.left);
        transform.position = Vector3.zero;
    }
    // public override void ShakeTheCar(float shakePower)
    // {
    //     base.ShakeTheCar(shakePower);
    //     CameraControl.Instance.CameraShake();
    // }

}
