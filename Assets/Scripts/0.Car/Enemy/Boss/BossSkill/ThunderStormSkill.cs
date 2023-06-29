using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStormSkill : BossSkill
{
    [Header("Storm setting")]
    public GameObject effectVFX;
    public Transform thunderGO;
    public int strikePerTick = 2;
    public float warningTime = 0.5f; 
    public float strikeCooldown = 2;
    public Transform[] firePoints;
    public Transform[] warningGO;
    private WaitForSeconds _strikeTimer, _warningTimer;
    protected override void OnEnable()
    {
        base.OnEnable();
        effectVFX.SetActive(false);
    }

    public override void Skill()
    {
        effectVFX.SetActive(true);
        transform.parent = transform.root.parent;
        transform.position.Scale(new Vector3(0,1,1));
        _strikeTimer = new WaitForSeconds(strikeCooldown);
        _warningTimer = new WaitForSeconds(warningTime);
        StartCoroutine(ThunderStorm());
        Activated();
    }
    void Update()
    {
        transform.position = AI.transform.position.z * Vector3.forward;
    }
    private IEnumerator ThunderStorm()
    {
        yield return _strikeTimer;
        while(gameObject.activeSelf)
        {
            ShuffleFirePoint();
            for (int i = 0; i <  strikePerTick; i++)
            {
                warningGO[i].gameObject.SetActive(true);
                warningGO[i].position = firePoints[i].position;
            }
            yield return _warningTimer;
            for (int i = 0; i <  strikePerTick; i++)
            {
                warningGO[i].gameObject.SetActive(false);
                EnemyBulletPool.Instance.GetBullet(thunderGO, firePoints[i]);
            }
            yield return _strikeTimer;
        }
    }

    private void ShuffleFirePoint()
    {
        // Iterate over the array in reverse order
        for (int i = firePoints.Length - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i
            int j = Random.Range(0, i + 1);

            // Swap elements at indices i and j
            Transform temp = firePoints[i];
            firePoints[i] = firePoints[j];
            firePoints[j] = temp;
        }
    }
}
