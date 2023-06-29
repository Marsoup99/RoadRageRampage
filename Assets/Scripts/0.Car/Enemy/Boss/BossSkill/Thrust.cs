using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : BossSkill
{
    [Header("Skill setting")]
    public GameObject effectVFX;
    protected override void OnEnable()
    {
        base.OnEnable();
        effectVFX.SetActive(false);
    }
    public override void Skill()
    {
        AI.ChangeToStandingByState();
        AI.carCtr.carMovement.speed = 40;
        AI.carCtr.carMovement.ChangeTopSpeed(60);
        StartCoroutine(Thrustting());
    }

    private IEnumerator Thrustting()
    {
        while(Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < 250)
        {
            yield return new WaitForSeconds(1);
        }
        AI.carCtr.transform.Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.5f);

    }
}
