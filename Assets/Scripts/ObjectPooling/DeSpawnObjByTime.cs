using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnObjByTime : MonoBehaviour
{
    [SerializeField] private ObjectPooling pool;
    [SerializeField] private float deSpawnTimer = 1;
    private float _timer = 0;

    void Reset()
    {
        pool = GetComponentInParent<ObjectPooling>();
        Debug.Log(this + " loaded " + pool);
    }
    void OnEnable()
    {
        // Invoke("DeSpawn", deSpawnTimer);
        _timer = deSpawnTimer;
    }

    void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;
        if(_timer < 0) DeSpawn(); 
    }
    // Update is called once per frame
    void DeSpawn()
    {
        pool.DeSpawnObject(this.transform.parent);
    }
}
