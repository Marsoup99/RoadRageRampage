using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl : MonoBehaviour
{
    [SerializeField] private ToxicArrow parrent;
    void Reset()
    {
        parrent = GetComponentInParent<ToxicArrow>();
    }
    

    public void ReloadArrow()
    {
        parrent.ReloadArrow();
    }
}
