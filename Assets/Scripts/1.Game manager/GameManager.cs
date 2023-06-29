using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    void Awake()//Set Instance.
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one GameManager");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    [Header("Phone setting")]
    public int targetFPS = 60;
    public int height = 1600;
    public LevelManager LevelManager;
    public InGameData InGameData;
    public RoadChoiceUI RoadChoiceUI;
    public FadingScreen fadingScreen;
    public ItemRewardUICtrl ItemRewardUI;
    public InventoryUI InventoryUI;
    public ShopUI ShopUI;
    public IngameUI ingameUI;
    public GameObject tutorialPrefab;
    [Header("enemy reward kill")]
    [SerializeField] private int _enemyBaseValue = 50;

    void Reset()
    {
        LevelManager = GetComponentInChildren<LevelManager>();
        InGameData = GetComponentInChildren<InGameData>();
        RoadChoiceUI = GameObject.FindObjectOfType<RoadChoiceUI>(); 
        fadingScreen = GameObject.FindObjectOfType<FadingScreen>();
        ItemRewardUI = GameObject.Find("ItemRewardUI").GetComponent<ItemRewardUICtrl>();
        InventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        ShopUI = GameObject.Find("ShopUI").GetComponent<ShopUI>();
        ingameUI = GetComponentInChildren<IngameUI>();
    }
    void Start()
    {
        // Resolution s = Screen.resolutions[0];
        // Screen.SetResolution(s.width * height / s.height, height, true);
        // Application.targetFrameRate = targetFPS;
        //Load map data for enemy spawner
        GameStart();
    }

    public void GameStart()
    {
        PlayerCtrl.Instance.gameObject.SetActive(false);
        PlayerCtrl.Instance.gameObject.SetActive(true);
        EnemySpawner.Instance.LoadEnemyMapData(LevelManager.enemyMapData);
        
        InGameData.GameStart();
        // EnemySpawner.Instance.currentLevelType = LEVELType.shop;
        // tutorialPrefab = Instantiate(tutorialPrefab);
        // tutorialPrefab.GetComponent<InGameTutorial>().TutorialStart();

        ResetPlayerPosition();
        //Camera animation
        CameraControl.Instance.CameraStart();
    }

    public void LastEnemyKilled()
    {
        TimeEffectControl.DoSlowMotion(0);
        InputManagerSingleton.Instance.isRecevingInput = false;
        
        LEVELType type = LevelManager.LevelType();
        if(type != LEVELType.shop)
            ShowItemRewardUI();

        //Spawn choice road.
        RoadGenerator.Instance.SpawnChoicesRoad();
    }

    public void FinishedLevel()
    {
        if(PlayerCtrl.Instance.carStat.isDead) return;
        if(LevelManager.currentLevel == LevelManager.maxLevelInMap)
        {
            FinishedMap();
            return;
        }
        LevelManager.NextLevel();
        ShowNextRoadRewards();
    }
    private void FinishedMap()
    {
        Debug.Log("End Map!");

        //End of demo
        PlayerDeadOrWin(true);
        //TODO: Change scense to next map or end the game.
    }
    private void ShowItemRewardUI()
    {
        ItemSO[] rewards = InGameData.GetCurrentLvLRewards();
        if(rewards != null)
            ItemRewardUI.ShowItemRewardUI(rewards);
    }
    // private void ShowBossItemRewardUI()
    // {
    //     //TODO: make boss drop item.
    //     ItemRewardUI.ShowItemRewardUI(InGameData.GetCurrentLvLRewards());
    //     // Debug.Log("boss drop");
    // }
    public void DoneChosingReward()
    {
        TimeEffectControl.DoNormalTime();
        InputManagerSingleton.Instance.isRecevingInput = true;
    }
    public void ShowNextRoadRewards()
    {
        fadingScreen.FadeByAlpha(0.5f);
        TimeEffectControl.DoSlowMotion();
        InputManagerSingleton.Instance.isRecevingInput = false;

        PlayerCtrl.Instance.carMovement.speed = 30;

        if(LevelManager.LevelType() == LEVELType.fight)
            RoadChoiceUI.ShowChoices(InGameData.roadChoices);
        else RoadChoiceUI.ShowChoices(LevelManager.LevelType());
    }
    public void DoneChoosingNextRoad(int index)
    {
        fadingScreen.FadeIn();
        TimeEffectControl.DoNormalTime();
        //Reset Position back to zero to avoid floating point.
        Invoke(nameof(ResetPlayerPosition), 2f);

        //Random next rewards and road choices.
        InGameData.NextRoadRewards(index, LevelManager.LevelType());
    }

    public void OpenShop()
    {
        InputManagerSingleton.Instance.isRecevingInput = false;
        ShopUI.OpenShop();    
    }
    public void ResetPlayerPosition()
    {
        fadingScreen.FadeOut();
        InputManagerSingleton.Instance.isRecevingInput = true;
        PlayerCtrl.Instance.ResetPlayerPosition();
        RoadGenerator.Instance.SpawnStartingRoad();
    }

    public void EnemyKilled()
    {
        LEVELType type = LevelManager.LevelType();
        if(type == LEVELType.shop) return;

        int reward = _enemyBaseValue * LevelManager.currentMap + _enemyBaseValue/10 * LevelManager.currentLevel;
        
        if( type == LEVELType.fight) InGameData.MoneyChange(reward);
        else if (type == LEVELType.miniBoss) InGameData.MoneyChange(reward * 4);
        else if (type == LEVELType.bigBoss) InGameData.MoneyChange(reward * 8);
        InGameData.EnemyKilled(type);
    }

    public void PlayerDeadOrWin(bool isWin)
    {
        if(isWin) ingameUI.OpenFinishPannel(true);
        else
        {
            ingameUI.OpenFinishPannel(false);
        }
    }
}
