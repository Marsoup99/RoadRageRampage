using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : ObjectPooling
{
    public static EffectManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one EffectManager");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    [SerializeField] private Transform[] bulletsHitVfx;
    [SerializeField] public ObjectPooling explosionVfxPool;
    [SerializeField] public ObjectPooling collideVfxPool;

    void Reset()
    {
        explosionVfxPool = transform.Find("ExplosivePool").GetComponent<ObjectPooling>();
        collideVfxPool = transform.Find("CollidePool").GetComponent<ObjectPooling>();
    }

    public void SpawnBulletHitEffect(Transform hitVfx, Vector3 position)
    {
        SpawnObject(hitVfx, position, Quaternion.identity);
    }
    public void SpawnBulletHitEffect(ELEMENT type, Vector3 position)
    {
        SpawnObject(bulletsHitVfx[(int)type], position, Quaternion.identity);
    }


    public void SpawnExplosiveEffect(Vector3 position)
    {
        explosionVfxPool.SpawnObject(position, Quaternion.identity);
    }

    public void SpawnCollideEffect(Vector3 position, Quaternion quat)
    {
        collideVfxPool.SpawnObject(position, quat);
    }
}
