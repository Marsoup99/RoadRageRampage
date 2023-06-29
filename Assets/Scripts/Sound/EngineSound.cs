using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    public CarMovement carMovement;
    public AudioSource source;
    void Start()
    {
        carMovement = transform.parent.GetComponentInChildren<CarMovement>();
        if(carMovement == null) Destroy(this.gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        source.pitch = carMovement.speed / 30;
    }
}
