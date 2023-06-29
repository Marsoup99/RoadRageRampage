using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyDoubleWeapon : EnemyWeapon
{
    [Header("Second weapon")]
    public Transform weapon_2;
    public Transform bullet_2;
    public Transform firePoint_2;
    [SerializeField] private int numberOfBullets_2 = 1;
    [SerializeField] private GameObject warningGO_2;
    [SerializeField] private AudioClip WarningSound_2;
    private AudioSource warningSource_2;
    private int _numberOfBullets_2;
    protected override void Awake()
    {
        base.Awake();
        bullet_2.name = GetComponentInParent<CarCtr>().transform.name + "Second";
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _numberOfBullets_2 = numberOfBullets_2;
    }

    public override void IncreaseNumberOfBullet()
    {
        base.IncreaseNumberOfBullet();
        _numberOfBullets_2++;
    }
    public override void RotateWeapon(bool rotateToTheBack)
    {
        base.RotateWeapon(rotateToTheBack);
        if(rotateToTheBack)
        {
            if(weapon_2.localEulerAngles.y == 180) return;
            weapon_2.DOLocalRotate(Vector3.up * 180, 0.5f);
        }
            
        else 
        {
            if(weapon_2.localEulerAngles.y == 0) return;
            weapon_2.DOLocalRotate(Vector3.zero, 0.5f);
        }
    }

    public override void WeaponActivate()
    {
        if(canFire)
        {
            canFire = false;
            if(UnityEngine.Random.value <= 0.5f) StartCoroutine(CoroutineFire());
            else StartCoroutine(CoroutineFire_2());
        }
    }
    protected IEnumerator CoroutineFire_2()
    {
        Warning2();
        weapon_2.localScale = Vector3.one;
        weapon_2.DOScale(_targetScale, _animationDuration)
                            .SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(_animationDuration);

        int nob = Random.Range(_numberOfBullets_2 - 1, _numberOfBullets_2 + 1);
        if(nob == 0) Fire_2();
        for(int i = 1; i <= nob; i++)
        {
            Fire_2();
            if(i < nob) Warning2();
            yield return waitTime;
        }
        canFire = true;
    }

    private void Warning2()
    {
        warningGO_2.SetActive(true);
        warningSource_2 = SoundManager.Instance?.sfxPlayer.PlayWarningSound(WarningSound_2, transform);
    }
    void Fire_2()
    {
        warningGO_2.SetActive(false);
        warningSource_2?.Stop();
        weapon_2.DOScale(Vector3.one, _animationDuration)
                            .SetEase(Ease.InQuad);
        
        Transform go = EnemyBulletPool.Instance.GetBullet(bullet_2, firePoint_2);
        go.localRotation = weapon_2.localRotation;
    }
}
