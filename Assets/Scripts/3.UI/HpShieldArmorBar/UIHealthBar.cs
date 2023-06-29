using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBar : MonoBehaviour
{
    // [SerializeField] private TextMeshPro text;
    public Slider hpSlider, shieldSlider, armorSlider;
    protected HpStat hp, shield, armor;

    public virtual void SetUIBars(HpStat hp, HpStat shield, HpStat armor)
    {
        this.hp = hp;
        this.shield = shield;
        this.armor = armor;

        this.hp.onValueChange += SetHealthBarUI;
        this.shield.onValueChange += SetShieldUI;
        this.armor.onValueChange += SetArmorUI;

        SetHealthBarUI();
        SetShieldUI();
        SetArmorUI();
    }
    public virtual void SetHealthBarUI ()
    {
        hpSlider.value = (float)hp.currentValue / (float)hp.maxValue;
        // text.text = currentHealth.ToString();
    }
    public virtual void SetShieldUI ()
    {
        if(shieldSlider == null) return;
        if(shield.maxValue == 0) shieldSlider.value = 0;
        else
            shieldSlider.value = (float)shield.currentValue / (float)shield.maxValue;
    }

    public virtual void SetArmorUI ()
    {
        if(armorSlider == null) return;
        if(armor.maxValue == 0) armorSlider.value = 0;
        else
            armorSlider.value = (float)armor.currentValue / (float)armor.maxValue;
    }
}
