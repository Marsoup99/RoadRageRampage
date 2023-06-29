using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitFront {Left, Mid, Right}
public enum HitSide {Front, Between, Back}
public class PlayerCollision : CarCollision
{
    [SerializeField] private BoxCollider _myBoxCol;
    [SerializeField] private float _speedBias = 5;
    [SerializeField] private float _sideCollideDmgMulti = 0.7f;
    [SerializeField] private float _nitroCollideDmgMulti = 1.5f;
    [SerializeField] private float _selfCollideDmgMulti = 0.2f;
    private float min_x, max_x, max_z, min_z, average_x, average_z;
    private Bounds _myBounds;
    private HitFront _hitFront;
    private HitSide _hitSide;

    private string _enemyStr = "Enemy";
    protected override void Reset()
    {
        base.Reset();
        _myBoxCol = GetComponent<BoxCollider>();
    }
    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);
        if(col.collider.CompareTag(_enemyStr))
        {
            CheckHitPosition(col.collider);
            PlayerCtrl.Instance.onCollideWithEnemy?.Invoke(col.transform.GetComponentInParent<IDmgable>());
        }
    }
    void OnCollisionStay(Collision col)
    {
        // base.OnCollisionStay(col);
        if(col.collider.CompareTag(_enemyStr))
        {
            CarPushAway(col.collider);
        }
    }

    private void CheckHitPosition(Collider collider)
    {
        CarCollision other = collider.GetComponent<CarCollision>();
        if(other == null) return;
        //Check where player getting hit at.
        _hitFront = CheckHitFront(collider);
        _hitSide = CheckHitSide(collider);


        if(_hitSide == HitSide.Front | _hitSide == HitSide.Back)
        {
            CarSlam(other, _hitSide);
        }
        else
        {
            if(_hitFront == HitFront.Left)
            {
                carCtrl.carMovement.ChangeLane(LANE.right);
                other.carCtrl.carMovement.ChangeLane(LANE.left);
            }
            else if(_hitFront == HitFront.Right)
            {
                carCtrl.carMovement.ChangeLane(LANE.left);
                other.carCtrl.carMovement.ChangeLane(LANE.right);
            }
            SideSlamDmgCal(other);
        }
    }
    
    private void CarSlam(CarCollision other, HitSide hitSide)
    {
        float averageSpeed = (carCtrl.carMovement.speed + other.carCtrl.carMovement.speed) / 2;

        if(hitSide == HitSide.Front)
        {
            HeadSlamDmgCal(other);

            carCtrl.carMovement.SetSpeed(averageSpeed - _speedBias);
            other.carCtrl.carMovement.SetSpeed(averageSpeed + _speedBias);
        }
        else 
        {   
            BackSlamedDmgCal(other);

            carCtrl.carMovement.SetSpeed(averageSpeed + _speedBias);
            other.carCtrl.carMovement.SetSpeed(averageSpeed - _speedBias);
        }
    }

    private void CarPushAway(Collider collider)
    {
        CarCollision enemyCol = collider.GetComponent<CarCollision>();
        if(enemyCol == null) return;

        _hitSide = CheckHitSide(collider);

        float averageSpeed = (carCtrl.carMovement.speed + enemyCol.carCtrl.carMovement.speed) / 2;
        if(_hitSide == HitSide.Front)
        {
            carCtrl.carMovement.SetSpeed(averageSpeed - _speedBias);
            enemyCol.carCtrl.carMovement.SetSpeed(averageSpeed + _speedBias);
        }
        else 
        {
            carCtrl.carMovement.SetSpeed(averageSpeed + _speedBias);
            enemyCol.carCtrl.carMovement.SetSpeed(averageSpeed - _speedBias);
        }
    }

    public void HeadSlamDmgCal(CarCollision enemyCarCol)
    {
        if(carCtrl.carMovement.isUsingNitro) 
            // enemyCarCol.TakeCollsionDamage(carCtrl.carStat.collisionDmg * 4/3);
            DealCollideDamage(enemyCarCol, _nitroCollideDmgMulti);
        else 
        {
            DealCollideDamage(enemyCarCol, 1f);
            DealCollideDamage(this, _selfCollideDmgMulti); //Self Damaged
        }
    }

    public void BackSlamedDmgCal(CarCollision enemyCarCol)
    {
        if(carCtrl.carMovement.isUsingNitro) 
            enemyCarCol.DealCollideDamage(this, _selfCollideDmgMulti);
        else 
            enemyCarCol.DealCollideDamage(this, 1);
    }

    public void SideSlamDmgCal(CarCollision enemyCarCol)
    {   
        //Being side slamed
        if(!carCtrl.carMovement.isChangingLane)
        {
            if(carCtrl.carMovement.isUsingNitro) 
                enemyCarCol.DealCollideDamage(this, _selfCollideDmgMulti * _sideCollideDmgMulti);
            else 
                enemyCarCol.DealCollideDamage(this, _sideCollideDmgMulti);
            return;
        }

        //Going to side slam
        if(carCtrl.carMovement.isUsingNitro) 
            DealCollideDamage(enemyCarCol, _nitroCollideDmgMulti * _sideCollideDmgMulti);
        else 
        {
            DealCollideDamage(enemyCarCol, _sideCollideDmgMulti);
            DealCollideDamage(this, _selfCollideDmgMulti * _sideCollideDmgMulti);
        }
    }


    private HitFront CheckHitFront(Collider col)
    {
        _myBounds = _myBoxCol.bounds;
        Bounds col_bounds = col.bounds;

        min_x = Mathf.Max(col_bounds.min.x, _myBounds.min.x);
        max_x = Mathf.Min(col_bounds.max.x, _myBounds.max.x);
        average_x = (min_x + max_x)/ 2 - _myBounds.min.x;
        average_x /= _myBounds.size.x;
        
        // Debug.Log(myBounds.min.x);
        // Debug.Log(col_bounds.size.x);
        if (average_x > 0.7f)
            return HitFront.Right;
        else if (average_x < 0.3f)
            return HitFront.Left;
        else 
            return HitFront.Mid;
    }
    private HitSide CheckHitSide(Collider col)
    {
        _myBounds = _myBoxCol.bounds;
        Bounds col_bounds = col.bounds;

        min_z = Mathf.Max(col_bounds.min.z, _myBounds.min.z);
        max_z = Mathf.Min(col_bounds.max.z, _myBounds.max.z);
        average_z = (min_z + max_z)/ 2f - _myBounds.min.z;
        average_z /= _myBounds.size.z;

        if (average_z < 0.1f)
            return HitSide.Back;
        else if (average_z > 0.9f)
            return HitSide.Front;
        else 
            return HitSide.Between;
    }
}
