using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnObjByDis : MonoBehaviour
{
    [SerializeField] private ObjectPooling pool;
    [SerializeField] private float deSpawnDistance = 50;
    private Vector3 _startPos;

    void Reset()
    {
        pool = GetComponentInParent<ObjectPooling>();
        Debug.Log(this + " loaded " + pool);
    }
    void OnEnable()
    {
        // Invoke("DeSpawn", deSpawnTimer);
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(_startPos, transform.position) > deSpawnDistance)
        {
            DeSpawn();
        }
    }
    void DeSpawn()
    {
        pool.DeSpawnObject(this.transform.parent);
    }
}
