using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemsDatabase : MonoBehaviour
{
    static public ItemsDatabase Instance {get; private set;}
    [SerializeField] public ItemDatabaseSO myItems;
    private System.Random RNG;

    // public Dictionary<ItemSO, int> testing;
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one ItemsDatabase");
            // Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        RNG = new System.Random(Random.Range(0,10000));
    }

    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.K))
    //     {
    //         Testing();
    //     }
    // }
    // public void Testing()
    // {
    //     testing = new Dictionary<ItemSO, int>();
    //     ItemSO tmp;
    //     for (int i = 0; i < 1000; i++)
    //     {
    //         tmp = GetRandomItem(ITEM.weapon);
    //         if(testing.ContainsKey(tmp)) testing[tmp] ++;
    //         else testing.Add(tmp, 1);
    //     }
    //     for (int i = 0; i < testing.Count; i ++)
    //     {
    //         Debug.Log(testing.ElementAt(i));
    //     }
    // }
    public ItemSO GetRandomItem()
    {
        int v = RNG.Next(4);
        if(v == 0) return myItems.weaponList[RNG.Next(myItems.weaponList.Length)];
        else if (v == 1) return myItems.sideArmorList[RNG.Next(myItems.sideArmorList.Length)];
        else if (v == 2) return myItems.frontArmorList[RNG.Next(myItems.frontArmorList.Length)];
        else return myItems.nitroEngineList[RNG.Next(myItems.nitroEngineList.Length)];
        // return myItems.allItems[RNG.Next(myItems.allItems.Length)];
    }
    public ItemSO GetRandomItemType(ITEM type)
    {
        if(type == ITEM.frontArmor) return myItems.frontArmorList[RNG.Next(myItems.frontArmorList.Length)];
        else if(type == ITEM.sideArmor) return myItems.sideArmorList[RNG.Next(myItems.sideArmorList.Length)];
        else if(type == ITEM.nitroEngine) return myItems.nitroEngineList[RNG.Next(myItems.nitroEngineList.Length)];
        else if(type == ITEM.relic) return myItems.relicList[RNG.Next(myItems.relicList.Length)];
        else if(type == ITEM.weapon) return myItems.weaponList[RNG.Next(myItems.weaponList.Length)];

        return null;
    }

    public ItemSO GetItemByID(ITEM type, int id)
    {
        if(type == ITEM.frontArmor) return myItems.frontArmorList.FirstOrDefault(i => i.itemID == id);
        else if(type == ITEM.sideArmor) return myItems.sideArmorList.FirstOrDefault(i => i.itemID == id);
        else if(type == ITEM.nitroEngine) return myItems.nitroEngineList.FirstOrDefault(i => i.itemID == id);
        else if(type == ITEM.relic) return myItems.relicList.FirstOrDefault(i => i.itemID == id);
        else if(type == ITEM.weapon) return myItems.weaponList.FirstOrDefault(i => i.itemID == id);

        return null;
    }

    public ItemSO[] GetItemByID(int[] ids)
    {
        ItemSO[] items = new ItemSO[ids.Length];
        for (int i = 0; i<ids.Length;i++)
        {
            if(ids[i] == 0)  items[i] = null;
            else 
            {
                items[i] = myItems.allItems.FirstOrDefault(j => j.itemID == ids[i]);
            }
        }
        return items;
    }

    public ItemSO GetRandomItemByReward(LEVELREWARD rewardType)
    {
        if(rewardType == LEVELREWARD.money) return null;
        else if(rewardType == LEVELREWARD.relic)
        {
            return GetRandomItemType(ITEM.relic);
        }
        else if(rewardType == LEVELREWARD.weapon)
        {
            if(RNG.Next(2) == 0)
                return GetRandomItemType(ITEM.weapon);
            else return GetRandomItemType(ITEM.nitroEngine);
        }
        else if(rewardType == LEVELREWARD.armor)
        {
            if(RNG.Next(2) == 0)
                return GetRandomItemType(ITEM.frontArmor);
            else return GetRandomItemType(ITEM.sideArmor);
        }
        return null;
    }
}
