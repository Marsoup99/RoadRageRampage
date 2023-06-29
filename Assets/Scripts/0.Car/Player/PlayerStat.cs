using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerStat : CarStat
{
    public float deadDotweenDuration = 1;

    protected override void OnEnable()
    {
        base.OnEnable();
        InputManagerSingleton.Instance.isRecevingInput = true;
        _carCtrl.carMovement.enabled = true;
    }
    public override void Dead()
    {
        if(isDead) return;
        DeadAnimation();
        _carCtrl.carMovement.speed = 0;
        _carCtrl.carMovement.enabled = false;
        base.Dead();
        InputManagerSingleton.Instance.isRecevingInput = false;
    }

    private void DeadAnimation()
    {
        // Make enemy jump then fall back down
        transform.DOMove(transform.position + new Vector3(0, 3f, _carCtrl.carMovement.speed), 
                                            deadDotweenDuration)
                                            .SetEase(Ease.OutQuad).OnComplete(
                                                () => transform.DOMoveY(0, deadDotweenDuration).SetEase(Ease.InQuad)
                                            );
        GameManager.Instance.PlayerDeadOrWin(false);
        // // Make the enemy spin
        // transform.DORotate(new Vector3(Random.Range(-180f,180f), Random.Range(-180f,180f), 0), 
        //                                     deadDotweenDuration * 2, 
        //                                     RotateMode.FastBeyond360)
        //                                     .SetEase(Ease.InOutQuad).OnComplete(
        //                                         ()=> GameManager.Instance.PlayerDeadOrWin(false)
        //                                     );
    }
}
