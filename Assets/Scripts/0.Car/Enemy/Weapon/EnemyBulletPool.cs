using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : ObjectPooling
{
    public static EnemyBulletPool Instance { get; private set; }
    private Dictionary<string, List<Transform>> bulletDic = new Dictionary<string, List<Transform>>();
    private string dicKey;
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one EnemyBulletPool");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    public Transform GetBullet(Transform bullet, Transform firepoint)
    {
        dicKey = bullet.name;
        if(!bulletDic.ContainsKey(dicKey)) 
            bulletDic.Add(dicKey, new List<Transform>());
    
        return SpawnObject(bullet, firepoint.position, Quaternion.identity);
    }

    protected override Transform GetObjectFromPool(Transform go)
    {
        foreach(Transform objTrans in bulletDic[dicKey])
        {
            if(objTrans.name == go.name)
            {
                this.bulletDic[dicKey].Remove(objTrans);
                return objTrans;
            }
        }
        Transform newGO = Instantiate(go, prefabHolder);
        newGO.name = go.name;
        return newGO;
    }
    public override void DeSpawnObject(Transform go)
    {
        go.gameObject.SetActive(false);
        bulletDic[go.name].Add(go);
    }
}
