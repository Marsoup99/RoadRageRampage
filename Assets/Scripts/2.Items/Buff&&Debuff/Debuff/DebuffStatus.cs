using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELEMENT{
    normal,
    fire,
    electro,
    toxic
}
public abstract class DebuffStatus : MonoBehaviour
{
    [SerializeField] protected CarCtr carCtr;
    [SerializeField] protected float _duration = 5;
    
    public virtual void Apply(CarCtr target, int weaponDmg)
    {
        carCtr = target;
        transform.parent = target.carBuffDebuff.transform;
        transform.localPosition = Vector3.zero;
    }
    protected virtual void BackToPool()
    {
        DebuffEffectSpawner.Instance.DeSpawnObject(this.transform);
    }
    public abstract void ResetDebuff();
    public abstract void StopApply();

}
