using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : ObjectPooling
{
    public static EnemySpawner Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one EnemySpawner");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    [SerializeField] private int _maxEnemies = 2;
    private int _numberOfEnemies = 5;
    private int _enemyCount = 0;
    private int _enemyLeft = 0;
    private EnemyMapDataSO enemyMapData;
    private int _currentLevel; 
    private Action onSpawnTarget;
    public LEVELType currentLevelType;
    // protected override void OnEnable()
    // {
    //     base.OnEnable();
    //     _enemyCount = 0;
    //     _enemyLeft = _numberOfEnemies;
    // }

    public void SetEnemy(int currentLevel)
    {
        
        _currentLevel = currentLevel;

        this._numberOfEnemies = enemyMapData.levels[_currentLevel].numberOfEnemies;
        _enemyCount = 0;
        _enemyLeft = _numberOfEnemies;

        if(currentLevel == 0)
        {
            _enemyLeft = 1;
            onSpawnTarget = FirstLevel;
            currentLevelType = LEVELType.shop;
            return;
        }

        currentLevelType = enemyMapData.levels[_currentLevel].type;
        LvlTypeCheck();
    }

    private void LvlTypeCheck()
    {
        switch (currentLevelType)
        {
            case LEVELType.fight:
                onSpawnTarget = SpawnEnemy;
                break;
            case LEVELType.shop:
                _enemyLeft = 1;
                onSpawnTarget = SpawnShop;
                break;
            case LEVELType.miniBoss:
                _enemyLeft = 1;
                onSpawnTarget = SpawnMiniBoss;
                break;
            case LEVELType.bigBoss:
                _enemyLeft = 1;
                onSpawnTarget = SpawnBigBoss;
                break;
        }
    }
    public override void DeSpawnObject(Transform go)
    {
        base.DeSpawnObject(go);
        _enemyCount --;
        GameManager.Instance.EnemyKilled();
        if(_enemyCount == 0 && _enemyLeft <= 0)
        {
            // Debug.Log("ALl DEAD!!!");
            GameManager.Instance.LastEnemyKilled();
        }
    }
    public void Spawn()
    {
        if(_enemyCount < _maxEnemies && _enemyLeft > 0)
        {
            onSpawnTarget.Invoke();
            _enemyLeft --;
            _enemyCount ++;
        }
    }
    private void SpawnEnemy()
    {
        //Get next enemy to spawn.
        Transform enemyGO = enemyMapData.GetEnemyByIndex(_currentLevel, _numberOfEnemies - _enemyLeft);
        

        if(UnityEngine.Random.value < 0.8f) 
        {   
            //Spawn enemy in the last road (in front of player)
            SpawnObject(enemyGO, RoadGenerator.Instance.lastRoadInList.position + Vector3.up , RoadGenerator.Instance.lastRoadInList.rotation);
        }
        else 
        {
            //Spawn enemy in firstRoad (behind player)
            Transform road = RoadGenerator.Instance.firstRoadInList;
            SpawnObject(enemyGO, road.position + Vector3.up + RoadGenerator.Instance._roadLength/2 * road.forward, road.rotation);
        }
    }
    private void SpawnShop()
    {
        SpawnObject(enemyMapData.shopCar, 
                    RoadGenerator.Instance.firstRoadInList.position + Vector3.forward * 200 + Vector3.right * 3, 
                    RoadGenerator.Instance.firstRoadInList.rotation);
        _enemyLeft --;
    }
    private void SpawnMiniBoss()
    {
        SpawnObject(enemyMapData.miniBoss, 
                    RoadGenerator.Instance.lastRoadInList.position + Vector3.up * 2, 
                    RoadGenerator.Instance.lastRoadInList.rotation);
    }

    private void SpawnBigBoss()
    {
        SpawnObject(enemyMapData.bigBoss, 
                    RoadGenerator.Instance.lastRoadInList.position + Vector3.up * 2, 
                    RoadGenerator.Instance.lastRoadInList.rotation);
    }
    private void FirstLevel()
    {
        //Generate road choice
        RoadGenerator.Instance.SpawnChoicesRoad();
    }
    public void LoadEnemyMapData(EnemyMapDataSO enemyMapData)
    {
        this.enemyMapData = enemyMapData;
    }
}
