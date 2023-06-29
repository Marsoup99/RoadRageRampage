using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfCollision : CarCollision
{
    [SerializeField] private float _speedBias = 5;
    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        if(col.collider.CompareTag("Enemy"))
        {
            CarCollision other = col.collider.GetComponent<CarCollision>();
            if(other == null) return;
            CarPushSideWay(other);
            CarPushAway(other);
        }
    }
    // void OnCollisionStay(Collision col)
    // {
    //     // base.OnCollisionStay(col);
    //     if(col.collider.CompareTag("Enemy"))
    //     {
    //         CarCollision other = col.collider.GetComponent<CarCollision>();
    //     }
    // }

    private void CarPushAway(CarCollision other)
    {
        if(other.transform.position.z > transform.position.z)
        {
            carCtrl.carMovement.SetSpeed(other.carCtrl.carMovement.speed - _speedBias);
            
        }
        else 
        {
            carCtrl.carMovement.SetSpeed(carCtrl.carMovement.speed + _speedBias);
        }
    }

    private void CarPushSideWay(CarCollision other)
    {
        if(carCtrl.carMovement.isChangingLane)
        {
            if(carCtrl.carMovement.lastInput == LANE.left)
            {
                carCtrl.carMovement.ChangeLane(LANE.right);
                if(!other.carCtrl.carMovement.isChangingLane)
                    other.carCtrl.carMovement.ChangeLane(LANE.left);
            }
            else
            {   
                carCtrl.carMovement.ChangeLane(LANE.left);
                if(!other.carCtrl.carMovement.isChangingLane)
                    other.carCtrl.carMovement.ChangeLane(LANE.right);
            } 
        }
    }
}
