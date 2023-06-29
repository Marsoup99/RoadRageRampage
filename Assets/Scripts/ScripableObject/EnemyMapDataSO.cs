using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Enemy/EnemyMapData")]
public class EnemyMapDataSO : ScriptableObject
{
    public int MapIndex = 1;
    public List<Transform> smallEnemies;
    public Transform miniBoss;
    public Transform bigBoss;
    public Transform shopCar;
    [Space]
    public List<LevelEnemyList> levels;

    public Transform GetEnemyByIndex(int currentLvl, int index)
    {
        return smallEnemies[levels[currentLvl].enemyIndexList[index]];
    }
}

[System.Serializable]
public class LevelEnemyList
{
    public LEVELType type = LEVELType.fight;
    public int numberOfEnemies => enemyIndexList.Count;
    public List<int> enemyIndexList; //Set which enemy going to appear by using its index.
}

public enum LEVELType
{
    fight,
    shop,
    miniBoss,
    bigBoss
}
