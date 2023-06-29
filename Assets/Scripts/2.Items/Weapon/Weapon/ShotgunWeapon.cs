using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShotgunWeapon : BasicWeaponStat
{
    [Header("Shotgun stat")]
    [SerializeField] private int _pelletsPerShot = 8;
    [SerializeField] public float range = 50f;
    [SerializeField] private float _spread = 0.1f;
    protected override void Fire()
    {   
        _timer.StartTimer();
        _bulletGO = SpawnObject(_firePoint.position, Quaternion.identity);
        for (int i = 0; i < _pelletsPerShot; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle.normalized * _spread;

            // spawn the bullet prefab from pool
            _bulletGO = SpawnObject(_firePoint.position, Quaternion.identity);

            // Set the velocity of the bullet
            // Rigidbody rb = _bulletGO.GetComponent<Rigidbody>();
            // rb.velocity = (_firePoint.position + randomDirection + Vector3.forward).normalized * speed;
            // Set the direction of the bullet
            _bulletGO.LookAt(_firePoint.position + randomDirection + Vector3.forward);
        }
    }

    public override string GetStats()
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append("Damage: " + _wpPower + "x" + _pelletsPerShot + "\n");
        if(_elementPercent > 0)
        {
            sb.Append("Element: " + type + " - " + _elementPercent + "%" + "\n");
        }
        sb.Append("Fire Rate: " + _fireRate + "\n");
        sb.Append("Bullet Speed: " + speed + " m/s\n");
        sb.Append("Range: " + range + "m" + "\n");
        
        return sb.ToString();
    }
}
