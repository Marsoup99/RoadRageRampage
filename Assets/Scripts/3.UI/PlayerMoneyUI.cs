using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI tmpMoney;

    void Awake()
    {
        GameManager.Instance.InGameData.onMoneyChange += SetMoneyText;
    }
    void SetMoneyText(int v)
    {
        tmpMoney.text = v.ToString();
    }
}
