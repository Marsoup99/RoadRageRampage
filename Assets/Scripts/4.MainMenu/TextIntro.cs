using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TextIntro : MonoBehaviour
{
    [SerializeField] private RectTransform all;
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform btm;
    [SerializeField] private Image bgImg;
    [SerializeField] private float animationDuration = 2;
    [SerializeField] private Canvas canvas;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        canvas.sortingOrder = 100;
        bgImg.color = Color.black;
        all.anchoredPosition = Vector2.zero;

        // Set initial positions
        top.anchoredPosition = Vector2.left * 1000;
        btm.anchoredPosition = Vector2.right * 1000;

        // Create the animation sequence
        Sequence sequence = DOTween.Sequence();

        // Animation: Move from left
        sequence.Append(top.DOAnchorPosX(0f, animationDuration/2).SetEase(Ease.OutQuad));

        // Animation: Move from right
        sequence.Join(btm.DOAnchorPosX(0f, animationDuration).SetEase(Ease.OutQuad));
        sequence.Append(all.DOAnchorPosY(400, animationDuration/2).SetEase(Ease.OutQuad));
        sequence.Join(bgImg.DOFade(0,animationDuration/2));

        sequence.OnComplete(() => canvas.sortingOrder = 0);
    }
}
