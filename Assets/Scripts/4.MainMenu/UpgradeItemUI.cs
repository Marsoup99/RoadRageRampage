using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    public Transform processTransform;
    public GameObject[] processIcon;
    public TextMeshProUGUI description;
    public TextMeshProUGUI prices;
    public Image colorImg;
    public void AddProcess(int process)
    {
        if(process == 0)
        {
            colorImg.color = Color.gray;
        }
        for (int i = 0; i < processIcon.Length; i++)
        {
            if(i <= process) processIcon[i].SetActive(true);
            else processIcon[i].SetActive(false);
        }
    }
    public void SetUI(string txt, int process, int prices)
    {
        
        description.text = txt;
        AddProcess(process);
        
        if(prices == 99999)
        {
            this.prices.text = "MAX";
            colorImg.color = Color.green;
        }
            
        else
            this.prices.text = prices.ToString();
    }

}
