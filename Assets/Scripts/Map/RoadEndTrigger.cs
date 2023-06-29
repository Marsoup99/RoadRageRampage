using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoadEndTrigger : MonoBehaviour
{
    public RoadStat roadStat;
    void Reset()
    {
        roadStat = GetComponentInParent<RoadStat>();
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            RoadGenerator.Instance.DeSpawnRoad();
            RoadGenerator.Instance.SpawnRoad();
            roadStat?.DeSpawnEnv();

            //Check Enemy
            EnemySpawner.Instance.Spawn();
        }
    }
}
