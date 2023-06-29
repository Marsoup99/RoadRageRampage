using DG.Tweening;
using UnityEngine;
public class PlayerCarMovement : CarMovement
{   
    [Header("Player additional setting")]
    public IconUI nitroIcon;
    public DeathTimerUI deathTimer;
    private MyTimer _speedTimer = new MyTimer(5);
    protected override void Awake()
    {
        base.Awake();
        nitroIcon.SetTargetTimer(_nitroCooldownTimer);

        _speedTimer.StartTimer();
        deathTimer.SetTargetTimer(_speedTimer);
    }
    void OnEnable()
    {
        //Assign input for movement controls.
        InputManagerSingleton.Instance.onSwipeLeft += ChangeLaneLeft;
        InputManagerSingleton.Instance.onSwipeRight += ChangeLaneRight;
        InputManagerSingleton.Instance.onSwipeUp += Nitro;
        InputManagerSingleton.Instance.onSwipeDown += Brake;

    }

    void OnDisable()
    {
        InputManagerSingleton.Instance.onSwipeLeft -= ChangeLaneLeft;
        InputManagerSingleton.Instance.onSwipeRight -= ChangeLaneRight;
        InputManagerSingleton.Instance.onSwipeUp -= Nitro;
        InputManagerSingleton.Instance.onSwipeDown -= Brake;

    }

    void LateUpdate()
    {
        if(speed < 15)
        {
            _speedTimer.TimerUpdate();
            if(_speedTimer.IsStop())
            {
                _carCtr.carStat.Dead();
            }
            return;
        }
    }
    private void ChangeLaneLeft()
    {
        ChangeLane(LANE.left);
    }
    private void ChangeLaneRight()
    {
        ChangeLane(LANE.right);
    }
    protected override void NitroActivated()
    {
        base.NitroActivated();
        PlayerCtrl.Instance.onUsingNitro?.Invoke();
        CameraControl.Instance.NitroCameraFeel(nitroDuration);
    }
    public override void Brake()
    {
        if(isUsingNitro)
        {
            CameraControl.Instance.StopNitroCameraFeel();
        }
        base.Brake();
    }

    // public void MovingLeftRightTutorialInput()
    // {
    //     InputManagerSingleton.Instance.onSwipeLeft -= ChangeLaneLeft;
    //     InputManagerSingleton.Instance.onSwipeRight -= ChangeLaneRight;
    //     InputManagerSingleton.Instance.onSwipeUp -= Nitro;
    //     InputManagerSingleton.Instance.onSwipeDown -= Brake;

    //     InputManagerSingleton.Instance.onSwipeLeft += ChangeLaneLeft;
    //     InputManagerSingleton.Instance.onSwipeRight += ChangeLaneRight;
    // }

    // public void NitroAndBrakeTutorial()
    // {
    //     InputManagerSingleton.Instance.onSwipeLeft -= ChangeLaneLeft;
    //     InputManagerSingleton.Instance.onSwipeRight -= ChangeLaneRight;
        
    //     InputManagerSingleton.Instance.onSwipeUp += Nitro;
    //     InputManagerSingleton.Instance.onSwipeDown += Brake;
    // }
}
