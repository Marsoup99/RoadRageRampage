using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChoiceEndTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            GameManager.Instance.RoadChoiceUI.EndTriggerChooseReach();
        }
    }
}
