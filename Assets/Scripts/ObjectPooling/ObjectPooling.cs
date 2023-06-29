using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] public Transform prefab;
    public List<Transform> pooledGO = new List<Transform>();
    protected Transform prefabHolder;
    protected Transform currentSelectedGO;

    protected virtual void OnEnable()
    {
        if(prefabHolder == null)
            CreateHolder();
    }
    protected virtual void CreateHolder()
    {
        Transform holder = (new GameObject()).transform;
        holder.name = "-Pool holder-";
        holder.parent = transform;
        prefabHolder = holder;
    }
    public virtual Transform SpawnObject(Vector3 pos, Quaternion quat)
    {

        Transform spawnGO = GetObjectFromPool(prefab);
        spawnGO.position = pos;
        spawnGO.rotation = quat;
        spawnGO.gameObject.SetActive(true);

        return spawnGO;
    }

    protected virtual Transform SpawnObject (Transform spawnGO, Vector3 pos, Quaternion quat)
    {
        currentSelectedGO = spawnGO;

        Transform spawn = GetObjectFromPool(this.currentSelectedGO);
        spawn.position = pos;
        spawn.rotation = quat;
        spawn.gameObject.SetActive(true);

        return spawn;

    }
    protected virtual Transform GetObjectFromPool(Transform go)
    {
        // foreach(Transform objTrans in pooledGO)
        // {
        //     if(objTrans.name == go.name)
        //     {
        //         this.pooledGO.Remove(objTrans);
        //         return objTrans;
        //     }
        // }
        foreach(Transform objTrans in pooledGO)
        {
            if(!objTrans.gameObject.activeSelf)
            {
                if(objTrans.name == go.name)
                {
                    return objTrans;
                }
            }
        }
        Transform newGO = Instantiate(go, prefabHolder);
        newGO.name = go.name;

        pooledGO.Add(newGO);
        return newGO;
    }

    public virtual void DeSpawnObject(Transform go)
    {
        go.gameObject.SetActive(false);
        // pooledGO.Add(go);
    }
}
