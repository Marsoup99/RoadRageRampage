using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChoiceUI : MonoBehaviour
{
    public RoadChoiceBtnUI[] roadChoices;
    public GameObject text;
    
    public void ShowChoices(LEVELREWARD[] choices)
    {
        for (int i = 0; i < roadChoices.Length; i++)
        {
            if(choices[i] != LEVELREWARD.none)
            {
                roadChoices[i].gameObject.SetActive(true);
                roadChoices[i].SetText(choices[i]);
            }
        }
        text.SetActive(true);
    }
    public void ShowChoices(LEVELType type)
    {
        if(type != LEVELType.shop)
        {
            roadChoices[1].gameObject.SetActive(true);
            roadChoices[1].SetText(type);
            return;
        }

        roadChoices[0].gameObject.SetActive(true);
        roadChoices[0].SetTextShop(isRelic: true);

        roadChoices[2].gameObject.SetActive(true);
        roadChoices[2].SetTextShop(isRelic: false);
    }
    public void OnChoose(int i)
    {
        GameManager.Instance.DoneChoosingNextRoad(i);
        TurnOffAll();

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }
    
    public void EndTriggerChooseReach()
    {
        if(text.activeSelf) OnChoose(Random.Range(0,3));
        // if(chooseID == 0) PlayerCtrl.Instance.carMovement._desireDirection = Vector3.left;
        // else if(chooseID == 1) PlayerCtrl.Instance.carMovement.desireDirection = Vector3.forward;
        // else if(chooseID == 2) PlayerCtrl.Instance.carMovement.desireDirection = Vector3.right;

    }
    private void TurnOffAll()
    {
        foreach(RoadChoiceBtnUI go in roadChoices)
        {
            go.gameObject.SetActive(false);
        }
        text.SetActive(false);
    }
}
