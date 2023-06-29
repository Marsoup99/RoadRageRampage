using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconUI : MonoBehaviour
{
    public MyTimer target;
    public Image cooldownImage;

    public void SetTargetTimer(MyTimer target)
    {
        this.target = target;
    }
    void LateUpdate()
    {
        cooldownImage.fillAmount = target.currentTime / target.cooldown;
    }
}
