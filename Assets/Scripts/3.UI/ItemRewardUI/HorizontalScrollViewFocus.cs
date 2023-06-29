using System.Collections;
using UnityEngine;
// using DG.Tweening;

public class HorizontalScrollViewFocus : MonoBehaviour
{
    public RectTransform rectTransform;
    public float childWidth = 300;
    public float horizontalSpace = -50;
    private float _maxWidth, _uiWidth;
    private int _activateChildCount;
    private Coroutine _coroutine;

    void OnEnable()
    {
        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            _uiWidth = rectTransform.rect.width;
        }
        rectTransform.anchoredPosition = Vector2.zero;
        _activateChildCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.gameObject.activeSelf)
            {
                _activateChildCount++;
            }
        }
        
        _maxWidth = _activateChildCount * (childWidth + horizontalSpace) + childWidth / 2 - _uiWidth + 2*horizontalSpace;
    }
    public void SelectedIndex(int id)
    {
        if(_coroutine != null) StopCoroutine(_coroutine);
        float x = id * (childWidth + horizontalSpace) + childWidth / 2 - _uiWidth/2;
        
        if( x < 0)
        {
            // rectTransform.anchoredPosition = Vector2.zero;
            // rectTransform.DOAnchorPosX(0, 0.2f);
            _coroutine = StartCoroutine(Animation(rectTransform.anchoredPosition, Vector2.zero));
            return;
        }
        if( x > _maxWidth)
        {
            // rectTransform.anchoredPosition = Vector2.left * _maxWidth;
            // rectTransform.DOAnchorPosX(-_maxWidth, 0.2f);
            _coroutine = StartCoroutine(Animation(rectTransform.anchoredPosition, Vector2.left * _maxWidth));
            return;
        }
        // rectTransform.anchoredPosition = Vector2.left * (x - _uiWidth/2);
        // rectTransform.DOAnchorPosX(-x + _uiWidth/2, 0.2f);
        _coroutine = StartCoroutine(Animation(rectTransform.anchoredPosition, Vector2.left * x));
    }

    IEnumerator Animation(Vector2 startVector, Vector2 target)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration

            // Interpolate the Vector2 values using Mathf.Lerp
            rectTransform.anchoredPosition = Vector2.Lerp(startVector, target, elapsedTime / 0.2f);

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.unscaledDeltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Make sure the final value is set to the target vector to ensure accuracy
        rectTransform.anchoredPosition = target;
        _coroutine = null;
    }
}
