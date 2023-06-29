using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDeSpawn : MonoBehaviour
{
    public ParticleSystem ps;
    public bool isExploVfx = false;
    public bool isCollideVfx = false;
    void Reset()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        ps.Stop();
        ps.Play();
    }

    void OnDisable()
    {
        if(isExploVfx) EffectManager.Instance.explosionVfxPool.DeSpawnObject(this.transform);
        else if(isCollideVfx) EffectManager.Instance.collideVfxPool.DeSpawnObject(this.transform);
        else 
            EffectManager.Instance.DeSpawnObject(this.transform);
    }
}
