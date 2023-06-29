using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadStat : MonoBehaviour
{
    public int spotsLeft = 9;
    public Transform leftEnv, rightEnv;
    private List<Transform> env = new List<Transform>();
    private Transform props;
    void OnEnable()
    {
        StartCoroutine(SpawnEnv(leftEnv));
        StartCoroutine(SpawnEnv(rightEnv));
        if(EnemySpawner.Instance.currentLevelType != LEVELType.shop)
        {
            props = PropsPool.Instance.SpawnProps(transform.position + Random.Range(0, 100) * Vector3.forward);
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
        if(env.Count > 0)
            DeSpawnEnv();
    }
    public void DeSpawnEnv()
    {
        foreach(Transform trans in env)
        {
            EnvGenerator.Instance.DeSpawnObject(trans);
        }
        if(props != null)
        {
            PropsPool.Instance.DeSpawnObject(props);
            props = null;
        }
        env.Clear();
    }
    IEnumerator SpawnEnv(Transform T)
    {
        Transform tmp;
        for (int i = 0; i <= spotsLeft; i++)
        {
            float v_left = Random.value;
            if(spotsLeft - i == 2) v_left -= 0.25f;
            if(spotsLeft - i == 1) v_left -= 0.6f;
            if(v_left < 0.4)
            {
                tmp = EnvGenerator.Instance.SpawnOneSpacePrefab();
                tmp.parent = T;
                tmp.localPosition = Vector3.right * (5 + 10 * i) + Vector3.forward * 2;
            }
            else if(v_left < 0.75)
            {
                tmp = EnvGenerator.Instance.SpawnTwoSpacePrefab();
                tmp.parent = T;

                i++;
                tmp.localPosition = Vector3.right * (10 * i) + Vector3.forward * 4;
            }
            else 
            {
                tmp = EnvGenerator.Instance.SpawnThreeSpacePrefab();
                tmp.parent = T;

                i++;
                tmp.localPosition = Vector3.right * (5 + 10 * i) + Vector3.forward * 6;
                i++;
            }
            tmp.localEulerAngles = Vector3.zero;
            env.Add(tmp);
            yield return null;
        }
    }
}
