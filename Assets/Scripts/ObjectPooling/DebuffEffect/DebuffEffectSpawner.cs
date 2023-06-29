using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffEffectSpawner : ObjectPooling
{   
    public static DebuffEffectSpawner Instance { get; private set; }
    [Header("Defaut debuff prefab")]
    [SerializeField] private Transform fireDebuff;
    [SerializeField] private Transform elecDebuff;
    [SerializeField] private Transform toxicDebuff;
    void Awake()
    {
         // If there is an instance, and it's not this, delete itself.
        if (Instance != null && Instance != this) 
        { 
            Debug.Log("there more than one DebuffEffectSpawner");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    public void ApllyEffect(CarCtr target, ELEMENT debuffType, int weaponDmg)
    {
        switch (debuffType)
        {
            case ELEMENT.fire:
            {
                ApplyEffect(target, fireDebuff, weaponDmg);
                break;
            }
            case ELEMENT.electro:
            {
                ApplyEffect(target, elecDebuff, weaponDmg);
                break;
            }
            case ELEMENT.toxic:
            {
                ApplyEffect(target, toxicDebuff, weaponDmg);
                break;
            }
        }
    }

    public void ApplyEffect(CarCtr target, Transform debuff, int weaponDmg)
    {
        
        Transform spawnedGO = SpawnObject(debuff, Vector3.zero, Quaternion.identity);
        spawnedGO.GetComponent<DebuffStatus>().Apply(target, weaponDmg);
    }

    public override void DeSpawnObject(Transform go)
    {
        base.DeSpawnObject(go);
        go.parent = prefabHolder;
    }
}
