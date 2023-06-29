using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGameData : MonoBehaviour
{
    public GameManager GM;
    [SerializeField] private InGameDataSO _dataSO;

    public ItemSO[] shopItems => _dataSO.currentShopItems;
    public int playerMoney => _dataSO.currentMoney;
    public LEVELREWARD[] roadChoices => _dataSO.roadChoices;
    private System.Random rng;
    public Action<int> onMoneyChange;
    void Reset()
    {
        GM = GetComponentInParent<GameManager>();
        _dataSO = Resources.LoadAll<InGameDataSO>("InGameData")[0];
    }
    void Awake()
    {
        rng = new System.Random(UnityEngine.Random.Range(0,10000));
    }
    public void GameStart()
    {
        MainGameDataSO mainGameData = SaveLoadManager.Instance?.mainGameDataSO;
        if(mainGameData != null)
        {
            PlayerCtrl.Instance.carStat.hpStat.MaxValueAddFlat(new StatModifier(mainGameData.getHpValue, StatModType.Flat));
            PlayerCtrl.Instance.carStat.nitroPowerStat.MaxValueAddPercent(new StatModifier(mainGameData.getNitroValue, StatModType.PercentAdd));
            PlayerCtrl.Instance.carStat.collisionDmgStat.AddModifier(new StatModifier(mainGameData.getcollideValue, StatModType.PercentAdd));
            if(mainGameData.nitroInvincibleProcess)  Instantiate(ItemsDatabase.Instance.myItems.nitroInvinciblePrefab, PlayerCtrl.Instance.transform);

            //Play Tutorial
            if(mainGameData.didPlayTutorial == false)
            {
                if(SaveLoadManager.Instance != null)
                {
                    SaveLoadManager.Instance.ClearInGameData();
                }
                else{
                    _dataSO.NewInGameData();
                }
                EnemySpawner.Instance.currentLevelType = LEVELType.shop;
                GameObject tutorial = Instantiate(GM.tutorialPrefab);
                tutorial.GetComponent<InGameTutorial>().TutorialStart();
                return;
            }
        }
        if(_dataSO.loadFromLastRun)
        {
            //Load old data
            GM.LevelManager.SetCurrentLevel(_dataSO.currentLevel);
            //Load other thing like money, items, weapon.
            EquipItem(_dataSO.equippedWeapon);
            EquipItem(_dataSO.equippedFrontArmor);
            EquipItem(_dataSO.equippedNitroEngine);
            EquipItem(_dataSO.equippedSideArmor);
            for (int i = 0; i < _dataSO.equippedRelics.Length; i++)
            {
                EquipRelic(i, _dataSO.equippedRelics[i]);
            }

            //Load player HP, Armor, etc...
            PlayerCtrl.Instance.carStat.hpStat.SetCurrentValue(_dataSO.currentHP);
            PlayerCtrl.Instance.carStat.armorStat.SetCurrentValue(_dataSO.currentArmor);
        }
        else 
        {
            StartNewRun();
        }
        onMoneyChange?.Invoke(_dataSO.currentMoney);
    }
    void StartNewRun()
    {
        if(SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.ClearInGameData();
        }
        else{
            _dataSO.NewInGameData();
        }
        GM.LevelManager.SetCurrentLevel(0);
        _dataSO.currentLevel = GM.LevelManager.currentLevel;

        //Random road choice.
        ShuffleNextChoices();

        //Save player hp
        _dataSO.currentHP = PlayerCtrl.Instance.carStat.hpStat.currentValue;
        _dataSO.currentArmor = PlayerCtrl.Instance.carStat.armorStat.currentValue;
    }

    public void NextRoadRewards(int index, LEVELType type)
    {
        //generate item reward.
        if(type == LEVELType.fight)
        {
            StartCoroutine(GenerateRoadRewards(_dataSO.roadChoices[index]));
        }
        else if(type == LEVELType.shop)
        {
            if(index == 0) StartCoroutine(GenerateShopRelics());
            if(index == 2) StartCoroutine(GenerateShopItems());
        }
        else if (type == LEVELType.miniBoss)
        {
            StartCoroutine(GenerateRoadRewards(LEVELREWARD.relic));
        }
        else if (type == LEVELType.bigBoss)
        {
            StartCoroutine(GenerateRoadRewards(LEVELREWARD.weapon));
        }

        //Pre random road choice.
        ShuffleNextChoices();

        //TODO: Save in game data current lvl.
        _dataSO.currentLevel = GM.LevelManager.currentLevel;
        _dataSO.loadFromLastRun = true;
        //Save player hp
        _dataSO.currentHP = PlayerCtrl.Instance.carStat.hpStat.currentValue;
        _dataSO.currentArmor = PlayerCtrl.Instance.carStat.armorStat.currentValue;

        // SaveLoadManager.Instance?.SaveInGameData();
    }
    public void EquipItem(ItemSO item)
    {
        if(item == null) return;

        //Give player item.
        PlayerCtrl.Instance.playerItems.EquipNewItem(item.type, item.itemPrefab);
        //Update inventory ui
        GM.InventoryUI.EquipItem(item);
        
        _dataSO.SaveItems(item);
    }
    public void EquipRelic(int slot, ItemSO item)
    {
        if(item == null) return;

        //Give player item.
        PlayerCtrl.Instance.playerItems.EquipNewRelic(slot, item.itemPrefab);
        //Update inventory UI.
        GM.InventoryUI.EquipRelics(slot, item);

        _dataSO.SaveRelics(slot, item);
    }

    public ItemSO[] GetCurrentLvLRewards()
    {
        if(_dataSO.currentReward[0] == null)
        {
            MoneyChange(500 * _dataSO.currentMap + 10 * _dataSO.currentLevel);
            GM.DoneChosingReward();
            return null;
        }
        return _dataSO.currentReward;
    }
    private IEnumerator GenerateRoadRewards(LEVELREWARD type)
    {
        ItemSO[] arr = new ItemSO[3];
        for(int i =0; i< arr.Length; i++)
        {
            arr[i] = ItemsDatabase.Instance.GetRandomItemByReward(type);
            if(arr[i] != null)
                _dataSO.currentRewardIds[i] = arr[i].itemID;
            else _dataSO.currentRewardIds[i] = 0;
            yield return null;
        }
        _dataSO.currentReward = arr;

        SaveLoadManager.Instance?.SaveInGameData();
    }
    private IEnumerator GenerateShopItems()
    {
        ItemSO[] arr = new ItemSO[6];
        for(int i =0; i<arr.Length; i++)
        {
            arr[i] = ItemsDatabase.Instance.GetRandomItem();
            _dataSO.currentShopItemIds[i] = arr[i].itemID;
            yield return null;
        }
        Array.Sort(arr, (x, y) => (int)x.type - (int)y.type);
        _dataSO.currentShopItems = arr;

        SaveLoadManager.Instance?.SaveInGameData();
    }
    private IEnumerator GenerateShopRelics()
    {
        ItemSO[] arr = new ItemSO[4];
        for(int i =0; i<arr.Length; i++)
        {
            arr[i] = ItemsDatabase.Instance.GetRandomItemType(ITEM.relic);
            _dataSO.currentShopItemIds[i] = arr[i].itemID;
            yield return null;
        }
        _dataSO.currentShopItems = arr;

        SaveLoadManager.Instance?.SaveInGameData();
    }
    private void ShuffleNextChoices()
    {
        // Iterate over the array in reverse order
        for (int i = _dataSO.roadChoices.Length - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i
            int j = rng.Next(0, i + 1);

            // Swap elements at indices i and j
            LEVELREWARD temp = _dataSO.roadChoices[i];
            _dataSO.roadChoices[i] = _dataSO.roadChoices[j];
            _dataSO.roadChoices[j] = temp;
        }
    }
    public void MoneyChange(int v)
    {
        _dataSO.currentMoney += v;
        onMoneyChange?.Invoke(_dataSO.currentMoney);
    }

    public void EnemyKilled(LEVELType type)
    {
        if(type == LEVELType.fight)
            _dataSO.smallEnemyKills ++;
        else if(type == LEVELType.miniBoss)
            _dataSO.miniBoss ++;
        else if(type == LEVELType.bigBoss)
            _dataSO.bossKills ++;
    }

    public (int, int, int) GetEnemyKilledNumber()
    {
        return (_dataSO.smallEnemyKills, _dataSO.miniBoss, _dataSO.bossKills);
    }
}
