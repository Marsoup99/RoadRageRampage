using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    void Awake()//Set Instance.
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one SaveLoadManager");
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
    }

    public InGameDataSO inGameDataSO;
    public string inGameString = "data.json";
    public MainGameDataSO mainGameDataSO;
    public string mainGameString = "maindata.json";
    public void SaveInGameData()
    {
        // Convert ScriptableObject to JSON
        // InGameDataConvert data = new InGameDataConvert();
        // data.Convert(inGameDataSO);
        string json = JsonUtility.ToJson(inGameDataSO);

        // Save JSON to a local file
        string filePath = Application.persistentDataPath + "/" + inGameString; // Choose the file path and name for your JSON file
        File.WriteAllText(filePath, json);

        Debug.Log("ScriptableObject converted to JSON and saved to: " + filePath);
    }

    public void LoadInGameData()
    {
        string filePath = Application.persistentDataPath + "/" + inGameString; // Specify the file path and name of your JSON file

        // Check if the JSON file exists
        if (File.Exists(filePath))
        {
            // Load JSON from the file
            string json = File.ReadAllText(filePath);

            // Convert JSON to ScriptableObject
            JsonUtility.FromJsonOverwrite(json, inGameDataSO);

            Debug.Log("JSON loaded and converted to ScriptableObject.");
        }
        else
        {
            ClearInGameData();
            Debug.Log("JSON file not found at: " + filePath);
        }

        inGameDataSO.LoadItems();
    }

    public void ClearInGameData()
    {
        inGameDataSO.NewInGameData();
        SaveInGameData();
    }

    public void SaveMainGame()
    {
        // Convert ScriptableObject to JSON
        string json = JsonUtility.ToJson(mainGameDataSO);

        // Save JSON to a local file
        string filePath = Application.persistentDataPath + "/" + mainGameString; // Choose the file path and name for your JSON file
        File.WriteAllText(filePath, json);

        Debug.Log("ScriptableObject converted to JSON and saved to: " + filePath);
    }
    public void LoadMainGame()
    {
        string filePath = Application.persistentDataPath + "/" + mainGameString; // Specify the file path and name of your JSON file

        // Check if the JSON file exists
        if (File.Exists(filePath))
        {
            // Load JSON from the file
            string json = File.ReadAllText(filePath);

            // Convert JSON to ScriptableObject
            JsonUtility.FromJsonOverwrite(json, mainGameDataSO);

            Debug.Log("JSON loaded and converted to ScriptableObject.");
        }
        else
        {
            mainGameDataSO.money = 0;
            mainGameDataSO.ResetProcess();
            SaveMainGame();
            Debug.Log("JSON file not found at: " + filePath);
        }

        mainGameDataSO.UpdateString();
    }

    public void ResetGameData()
    {
        ClearInGameData();

        mainGameDataSO.money = 0;
        mainGameDataSO.ResetProcess();
        SaveMainGame();
    }
}
