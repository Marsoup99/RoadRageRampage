using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike : EnemyBulletMovement
{
    [Header("Strike setting")]
    public ParticleSystem ps;
    public float thunderTick = 0.5f;
    public LayerMask playerLayer;
    private float _timer = 0;
    private Vector3 boxSize = Vector3.one;
    [SerializeField] private AudioClip thunderSfx;

    protected override void OnEnable()
    {
        base.OnEnable();
        _timer = thunderTick;
        boxSize.y = transform.position.y;
    }
    void FixedUpdate()
    {
        if(_timer >= thunderTick)
        {
            Strike();
            _timer = 0;
            return;
        }
        _timer += Time.fixedDeltaTime;
    }

    private void Strike()
    {
        ps.Play();
        
        if(Physics.BoxCast(transform.position, boxSize / 2f, Vector3.down, Quaternion.identity, boxSize.y, playerLayer))
        {
            PlayerCtrl.Instance.carStat.TakePreCalculateDamage(damageStat);
        }

        //Play SOund
        SoundManager.Instance?.sfxPlayer.PlayAudioAtPos(thunderSfx, transform.position);
    }
}
