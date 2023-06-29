using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Transform hitVfx;
    [SerializeField] protected BasicWeaponStat weapon;
    [SerializeField] protected float y = 0;
    [SerializeField] private AudioClip sound;
    void Reset()
    {
        weapon = GetComponentInParent<BasicWeaponStat>();
    }
    void OnDisable()
    {
        //PlaySound
        SoundManager.Instance?.sfxPlayer.PlayBulletHitSFX(sound, transform.position);
    }
    protected virtual void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player")) return;
        IDmgable target = col.GetComponentInParent<IDmgable>();
        if(target != null)
        {
            //Send weapon damage deal to target
            target.TakePreCalculateDamage(weapon.GetDamageStat());

            //Tell weapon that bullet hit what enemy.
            weapon.OnHit(col.transform);
        }
        if(hitVfx != null) EffectManager.Instance.SpawnBulletHitEffect(hitVfx, transform.position);
        else EffectManager.Instance.SpawnBulletHitEffect(weapon.GetDamageStat().type, transform.position);
        weapon.DeSpawnObject(this.transform);
    }
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * weapon.speed);
        transform.Translate(Vector3.up * y * Time.deltaTime);
    }
}
