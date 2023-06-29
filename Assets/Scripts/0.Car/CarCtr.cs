using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarCtr : MonoBehaviour
{
    public Transform carModel;
    public CharacterController characterController;
    public CarCollision carHitBox;
    public CarMovement carMovement;
    
    public CarStat carStat; //TODO: change carStat to scripable object maybe?
    public DebuffManager carBuffDebuff;

    [Header("Shake")]
    private float shakeDuration = 0.2f;
    private float shakeStrength = 0.15f;
    private Vector3 _modelLocalPosition;
    protected virtual void Reset()
    {
        carModel = transform.Find("Model");
        Debug.Log(this + " loaded " + carModel);

        characterController = GetComponent<CharacterController>();
        Debug.Log(this + " loaded " + characterController);

        carHitBox = GetComponentInChildren<CarCollision>();
        Debug.Log(this + " loaded " + carHitBox);

        carMovement = GetComponentInChildren<CarMovement>();
        Debug.Log(this + " loaded " + carMovement);

        carStat = GetComponent<CarStat>();
        Debug.Log(this + " loaded " + carStat);

        carBuffDebuff = GetComponentInChildren<DebuffManager>();
        Debug.Log(this + " loaded " + carBuffDebuff);
    }

    void Awake()
    {
        _modelLocalPosition = carModel.transform.localPosition;
    }
    public virtual void ShakeTheCar(float shakePower)
    {
        // Shake the car using DOTween
        carModel.DOKill(false);
        carModel.transform.localPosition = _modelLocalPosition;
        carModel.DOShakePosition(shakeDuration * shakePower, shakeStrength * shakePower).SetEase(Ease.OutElastic);
    }
}
