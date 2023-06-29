using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Phone setting")]
    public int targetFPS = 60;
    public int height = 1600;
    [Header ("Load GameSave")]
    public InGameDataSO inGameDataSO;
    public GameObject ContinueBoxGO;
    [Header ("UpgradeShop")]
    public UpgradeShop shop;
    // Start is called before the first frame update
    void Start()
    {
        Resolution s = Screen.resolutions[0];
        Screen.SetResolution(s.width * height / s.height, height, true);
        Application.targetFrameRate = targetFPS;

        SaveLoadManager.Instance.LoadInGameData();
        SaveLoadManager.Instance.LoadMainGame();
        if(inGameDataSO.loadFromLastRun)
        {
            /// Show a pop up for player to cofirm
            ContinueBoxGO.SetActive(true);
        }

        shop.ShopStart();
    }

    public void OpenUpgradeShop()
    {
        shop.OpenUpgradeShop();

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }
    public void Play()
    {
        ///Start new Game
        LoadingScenesManager.Instance?.LoadScene();
    }

    public void ContinuePlay()
    {
        ContinueBoxGO.SetActive(false);
        //Press continute button
        LoadingScenesManager.Instance?.LoadScene(inGameDataSO.currentMap);

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }
    public void NewPlay()
    {
        ContinueBoxGO.SetActive(false);
        //New game
        SaveLoadManager.Instance?.ClearInGameData();

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }

    public void RESET_GAME_DATA()
    {
        SaveLoadManager.Instance?.ResetGameData();
        shop.ShopStart();
    }
    
}
