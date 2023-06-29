using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroInvicible : MonoBehaviour
{
    private CarStat _carStat;
    void OnEnable()
    {
        if(PlayerCtrl.Instance == null) return;
        PlayerCtrl.Instance.onUsingNitro += Invicible;
        _carStat = PlayerCtrl.Instance.carStat;
    }
    void OnDisable()
    {
        PlayerCtrl.Instance.onUsingNitro -= Invicible;
    }
    private void Invicible()
    {
        _carStat.dmgTakenMult.BaseValue = 0;
        _carStat.dmgTakenMult.CalculateFinalValue();
        Invoke(nameof(EndInvicible), 0.2f);
    }
    private void EndInvicible()
    {
        _carStat.dmgTakenMult.BaseValue = 100;
        _carStat.dmgTakenMult.CalculateFinalValue();
    }
}
