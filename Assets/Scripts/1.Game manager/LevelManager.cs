using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LEVELREWARD{
    money,
    armor,
    weapon,
    relic,
    none
}
public class LevelManager : MonoBehaviour
{
    public int currentMap = 1;
    public int currentLevel = 0;
    public int maxLevelInMap;
    // public LEVELREWARD _currentRewardChoice = LEVELREWARD.weapon;
    public EnemyMapDataSO enemyMapData;

    void Awake()
    {
        currentMap = enemyMapData.MapIndex;
        maxLevelInMap = enemyMapData.levels.Count - 1;
        currentLevel = 0;
    }

    public void SetCurrentLevel(int currentLevel)
    {
        //Reload road data when reload the game.
        this.currentLevel = currentLevel;
        // this._currentRewardChoice = currentRewardChoice;
        EnemySpawner.Instance.SetEnemy(currentLevel);
    }

    public void NextLevel()
    {
        // _currentRewardChoice = choice;
        currentLevel ++;
        if(currentLevel <= maxLevelInMap) 
        {
            EnemySpawner.Instance.SetEnemy(currentLevel);
        }
    }

    public LEVELType LevelType()
    {
        return enemyMapData.levels[currentLevel].type;
    }
    
}
