using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletRain : BossSkill
{
    [Header("Skill setting")]
    public Transform gun;
    public Transform bullet;
    public Transform firePoint;
    public float bulletSpeed;
    public Transform warningGO;
    public CarCtr target;
    public override void Skill()
    {
        AI.ChangeToStandingByState();
        AI.carCtr.carMovement.speed = 40;
        AI.carCtr.carMovement.ChangeTopSpeed(20);
        target = PlayerCtrl.Instance;
        StartCoroutine(Activating());
        warningGO.parent = transform.root.parent;
    }

    void OnDisable()
    {
       Invoke(nameof(ChangeParent), 0);
    }

    private void ChangeParent()
    {
        warningGO.SetParent(this.transform);
    }
    private IEnumerator Activating()
    {
        gun.DOLocalRotate(new Vector3(-80, 0, 0), 1);
        WaitForSeconds onesec = new WaitForSeconds(1);
        yield return onesec;

        int bullets = 5;
        for (int i = 0; i <= bullets; i++)
        {
            Transform go = EnemyBulletPool.Instance.GetBullet(bullet, firePoint);
            go.localRotation = gun.localRotation;
            yield return new WaitForSeconds(0.2f);
        }

        gun.DOLocalRotate(new Vector3(0, 0, 0), 1);
        yield return onesec;
        Activated();

        int time = 10;

        while(time > 0)
        {
            time --;
            Transform go = EnemyBulletPool.Instance.GetBullet(bullet, firePoint);
            go.position = (target.transform.position 
                            + (target.carMovement.speed + 5) * Vector3.forward
                            + Vector3.up * (bulletSpeed + 5 * Time.deltaTime));
            go.localEulerAngles = Vector3.right * 90;

            warningGO.gameObject.SetActive(true);
            warningGO.position = go.position - (go.position.y - 0.1f) * Vector3.up;
            yield return onesec;
            warningGO.gameObject.SetActive(false);
            yield return onesec;
        }
    }
}
