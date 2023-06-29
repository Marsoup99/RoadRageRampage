using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStat : CarStat
{
    [SerializeField] private EnemySimpleAI enemyAI;
    public float deadDotweenDuration = 1f;
    protected override void Reset()
    {
        base.Reset();
        enemyAI = GetComponent<EnemySimpleAI>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SpawnOrDead(true);
        transform.localEulerAngles = Vector3.zero;
    }
    void SpawnOrDead(bool spawn)
    {
        isDead = !spawn;
        _carCtrl.carMovement.enabled = spawn;
        _carCtrl.carHitBox.gameObject.SetActive(spawn);
        _carCtrl.characterController.enabled = spawn;
        healthBarUI.gameObject.SetActive(spawn);
        enemyAI.enabled = spawn;
    }
    public override void Dead()
    {
        if(isDead) return;
        base.Dead();
        SpawnOrDead(false);
        // Make enemy jump then fall back down
        transform.DOMove(transform.position + new Vector3(Random.Range(-2f,2f), 3f, _carCtrl.carMovement.speed), 
                                            deadDotweenDuration)
                                            .SetEase(Ease.OutQuad).OnComplete(
                                                () => transform.DOMoveY(-1f, deadDotweenDuration).SetEase(Ease.InQuad)
                                            );

        // Make the enemy spin
        transform.DORotate(new Vector3(Random.Range(-180f,180f), Random.Range(-180f,180f), Random.Range(-180f,180f)), 
                                            deadDotweenDuration * 2, 
                                            RotateMode.FastBeyond360)
                                            .SetEase(Ease.InOutQuad).OnComplete(
                                                ()=>EnemySpawner.Instance.DeSpawnObject(this.transform)
                                            );
    }
    public override void TakePreCalculateDamage(DamageStat weaponStat)
    {
        float dmg = PlayerCtrl.Instance.damageCalculator.CalculateDamageDeal(weaponStat, this._carCtrl);   
        if(Random.value * 100 < weaponStat.percent)
        {
            _carCtrl.carBuffDebuff.ApplyDebuffEffect(weaponStat.type, (int)dmg);
        }
        TakeDamage(dmg, weaponStat.type);
    }
}
