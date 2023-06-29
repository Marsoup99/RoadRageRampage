using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    // [SerializeField] private List<DebuffStatus> debuffEffectList;
    [SerializeField] private CarCtr _carCtrl;
    [SerializeField] public bool isBurn = false;
    [SerializeField] public bool isElectrocute = false;
    [SerializeField] public bool isDecay = false;
    // Start is called before the first frame update
    // void Start()
    // {
    //     debuffEffectList = new List<DebuffStatus>();
    // }
    void Reset()
    {
        _carCtrl = GetComponentInParent<CarCtr>();
    }
    void OnEnable()
    {
        // if(debuffEffectList != null) debuffEffectList.Clear();
        isBurn = false;
        isElectrocute = false;
        isDecay = false;
    }

    // Update is called once per frame
    public void ApplyDebuffEffect(ELEMENT type, int dmg)
    {
        switch (type)
        {
            case ELEMENT.fire: 
            {
                if(isBurn) GetComponentInChildren<FireDebuff>().ResetDebuff();
                else 
                {
                    isBurn = true;
                    DebuffEffectSpawner.Instance.ApllyEffect(_carCtrl, type, dmg);
                }
                break;
            }
            case ELEMENT.electro: 
            {
                if(isElectrocute) GetComponentInChildren<ElectroDebuff>().ResetDebuff();
                else 
                {
                    isElectrocute = true;
                    DebuffEffectSpawner.Instance.ApllyEffect(_carCtrl, type, dmg);
                }
                break;
            }
            case ELEMENT.toxic: 
            {
                if(isDecay) GetComponentInChildren<ToxicDebuff>().ResetDebuff();
                else 
                {
                    isDecay = true;
                    DebuffEffectSpawner.Instance.ApllyEffect(_carCtrl, type, dmg);
                }
                break;
            }
            case ELEMENT.normal:
            {
                DebuffEffectSpawner.Instance.ApllyEffect(_carCtrl, type, dmg);
                break;
            }
            default: break;
        }
        

    }
}
