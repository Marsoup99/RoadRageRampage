using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance { get; private set; }
    
    private float _fov;
    private Camera mainCamera;
    private Tweener dotween;
    public Vector3 camPosition;
    public Vector3 camEulers;
    [Header("Shake setting")]
    public float shakeDuration = 0.1f;      // Duration of the camera shake
    public float shakeIntensity = 0.1f; 
    void Awake()//Set Instance.
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one CameraControl");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        mainCamera = Camera.main;
        _fov = mainCamera.fieldOfView;
    }

    public void CameraStart()
    {
        InputManagerSingleton.Instance.isRecevingInput = false;
        TimeEffectControl.DoSlowMotion(0.8f);
        transform.DOLocalMove(camPosition, 1).SetEase(Ease.OutQuad);
        transform.DORotate(camEulers, 1).OnComplete(() => {
            InputManagerSingleton.Instance.isRecevingInput = true;
            TimeEffectControl.DoNormalTime();
        });
    }
    public void NitroCameraFeel(float nitroDuration)
    {
        dotween = mainCamera.DOFieldOfView(_fov + 20, nitroDuration).OnComplete(
            () => StopNitroCameraFeel());
    }

    public void StopNitroCameraFeel()
    {
        if(dotween.IsPlaying()) dotween.Kill();
        mainCamera.DOFieldOfView(_fov, 1);
    }
    public void CameraShake()
    {
        transform.DOShakePosition(shakeDuration, shakeIntensity).SetEase(Ease.OutQuad)
                                    .OnComplete(() => transform.localPosition = camPosition);
    }
}
