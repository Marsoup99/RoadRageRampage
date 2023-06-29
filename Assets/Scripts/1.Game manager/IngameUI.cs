using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IngameUI : MonoBehaviour
{
    [Header("Lose, win pannel")]
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;
    [SerializeField] private Image BGimg;
    [SerializeField] private GameObject runInfoPannel;
    [SerializeField] private TextMeshProUGUI smallcars, miniboss, boss, money;
    [SerializeField] private GameObject tryAgianBtn, menuBtn;
    private bool isOpen = false;
    public void QuickRestart()
    {
        SaveLoadManager.Instance?.ClearInGameData();
        GameManager.Instance.GameStart();

        if(LoadingScenesManager.Instance != null)
            StartCoroutine(LoadingScenesManager.Instance.LoadingBG());
        if(holder.activeSelf) holder.SetActive(false);
    }
    public void MainMenu()
    {
        SaveLoadManager.Instance?.ClearInGameData();
        LoadingScenesManager.Instance?.LoadScene(0);
    }

    public void OpenFinishPannel(bool WON)
    {
        if(isOpen) return;
        isOpen = true;
        tryAgianBtn.SetActive(false);
        menuBtn.SetActive(false);
        runInfoPannel.SetActive(false);
        loseText.SetActive(false);
        winText.SetActive(false);
        StartCoroutine(LosePannelAnimation(WON));
    }

    private IEnumerator LosePannelAnimation(bool WON)
    {
        (int smallCars, int miniBoss, int bigBoss) = GameManager.Instance.InGameData.GetEnemyKilledNumber();
        int targetMoney = smallCars * 20 + miniBoss * 100 + bigBoss * 500;

        if(SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.mainGameDataSO.money += targetMoney;
            SaveLoadManager.Instance.SaveMainGame();

            SaveLoadManager.Instance.ClearInGameData();
        }
        holder.SetActive(true);
        BGimg.DOFade(0,0);
        BGimg.DOFade(1,1);
        yield return new WaitForSeconds(1);

        loseText.SetActive(!WON);
        winText.SetActive(WON);
        yield return new WaitForSeconds(1);
        runInfoPannel.SetActive(true);
        smallcars.text = "x" + 0.ToString();
        miniboss.text = "x" + 0.ToString();
        boss.text = "x" + 0.ToString();
        money.text = "x" + 0.ToString();

        yield return null;

        float time = 0;
        while (time < 2)
        {
            smallcars.text = "x" + Mathf.Lerp(0, smallCars, time).ToString("0");
            miniboss.text = "x" + Mathf.Lerp(0, miniBoss, time).ToString("0");
            boss.text = "x" + Mathf.Lerp(0, bigBoss, time).ToString("0");
            time += Time.deltaTime;
            yield return null;
        }

        smallcars.text = "x" + smallCars;
        miniboss.text = "x" + miniBoss;
        boss.text = "x" + bigBoss;

        time = 0;
        while (time < 1)
        {
            money.text = "x" + Mathf.Lerp(0,targetMoney, time).ToString("0");
            time += Time.deltaTime;
            yield return null;
        }
        isOpen = false;
        money.text = "x" + targetMoney.ToString();
        tryAgianBtn.SetActive(!WON);
        menuBtn.SetActive(true);
    }
}
