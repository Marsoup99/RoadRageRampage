using UnityEngine;
using System;

public class Wheels : MonoBehaviour
{
    public Transform[] wheelsTransform;
    public CarMovement carMovement;
    public float speed = 5;
    private delegate void MyDelegate();
    MyDelegate myDelegate;
    

    void Reset()
    {
        carMovement = GetComponentInParent<CarCtr>()?.carMovement;
        
    }

    void Awake()
    {
        if(carMovement == null) myDelegate = NoCarMovement;
        else myDelegate = HadCarMovement;
    }
    void LateUpdate()
    {
        myDelegate();
    }
    void NoCarMovement()
    {
        for (int i = 0; i < wheelsTransform.Length; i++)
        {
            wheelsTransform[i].Rotate(speed * 10 * Time.deltaTime, 0, 0);
        }
    }
    void HadCarMovement()
    {
        for (int i = 0; i < wheelsTransform.Length; i++)
        {
            wheelsTransform[i].Rotate(carMovement.speed * 30 * Time.deltaTime, 0, 0);
        }
    }
}
