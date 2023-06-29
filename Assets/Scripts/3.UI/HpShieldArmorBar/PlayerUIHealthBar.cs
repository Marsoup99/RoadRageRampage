using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIHealthBar : UIHealthBar
{
    // [SerializeField] private TextMeshPro text;
    public TextMeshProUGUI TMP_hp, TMP_shield, TMP_armor;
    public override void SetHealthBarUI ()
    {
        hpSlider.value = (float)hp.currentValue / (float)hp.maxValue;
        TMP_hp.text = hp.currentValue + "/" + hp.maxValue;
        // text.text = currentHealth.ToString();
    }
    public override void SetShieldUI ()
    {
        if(shield.maxValue == 0) 
        {
            shieldSlider.gameObject.SetActive(false);
            shieldSlider.value = 0;
            return;
        }
        
        if(!shieldSlider.gameObject.activeSelf) shieldSlider.gameObject.SetActive(true);

        shieldSlider.value = (float)shield.currentValue / (float)shield.maxValue;
        if(TMP_shield != null) TMP_shield.text = shield.currentValue + "/" + shield.maxValue;
    }

    // private IEnumerator Animation(float target)
    // {
    //     float elapsedTime = 0f;
    //     float startFill = hpSlider.value;

    //     while (elapsedTime < 0.1f)
    //     {
    //         hpSlider.value = Mathf.Lerp(startFill, target, elapsedTime/0.1f);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }
}
