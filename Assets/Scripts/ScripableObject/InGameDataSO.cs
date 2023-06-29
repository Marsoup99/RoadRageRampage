using System;
using System.Collections.Generic;
using UnityEngine;

// [Serializable]
// public class InGameDataConvert
// {
//     public int[] myArray;
//     public void Convert(InGameDataSO data)
//     {
//         for (int i =0; i< data.currentRewardIds.Length; i++)
//          Debug.Log(data.currentRewardIds[i]);
//         myArray = data.currentRewardIds;
//     }
// }


[CreateAssetMenu(fileName = "InGameData", menuName = "ScriptableObjects/InGame/InGameData")]
public class InGameDataSO : ScriptableObject
{
    public bool loadFromLastRun = false;
    [Header("Enemy kill")]
    public int smallEnemyKills = 0;
    public int miniBoss = 0;
    public int bossKills = 0;
    public int currentLevel = 0;
    public int currentMap = 1;
    public LEVELREWARD[] roadChoices = new LEVELREWARD[6] {
        LEVELREWARD.money, 
        LEVELREWARD.armor, 
        LEVELREWARD.weapon, 
        LEVELREWARD.relic,
        LEVELREWARD.none,
        LEVELREWARD.none
        };
    public ItemSO[] currentReward = new ItemSO[3];
    public ItemSO[] currentShopItems = new ItemSO[6];
    public int currentMoney = 0;
    [Header("Player stat")]
    public int currentHP;
    public int currentArmor;

    [Header("Weapon")]
    public ItemSO equippedWeapon;
    public int weaponTier = 0;
    [Header("Front Armor")]
    public ItemSO equippedFrontArmor;
    public int frontArmorTier = 0;
    [Header("Side Armor")]
    public ItemSO equippedSideArmor;
    public int sideArmorTier = 0;
    [Header("Nitro Engine")]
    public ItemSO equippedNitroEngine;
    public int nitroEngineTier = 0;
    [Header("Relic")]
    public ItemSO[] equippedRelics = new ItemSO[4];
    public int[] relicTier = new int[4];

    [HideInInspector]
    public int[] itemIds = new int[8];
    public int[] currentRewardIds = new int[3];
    public int[] currentShopItemIds = new int[6];
    public void NewInGameData()
    {
        loadFromLastRun = false;
        
        smallEnemyKills = 0;
        bossKills = 0;
        miniBoss = 0;
        
        currentLevel = 0;
        currentMap = 1;

        currentMoney = 0;
        //Reset front armor
        equippedWeapon = null;
        weaponTier = 0;

        //Reset front armor
        equippedFrontArmor = null;
        frontArmorTier = 0;

        //Reset side armor
        equippedSideArmor = null;
        sideArmorTier = 0;

        //Reset nitro engine
        equippedNitroEngine = null;
        nitroEngineTier = 0;

        //Reset relic
        for (int i = 0; i < equippedRelics.Length; i++)
        {
            equippedRelics[i] = null;
            relicTier[i] = 0;
        }
        itemIds = new int[8];
        currentRewardIds = new int[3];
        currentShopItemIds = new int[6];
    }

    public void SaveItems(ItemSO item)
    {
        ITEM type = item.type;
        if(type == ITEM.weapon) 
        {
            equippedWeapon = item;
            itemIds[0] = item.itemID;
        }
        else if(type == ITEM.frontArmor) 
        {
            equippedFrontArmor = item;
            itemIds[1] = item.itemID;
        }
        else if(type == ITEM.sideArmor)
        {
           equippedSideArmor = item; 
           itemIds[2] = item.itemID;
        } 
        else if(type == ITEM.nitroEngine)
        {
            equippedNitroEngine = item;
            itemIds[3] = item.itemID;
        } 
    }

    public void SaveRelics(int index, ItemSO item)
    {
        equippedRelics[index] = item;
        itemIds[index + 4] = item.itemID;
    }

    public void LoadItems()
    {
        if(itemIds[0] == 0) equippedWeapon = null;
        else equippedWeapon = ItemsDatabase.Instance.GetItemByID(ITEM.weapon, itemIds[0]);

        if(itemIds[1] == 0) equippedFrontArmor = null;
        else equippedFrontArmor = ItemsDatabase.Instance.GetItemByID(ITEM.frontArmor, itemIds[1]);

        if(itemIds[2] == 0) equippedSideArmor = null;
        else equippedSideArmor = ItemsDatabase.Instance.GetItemByID(ITEM.sideArmor, itemIds[2]);

        if(itemIds[3] == 0) equippedNitroEngine = null;
        else equippedNitroEngine = ItemsDatabase.Instance.GetItemByID(ITEM.nitroEngine, itemIds[3]);

        for (int i = 0; i<equippedRelics.Length; i++)
        {
            if(itemIds[i+4] == 0) equippedRelics[i] = null;
            else
                equippedRelics[i] = ItemsDatabase.Instance.GetItemByID(ITEM.relic, itemIds[i + 4]);
        }

        currentReward = ItemsDatabase.Instance.GetItemByID(currentRewardIds);
        currentShopItems = ItemsDatabase.Instance.GetItemByID(currentShopItemIds);
    }
}
