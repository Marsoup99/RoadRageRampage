using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static int[] itemDefaultPrices = new int[4]{352, 435, 556, 710};

    public static int GetThePrices(ITEMRarities type, int currentLevel, int currentMap)
    {
        return itemDefaultPrices[(int)type] + currentLevel * 5 + currentMap * 20;
    }
}
