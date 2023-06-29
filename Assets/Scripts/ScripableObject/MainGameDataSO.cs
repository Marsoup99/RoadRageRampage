using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InGameData", menuName = "ScriptableObjects/InGame/MainGameData")]
public class MainGameDataSO : ScriptableObject
{
    public bool didPlayTutorial = false;
    public int money = 0;
    public int[] prices = new int[5]{100, 500, 1000, 2000, 99999};
    [Header("Increase health points")]
    public string healthText = "Increase health by 0 points.";
    public int[] hpValue = new int[5];
    public int hpProcess = 0;
    public int getHpValue => hpValue[hpProcess];

    [Header("Increase nitro power")]
    public string nitroText = "Increase nitro power by 0%";
    public int[] nitroValue = new int[5];
    public int nitroProcess = 0; 
    public int getNitroValue => nitroValue[nitroProcess];

    [Header("Increase collide damage")]
    public string collideText = "Increase collide damage by 0%";
    public int[] collideValue = new int[5];
    public int collideProcess = 0; 
    public int getcollideValue => collideValue[collideProcess];

    [Header("Increase nitro power")]
    public string nitroInvincibleText = "Take no damage at the begin of nitro boost";
    public bool nitroInvincibleProcess = false; 

    public void UpdateString()
    {
        healthText = "Increase health by " + hpValue[hpProcess] + " points.";
        nitroText = "Increase nitro power by " + nitroValue[nitroProcess] + "%";
        collideText = "Increase collide damage by " + collideValue[collideProcess] + "%";
    }

    public void ResetProcess()
    {
        hpProcess = 0;
        nitroProcess =0;
        collideProcess = 0;
        nitroInvincibleProcess = false;
        UpdateString();
    }
}
