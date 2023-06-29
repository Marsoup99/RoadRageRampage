using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    public Transform hitVfx;
    [SerializeField] private float _dmg = 70;
    [SerializeField] private ELEMENT _type = ELEMENT.normal;
    public float speed = 80;
    public float y = 2f;
    public float deSpawnTime = 2f;
    private float _dstime;
    protected DamageStat damageStat;
    private Vector3 _localForward;
    [SerializeField] private AudioClip sound;

    void OnDisable()
    {
        if(hitVfx != null) EffectManager.Instance.SpawnBulletHitEffect(hitVfx, transform.position);
        else EffectManager.Instance.SpawnBulletHitEffect(damageStat.type, transform.position);

        //PlaySound
        SoundManager.Instance?.sfxPlayer.PlayBulletHitSFX(sound, transform.position);
        
    }
    void Awake()
    {
        damageStat = new DamageStat(_dmg, _type, 30);
    }
    protected virtual void OnEnable()
    {
        _dstime = 0;
        _localForward = transform.TransformDirection(Vector3.forward);
    }
    void Update()
    {
        transform.Translate(_localForward * Time.deltaTime * speed);
        transform.Translate(transform.up * -y * Time.deltaTime);
        if(_dstime >= deSpawnTime)
        {
            EnemyBulletPool.Instance.DeSpawnObject(this.transform);
            return;
        }
        _dstime += Time.deltaTime;

    }
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(transform.tag)) return;
        IDmgable target = col.GetComponentInParent<IDmgable>();
        if(target != null)
        {
            //Send weapon damage deal to target
            target.TakePreCalculateDamage(damageStat);
        }
        EnemyBulletPool.Instance.DeSpawnObject(this.transform);
    }
}
