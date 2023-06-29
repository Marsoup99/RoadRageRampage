using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarCollision : MonoBehaviour
{
    [SerializeField] private LayerMask sideWallLayerMask = 1<<6;
    [SerializeField] public CarCtr carCtrl;
    [SerializeField] public SimpleStat CollideDmgTakenMulti = new SimpleStat(100);
    
    
    protected virtual void Reset()
    {
        carCtrl = transform.parent.GetComponent<CarCtr>();
        Debug.Log(this + " loaded " + carCtrl);
    }
    protected virtual void OnCollisionEnter(Collision col)
    {
        EffectManager.Instance.SpawnCollideEffect(col.contacts[0].point, Quaternion.FromToRotation(Vector3.up, transform.forward - col.contacts[0].point));
        
        if(col.transform.CompareTag("SideWall"))
        {
            carCtrl.carStat.TakeDamage((int)(carCtrl.carMovement.speed * 0.5f), ELEMENT.normal);

            if(Physics.Raycast(transform.position + Vector3.up, transform.right, 3f, sideWallLayerMask))
            {
                carCtrl.carMovement.ChangeLane(LANE.left);
            }
            else carCtrl.carMovement.ChangeLane(LANE.right);

            carCtrl.ShakeTheCar(0.5f);

            //PlaySound
            SoundManager.Instance?.sfxPlayer.PlayCollideSFX(transform.position);
        }
        if(col.transform.CompareTag("Slope"))
        {
            carCtrl.carMovement.OnSlope(Vector3.up);
        }
    }

    protected virtual void OnCollisionExit(Collision col)
    {
        if(col.transform.CompareTag("Slope"))
        {
            carCtrl.carMovement.OnSlope(Vector3.zero);
        }
    }

    private void TakeCollideDamage(DamageStat dmg)
    {
        carCtrl.carStat.TakePreCalculateDamage(dmg.SetDamage(dmg.damage * CollideDmgTakenMulti.currentValue / 100f));
    }

    public void DealCollideDamage(CarCollision target, float colMuiltiply)
    {
        target.TakeCollideDamage(carCtrl.carStat.GetCollisionDmgStat(colMuiltiply));

        //PlaySound
        SoundManager.Instance?.sfxPlayer.PlayCollideSFX(transform.position);
    }
    
}
