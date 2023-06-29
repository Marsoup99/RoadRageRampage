using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeInDuration = 2.0f;
    [SerializeField] private float fadeOutDuration = 1.0f;

    // private float currentAlpha = 0.0f;

    public void FadeIn()
    {
        blackScreen.DOFade(1.0f, fadeInDuration);
    }

    public void FadeOut()
    {
        blackScreen.DOFade(0.0f, fadeOutDuration);
    }

    public void FadeByAlpha(float alpha)
    {
        blackScreen.DOFade(alpha, fadeInDuration);
    }
}
