using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsPool : ObjectPooling
{
    public static PropsPool Instance { get; private set; }
    void Awake()
    {
         // If there is an instance, and it's not this, delete itself.
        if (Instance != null && Instance != this) 
        { 
            Debug.Log("there more than one PropsPool");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public Transform[] props;

    public Transform SpawnProps(Vector3 pos)
    {
        return SpawnObject(props[Random.Range(0,props.Length)], pos, Quaternion.identity);
    }
}
