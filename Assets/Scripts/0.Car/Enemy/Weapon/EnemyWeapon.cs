using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyWeapon : MonoBehaviour
{
    public Transform weapon;
    public Transform bullet;
    public Transform firePoint;
    [SerializeField] private int numberOfBullets = 1;
    public bool canFire = true;
    protected Vector3 _targetScale = Vector3.one * 0.7f;
    [SerializeField] protected float _animationDuration = 0.5f;
    [SerializeField] private float _betweenShootTime = 0.5f;
    [SerializeField] protected GameObject warningGO;
    protected int _numberOfBullets = 1;
    protected WaitForSeconds waitTime;
    [Header("Audio")]
    [SerializeField] private AudioClip warningSound;
    private AudioSource warningSource;
    protected virtual void Awake()
    {
        waitTime = new WaitForSeconds(_betweenShootTime);
        bullet.name = GetComponentInParent<CarCtr>().transform.name;
    }
    protected virtual void OnEnable()
    {
        _numberOfBullets = numberOfBullets;
        canFire = true;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    public virtual void IncreaseNumberOfBullet()
    {
        _numberOfBullets ++;
    }
    public virtual void RotateWeapon(bool rotateToTheBack)
    {
        if(rotateToTheBack)
        {
            if(weapon.localEulerAngles.y == 180) return;
            canFire = false;
            weapon.DOLocalRotate(Vector3.up * 180, 0.5f).OnComplete(()=> canFire = true);
        }
            
        else 
        {
            if(weapon.localEulerAngles.y == 0) return;
            canFire = false;
            weapon.DOLocalRotate(Vector3.zero, 0.5f).OnComplete(()=> canFire = true);
        }
    }

    public virtual void WeaponActivate()
    {
        if(canFire)
        {
            canFire = false;
            StartCoroutine(CoroutineFire());
        }
    }
    protected IEnumerator CoroutineFire()
    {
        Warning();

        weapon.localScale = Vector3.one;
        weapon.DOScale(_targetScale, _animationDuration)
                            .SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(_animationDuration);
    
        int nob = Random.Range(_numberOfBullets - 1, _numberOfBullets + 1);
        if(nob == 0) Fire();
        for(int i = 1; i <= nob; i++)
        {
            Fire();
            if(i < nob) 
            {
                Warning();
            }
            yield return waitTime;
        }
        canFire = true;
    }
    private void Warning()
    {
        warningGO.SetActive(true);
        //PlaySound
        warningSource = SoundManager.Instance?.sfxPlayer.PlayWarningSound(warningSound, this.transform);
    }
    private void Fire()
    {
        warningGO.SetActive(false);
        warningSource?.Stop();
        weapon.DOScale(Vector3.one, _animationDuration)
                            .SetEase(Ease.InQuad);
        
        Transform go = EnemyBulletPool.Instance.GetBullet(bullet, firePoint);
        go.localRotation = weapon.localRotation;
        
    }
}
