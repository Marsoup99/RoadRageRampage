using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoadingScenesManager : MonoBehaviour
{
    public int sceneToLoad = 1; // Index of the scene to load
    [Header("Loading screen")]
    public GameObject loadingCanvas;
    public Image loadingBG;
    public TextMeshProUGUI loadingText;
    public float dotFadeDuration = 0.5f;         // Duration of each dot fade animation
    public float dotFadeDelay = 0.5f;
    private string loadingStr = "LOADING";
    private int _dotCount = 0;

    public static LoadingScenesManager Instance { get; private set; }
    void Awake()//Set Instance.
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one LoadingScenesManager");
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
    }

    public void LoadScene(int id)
    {
        sceneToLoad = id;
        StartCoroutine(LoadSceneAsync());
    }
    public void LoadScene() {this.LoadScene(1);}

    private IEnumerator LoadSceneAsync()
    {
        loadingBG.DOFade(1, 0.25f);
        yield return new WaitForSeconds(0.25f); // Wait for one frame to ensure the loading screen is displayed
        loadingCanvas.SetActive(true);
        StartDotAnimation();

        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            // Update progress bar or other loading indicators
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // Normalize progress to 0-1 range
            Debug.Log("Loading progress: " + progress);

            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        CancelInvoke();
        asyncOperation.allowSceneActivation = true;
        yield return null;

        loadingCanvas.SetActive(false);
        loadingBG.DOFade(0, 0.5f);
        
        //Play bgmusic
        SoundManager.Instance?.bgPlayer.PlayBG(sceneToLoad);
    }
    private void StartDotAnimation()
    {
        InvokeRepeating("AnimateDot", dotFadeDelay, dotFadeDuration + dotFadeDelay);
    }

    private void AnimateDot()
    {
        _dotCount++;
        _dotCount = _dotCount % 4;

        string dots = new string('.', _dotCount);
        loadingText.text = loadingStr + dots;
    }

    public IEnumerator LoadingBG()
    {
        yield return null;
        loadingCanvas.SetActive(true);
        loadingBG.DOFade(1, 0f);
        StartDotAnimation();
        yield return new WaitForSeconds(0.5f); // Wait for one frame to ensure the loading screen is displayed
        CancelInvoke();
        loadingCanvas.SetActive(false);
        loadingBG.DOFade(0, 0.5f);
    }

}
