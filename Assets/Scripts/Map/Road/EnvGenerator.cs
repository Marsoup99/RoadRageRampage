using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvGenerator : ObjectPooling
{
    public static EnvGenerator Instance { get; private set; }
    [SerializeField] private Transform[] _oneSpacePrefab;
    [SerializeField] private Transform[] _twoSpacePrefab;
    [SerializeField] private Transform[] _threeSpacePrefab;
    void Awake()
    {
         // If there is an instance, and it's not this, delete itself.
        if (Instance != null && Instance != this) 
        { 
            Debug.Log("there more than one EnvGenerator");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public Transform SpawnOneSpacePrefab()
    {
        return SpawnObject(_oneSpacePrefab[Random.Range(0,_oneSpacePrefab.Length)], Vector3.zero, Quaternion.identity);
    }
    public Transform SpawnTwoSpacePrefab()
    {
        return SpawnObject(_twoSpacePrefab[Random.Range(0,_twoSpacePrefab.Length)], Vector3.zero, Quaternion.identity);
    }
    public Transform SpawnThreeSpacePrefab()
    {
        return SpawnObject(_threeSpacePrefab[Random.Range(0,_threeSpacePrefab.Length)], Vector3.zero, Quaternion.identity);
    }

    public override void DeSpawnObject(Transform go)
    {
        base.DeSpawnObject(go);
        go.parent = prefabHolder;
    }
}
