using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDistanceStateSO", menuName = "ScriptableObjects/Enemy/EnemyDistanceState")]
public class EnemyDistanceStateSO : ScriptableObject
{
    [Header("TooFarDisState")]
    [SerializeField] private int tooFarDis = 60;
    [SerializeField] private int tooFarTopSpeed = -10;
    [Header("SafeDisState")]
    [SerializeField] private int safeDis = 25;
    [SerializeField] private int safeTopSpeed = 0;
    [Header("WarningDisState")]
    [SerializeField] private int warningDis = 10;
    [SerializeField] private int warningTopSpeed = 4;
    [Header("UnsafeState")]
    [SerializeField] private int unsafeDis = -8;
    [SerializeField] private int unSafeTopSpeed = 8;
    [Header("Behind")]
    [SerializeField] private int behindTopSpeed = 18;

    public (EnemyDisState, int) GetStateTopSpeed(Vector3 pos)
    {
        float currentDis = pos.magnitude - PlayerCtrl.Instance.transform.position.magnitude;
        
        if(currentDis > tooFarDis) return (EnemyDisState.TooFar, tooFarTopSpeed);

        else if (currentDis > safeDis) return (EnemyDisState.Safe, safeTopSpeed);

        else if (currentDis > warningDis) return (EnemyDisState.Warning, warningTopSpeed);

        else if (currentDis > unsafeDis) return (EnemyDisState.UnSafe, unSafeTopSpeed); 

        else return (EnemyDisState.Behind, behindTopSpeed);
    }
}
