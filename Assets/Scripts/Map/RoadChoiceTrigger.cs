using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChoiceTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            GameManager.Instance.FinishedLevel();
        }
    }
}
