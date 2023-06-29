using UnityEngine;

public enum TOUCHData
{
    none,
    tap,
    swipeLeft,
    swipeRight,
    swipeUp,
    swipeDown
}
public class MyTouch
{
    public int touchID;
    private Vector2 startTouchPos, swipeDelta;
    private bool isSwipe = false;
    private TOUCHData state = TOUCHData.none;
    private float _touchBeginTime;
    private float _tapTime = 0.2f;
    // Start is called before the first frame update
    public MyTouch(int newTouchID)
    {
        touchID = newTouchID;
    }
    public void TouchBegin(Vector2 startPos)
    {
        startTouchPos = startPos;
        swipeDelta = Vector2.zero;
        isSwipe = false;

        if(_touchBeginTime + _tapTime < Time.time)
        {
            state = TOUCHData.none;
        }
        _touchBeginTime = Time.time;
        
    }
    public TOUCHData TouchEnd()
    {
        if(!isSwipe && _touchBeginTime + _tapTime > Time.time)
        {
            state = TOUCHData.tap;
            return state;
        }
        state = TOUCHData.none;
        return state;
    }

    public TOUCHData TouchMove (Vector2 curentTouchPos)
    {
        if(isSwipe) return TOUCHData.none;
        
        swipeDelta = curentTouchPos - startTouchPos;

        if (swipeDelta.magnitude > 20)
        {
            //Which direction?
            isSwipe = true;
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if (x < 0)
                    state = TOUCHData.swipeLeft;
                else
                    state = TOUCHData.swipeRight;
            }
            else
            {
                //Up or Down
                if (y < 0)
                    state = TOUCHData.swipeDown;
                else
                    state = TOUCHData.swipeUp;
            }
        }
        return state;
    }
    public bool TouchHold()
    {
        if(isSwipe == true) return false;
        if(state == TOUCHData.tap | _touchBeginTime + _tapTime < Time.time)
            return true;
        else return false;
    }
}
