using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LANE {left, right}
public class CarMovement : MonoBehaviour
{
    
    [Header("Get from parent")]
    [SerializeField] protected CarCtr _carCtr;
    [SerializeField] private GameObject _nitroVFX;
    private Transform _target;

    [Header("Car attribute")]
    [SerializeField] protected float _baseTopSpeed = 30;
    public float speed = 10;
    protected float _speedAcceleration = 7;
    private float _maxDegreesDelta = 0.69f; //changing the speed of turning animation.
    private float _topSpeed;
    private Vector3 _currentDirection;
    private Vector3 _desireDirection = Vector3.forward;
    private Vector3 _slopeDirection;

    [Header("Nitro")]
    [SerializeField] protected float _nitroCooldown = 1;
    protected MyTimer _nitroCooldownTimer;
    public bool isUsingNitro {get; private set;}
    public float nitroDuration {get; private set;}
    [SerializeField] private AudioClip _nitroAudio;
    private AudioSource _nitroSourceTmp;


    [Header("Lane")]
    [SerializeField] private Vector3 _changeLaneSpeedScaleVector = new Vector3 (1f, 1, 1);
    [SerializeField] private float _laneDistance = 3;
    public LANE lastInput {get; private set;} = LANE.left;
    public int currentLaneSide = 0; //{-1: left side, 0: middle, 1: right side}
    private float _middleLanePos;
    private int _maxleft = -2, _maxright = 2;
    
    [Header("Check")]
    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private LayerMask _roadLayerMask = 1<<6;
    [SerializeField] private float _rayCheckLength = 0.3f;
    public bool isChangingLane {get; private set;}
    private Quaternion _lockRotY;
    
    
    void Reset()
    {
        _carCtr = GetComponentInParent<CarCtr>();
        Debug.Log(this + " loaded " + _carCtr);

        _nitroVFX = _carCtr.carModel.Find("NitroVFXholder").gameObject;
    }
    
    protected virtual void Awake()
    {
        if (_target == null)
            _target = _carCtr.transform;

        _topSpeed = _baseTopSpeed;

        _currentDirection = _desireDirection = _target.forward;
        _middleLanePos = _target.position.x;

        _nitroCooldownTimer = new MyTimer(_nitroCooldown);
    }

    public void SetCarStat(float speedAcceleration, float nitroDuration)
    {
        this._speedAcceleration = speedAcceleration;
        this.nitroDuration = nitroDuration;

        //Reset some value
        _topSpeed = _baseTopSpeed;
        isUsingNitro = false;
        _nitroVFX.SetActive(false);
        currentLaneSide = 0;
    }
    // Update is called once per frame
    void Update()
    {
        CarAcceleration();
        _nitroCooldownTimer.TimerUpdate();
        //get correct direction from current target position
        _currentDirection = _desireDirection - (_target.position.x - _middleLanePos) * _target.right / _laneDistance;

        //If currentDirection not forward mean car is chargingLane
        if(_currentDirection.normalized.z > 0.99) isChangingLane = false;
        //Rotate model to moving direction
        _carCtr.carModel.rotation  = Quaternion.RotateTowards(_carCtr.carModel.rotation, Quaternion.LookRotation(_currentDirection.normalized + _slopeDirection), this._maxDegreesDelta);

        _currentDirection = Vector3.Scale(_currentDirection, _changeLaneSpeedScaleVector);
        if(GroundCheck())
        {
            _desireDirection.y = 0;

            _lockRotY = _target.rotation;
            _lockRotY.y = 0;
            _target.rotation = _lockRotY;
        }
        else 
        {
            if(_desireDirection.normalized.y > -0.5) _desireDirection -= Vector3.up * Time.deltaTime;
        }
        _carCtr.characterController.Move(_currentDirection.normalized * speed * Time.deltaTime);
    }
    private void CarAcceleration()
    {
        if(speed < _topSpeed)
        {
            speed += _speedAcceleration * Time.deltaTime;
        }
        else {
            speed = Mathf.Lerp(speed, _topSpeed, Time.deltaTime);
        }
    }
    public virtual void ChangeLane(LANE lane)
    {
        _carCtr.ShakeTheCar(0.25f);
        if(currentLaneSide == _maxleft && lane == LANE.left) return;
        if(currentLaneSide == _maxright && lane == LANE.right) return;

        isChangingLane = true;

        if(lane == LANE.left) currentLaneSide --;
        else currentLaneSide ++;

        lastInput = lane;
        _desireDirection = _target.forward + _target.right * currentLaneSide + _desireDirection.y * Vector3.up;
    }

    public virtual void Nitro()
    {
        if(_nitroCooldownTimer.IsStop() && !isUsingNitro)
        {
            _nitroCooldownTimer.SetTimeLeft();
            NitroActivated();
        }
    }
    protected virtual void NitroActivated()
    {

        isUsingNitro = true;
        _nitroVFX.SetActive(true);
        _topSpeed += _carCtr.carStat.nitroPowerStat.currentValue;
        speed += _carCtr.carStat.nitroPowerStat.currentValue / 2;
        _speedAcceleration += _carCtr.carStat.nitroPowerStat.currentValue / 2;
        Invoke(nameof(NitroDeActivate), nitroDuration);

        //Play sound
        if(_nitroAudio != null)
            _nitroSourceTmp = SoundManager.Instance?.sfxPlayer.PlayAudioAtPos(_nitroAudio, transform.position, this.transform);
    }
    protected virtual void NitroDeActivate()
    {
        isUsingNitro = false;
        _nitroVFX.SetActive(false);
        _topSpeed = _baseTopSpeed;
        _speedAcceleration = _carCtr.carStat.speedAcceleration;

        _nitroCooldownTimer.StartTimer();

        //StopSound
        _nitroSourceTmp?.Stop();
    }

    public virtual void Brake()
    {
        if(isUsingNitro) 
        {
            CancelInvoke();
            NitroDeActivate();
        }
        if(!_isGrounded) return;
        if(speed > _topSpeed) speed = _topSpeed;
        speed -= _baseTopSpeed / 2;
        if(speed < 5) speed = 5;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void ChangeTopSpeed(float value)
    {
        _topSpeed = _baseTopSpeed + value;
        if(isUsingNitro) _topSpeed += _carCtr.carStat.nitroPowerStat.currentValue/2;
    }
    public virtual void OnSlope(Vector3 slope)
    {
        this._slopeDirection = slope;
        _desireDirection.y = 0.3f;
    }
    private bool GroundCheck()
    {
        _isGrounded = Physics.Raycast(_target.position + Vector3.up * 0.1f, -Vector3.up, _rayCheckLength, _roadLayerMask);
        return _isGrounded;
    }
}
