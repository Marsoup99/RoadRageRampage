using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleAI : MonoBehaviour
{
     public CarCtr carCtr;
    [SerializeField] protected CarMovement carMovement;
    [Header("weapon")]
    public EnemyWeapon weapon;
    public float weaponTimer = 3f;

    [Header("State")]
    public EnemyDisState myDisState;
    private EnemyDisState newState;
    [SerializeField] private float _checkingTimer = 1; //number of sec before cheking player position (to change state).
    [SerializeField] private LayerMask _checkLayer = 1<<6;
    [SerializeField] protected EnemyDistanceStateSO enemyDistanceStateSO;
    [SerializeField] private float _nitroStateTimer = 2;
    private bool _leftRay, _rightRay;
    protected Transform myTransform;
    
    private float _weaponTimer, _nitroTimer;
    private float _timer;
    private float _weaponBias = 1, _nitroBias = 1;
    private delegate void StateBehavior();
    StateBehavior MyStateBehavior;

    void Reset()
    {
        carMovement = GetComponentInChildren<CarMovement>();
        carCtr = GetComponent<CarCtr>();
        weapon = GetComponentInChildren<EnemyWeapon>();
    }

    protected virtual void Start()
    {
        myTransform = transform;
        MyStateBehavior = TooFarState;
    }
    void Update()
    {
        _timer -= Time.deltaTime;
        MyStateBehavior();

        if(_timer <= 0)
        {
            //Change behavior base on distance between this and player.
            CheckingState();
        }
    }

    public virtual void TooFarState()
    {
        // WeaponActivate(2);
        Dodge();
    }

    public virtual void SafeState()
    {
        Dodge();
        NitroActivate();
        WeaponActivate();
    }
    public virtual void WarningState()
    {
        Dodge();
        NitroActivate();
        WeaponActivate();
    }
    public virtual void UnSafeState()
    {
        if(_timer <= 0)
        {
            if(Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < 3)
            SideCollidePlayer();
        }
        NitroActivate();
    }
    public virtual void BehindState()
    {
        Dodge();
        NitroActivate();
        WeaponActivate();
        
    }

    public virtual void StandingByState()
    {
        
    }

    public void CheckingState()
    {
        _timer = _checkingTimer;
        int topspeed;
        //Get topspeed base on distance between this and player.
        (newState, topspeed) = enemyDistanceStateSO.GetStateTopSpeed(myTransform.position);
        carCtr.carMovement.ChangeTopSpeed(topspeed);

        if(newState != myDisState)
            StateChange();

        //check if enemy is too far behind or too ahead then reset its position on the road.
        if(transform.position.y < -1)
        {
            transform.position += -transform.position.y * Vector3.up;
            if(transform.position.z < RoadGenerator.Instance.firstRoadInList.position.z)
            {
                transform.position = RoadGenerator.Instance.firstRoadInList.position;
            }
            else transform.position = RoadGenerator.Instance.lastRoadInList.position;
        } 
    }
    void StateChange()
    {
        myDisState = newState;
        weapon.RotateWeapon(true);
        switch (newState)
        {
            case EnemyDisState.TooFar:
                MyStateBehavior = TooFarState;
                break;
            case EnemyDisState.Safe:
                _weaponBias = 1;
                _nitroBias = 4;
                MyStateBehavior = SafeState;
                break;
            case EnemyDisState.Warning:
                _weaponBias = 0.75f;
                _nitroBias = 2;
                MyStateBehavior = WarningState;
                break;
            case EnemyDisState.UnSafe:
                _nitroBias = 1;
                MyStateBehavior = UnSafeState;
                break;
            case EnemyDisState.Behind:
                _weaponBias = 1;
                _nitroBias = 0.5f;
                _nitroTimer = 0.5f;
                MyStateBehavior = BehindState;
                weapon.RotateWeapon(false);
                break;
            default:
                break;
        }
    }

    public virtual void ChangeToStandingByState()
    {
        myDisState = EnemyDisState.StandingBy;
        MyStateBehavior = StandingByState;
        _timer = 1000;
    }
    public virtual void Dodge()
    {
        if(carMovement.isChangingLane) return;
        if(Physics.Raycast(myTransform.position + Vector3.up, Vector3.forward, 3f, _checkLayer))
        {
            _leftRay = Physics.Raycast(myTransform.position + Vector3.up, -Vector3.right, 3f, _checkLayer);
            _rightRay = Physics.Raycast(myTransform.position + Vector3.up, Vector3.right, 3f, _checkLayer);
            if(_rightRay && !_leftRay)
            {
                this.carMovement.ChangeLane(LANE.left);
            }
            else if(_leftRay && !_rightRay)
            {
                this.carMovement.ChangeLane(LANE.right);
            }
            else if(!_leftRay && !_rightRay)
            {
                if(Random.value < 0.5f) this.carMovement.ChangeLane(LANE.right);
                else this.carMovement.ChangeLane(LANE.left);
            }
            // else
            // {
            //     carCtr.carMovement.Brake();
            // }
        }
    }
    private void ChangeLaneToPlayer()
    {
        int playerLane = PlayerCtrl.Instance.carMovement.currentLaneSide;
        if(carMovement.currentLaneSide > playerLane && carMovement.currentLaneSide > -1)
        {
            carMovement.ChangeLane(LANE.left);
        }
            
        else if(carMovement.currentLaneSide < playerLane && carMovement.currentLaneSide < 1)
        {
            carMovement.ChangeLane(LANE.right);
        }
    }

    private void WeaponActivate()
    {
        if(_weaponTimer <= 0)
        {
            ChangeLaneToPlayer();
            _weaponTimer = weaponTimer * Random.Range(0.8f , 1.2f) * _weaponBias;
            weapon.WeaponActivate();
        }
        else if(weapon.canFire)
            _weaponTimer -= Time.deltaTime;
    }
    private void NitroActivate()
    {
        if(_nitroTimer <= 0)
        {
            _nitroTimer = _nitroStateTimer * Random.Range(0.8f , 1.2f) * _nitroBias;
            carMovement.Nitro();
        }
        else if(!carMovement.isUsingNitro)
        {
            _nitroTimer -= Time.deltaTime;
        }
    }

    private IEnumerator SideCollidePlayer()
    {
        carMovement.Brake();
        carMovement.speed = PlayerCtrl.Instance.carMovement.speed;
        yield return new WaitForSeconds(_checkingTimer / 2);
        ChangeLaneToPlayer();
    }
}