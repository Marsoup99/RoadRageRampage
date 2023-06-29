using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadChoiceBtnUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText;

    void Reset()
    {
        tmpText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(LEVELREWARD type)
    {
        if(type == LEVELREWARD.money) tmpText.text = "MONEY";
        
        else if(type == LEVELREWARD.weapon) tmpText.text = "WEAPON";
        
        else if(type == LEVELREWARD.armor) tmpText.text = "ARMOR";
        
        else if(type == LEVELREWARD.relic) tmpText.text = "RELIC";
    }

    public void SetText(LEVELType type)
    {
        if(type == LEVELType.shop) tmpText.text = "SHOP";
        else if(type == LEVELType.miniBoss) tmpText.text = "Mini Boss";
        else if(type == LEVELType.bigBoss) tmpText.text ="BOSS";
    }
    public void SetTextShop(bool isRelic)
    {
        if(isRelic) tmpText.text = "Relic shop";
        else tmpText.text = "Mechanic shop";
    }
}
