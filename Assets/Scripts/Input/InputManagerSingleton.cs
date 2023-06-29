using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InputManagerSingleton : MonoBehaviour
{
    public static InputManagerSingleton Instance { get; private set; }

    // [Header("Event action for input")]
    public event Action onSwipeLeft;
    public event Action onSwipeRight;
    public event Action onSwipeUp;
    public event Action onSwipeDown;
    public event Action onTap;

    public bool isRecevingInput = true;

    [Header("TOUCH")]
    public int howManyTouchInput = 2;
    private MyTouch[] _myTouches;
    private Touch _t;
    void Awake()
    {
         // If there is an instance, and it's not this, delete itself.
        if (Instance != null && Instance != this) 
        { 
            Debug.Log("there more than one InputManagerSingleton");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        AssignTouches();
    }
    public void AssignTouches()
    {
        _myTouches = new MyTouch[howManyTouchInput];
        for (int id = 0; id < howManyTouchInput; id++)
        {
            _myTouches[id] = new MyTouch(id);
        }
    }
    // Update is called once per frame
    void Update()
    {   
        if(!isRecevingInput) return;
        FromKeyBoard();
        FromFinger();
    }

    private void FromKeyBoard()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            onSwipeLeft?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            onSwipeRight?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            onSwipeUp?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            onSwipeDown?.Invoke();
        }
        if(Input.GetKey(KeyCode.Space))
        {
            onTap?.Invoke();
        }
    }

    private void FromFinger()
    {
        for(int i = 0; i < howManyTouchInput; i++)
        {
            if(Input.touchCount <= i) return;
            _t = Input.touches[i];
            if(_t.phase == TouchPhase.Began) _myTouches[i].TouchBegin(_t.position);
            else if(_t.phase == TouchPhase.Moved) DecodeTouchData(_myTouches[i].TouchMove(_t.position));
            else if(_t.phase == TouchPhase.Stationary) 
            {
                if(_myTouches[i].TouchHold()) onTap?.Invoke();
            }
            else if(_t.phase == TouchPhase.Ended) DecodeTouchData(_myTouches[i].TouchEnd());
        }
    }

    private void DecodeTouchData(TOUCHData touchData)
    {
        switch(touchData)
        {
            case TOUCHData.tap:
                onTap?.Invoke();
                break;
            case TOUCHData.swipeLeft:
                onSwipeLeft?.Invoke();
                break;
            case TOUCHData.swipeRight:
                onSwipeRight?.Invoke();
                break;
            case TOUCHData.swipeUp:
                onSwipeUp?.Invoke();
                break;
            case TOUCHData.swipeDown:
                onSwipeDown?.Invoke();
                break;
        }
    }
}
