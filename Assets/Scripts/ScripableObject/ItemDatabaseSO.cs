using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemData/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [Header("Front Armor")]
    public ItemSO[] frontArmorList;
    
    [Header("Side Armor")]
    public ItemSO[] sideArmorList;

    [Header("Nitro Enegine")]
    public ItemSO[] nitroEngineList;

    [Header("Relic")]
    public ItemSO[] relicList;

    [Header("Weapons")]
    public ItemSO[] weaponList;

    [Header("All Item")]
    public ItemSO[] allItems;
    [Header("Player Upgrade")]
    public Transform nitroInvinciblePrefab;
    [ContextMenu(itemName: "Get items data")]
    public void GetItemsData()
    {
        frontArmorList = Resources.LoadAll<ItemSO>("FrontArmor");
        sideArmorList = Resources.LoadAll<ItemSO>("SideArmor");
        nitroEngineList = Resources.LoadAll<ItemSO>("NitroEngine");
        relicList = Resources.LoadAll<ItemSO>("Relic");
        weaponList = Resources.LoadAll<ItemSO>("Weapon");

        // foreach(ItemSO itemSO in frontArmorList) itemSO.GetItemStat();
        // foreach(ItemSO itemSO in sideArmorList) itemSO.GetItemStat();
        // foreach(ItemSO itemSO in nitroEngineList) itemSO.GetItemStat();
        // foreach(ItemSO itemSO in relicList) itemSO.GetItemStat();
        // foreach(ItemSO itemSO in weaponList) itemSO.GetItemStat();
        allItems = new ItemSO[frontArmorList.Length + sideArmorList.Length + nitroEngineList.Length + relicList.Length + weaponList.Length];
        int index = 0;
        for(int i = 0; i < frontArmorList.Length; i++)
        {
            allItems[index] = frontArmorList[i];
            index ++;
        }
        for(int i = 0; i < sideArmorList.Length; i++)
        {
            allItems[index] = sideArmorList[i];
            index ++;
        }
        for(int i = 0; i < nitroEngineList.Length; i++)
        {
            allItems[index] = nitroEngineList[i];
            index ++;
        }
        for(int i = 0; i < relicList.Length; i++)
        {
            allItems[index] = relicList[i];
            index ++;
        }
        for(int i = 0; i < weaponList.Length; i++)
        {
            allItems[index] = weaponList[i];
            index ++;
        }

        foreach(ItemSO item in allItems)
        {
            item.GetItemStat();
        }
    }
}
