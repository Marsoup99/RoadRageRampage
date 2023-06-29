using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InGameTutorial : MonoBehaviour
{
    public Image fingerImg;
    public TextMeshProUGUI tmp;
    public GameObject dummy;
    private bool metCondition = false;
    [Header("Moving left right")]
    public GameObject LRGO;
    public string textLR = "Swipe left or right to change lane";
    [Header("Nitro boost")]
    public GameObject nitroGO;
    public string textNitro = "Swipe up to use nitro";
    public string textNitro_2 = "When using nitro you deal high collide damage and take no damage from that impact";
    // public string text3 = "This icon show the nitro cooldown process";
    [Header("Brake")]
    public GameObject BrakeGO;
    public string textBrake = "Swipe down to slow down";
    public string textBrake_2 = "Remember that if you slow down too much your car might explode";
    [Header("Shoot")]
    public string textShoot = "Tap or hold down to shot bulet";
    public string textShoot_2 = "You can get better weapon and armor on the run";
    public string textShoot_3 = "Have fun.";
    private PlayerCarMovement player;
    public void TutorialStart()
    {
        
        player = PlayerCtrl.Instance?.GetComponentInChildren<PlayerCarMovement>();
        if(player == null) 
        {
            //END Tutorial.
            EndTutorial();
        }
        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        //tutorial moving left right
        metCondition = false;
        fingerImg.gameObject.SetActive(true);
        tmp.text = textLR;
        LRGO.SetActive(true);

        // player.MovingLeftRightTutorialInput();
        InputManagerSingleton.Instance.onSwipeRight += MeetCondition;
        InputManagerSingleton.Instance.onSwipeLeft += MeetCondition;
        while(!metCondition)
        {
            yield return new WaitForSeconds(0.5f);
        }
        InputManagerSingleton.Instance.onSwipeRight -= MeetCondition;
        InputManagerSingleton.Instance.onSwipeLeft -= MeetCondition;

        yield return new WaitForSeconds(2);
        LRGO.SetActive(false);

        //tutorial nitro
        // player.NitroAndBrakeTutorial();
        metCondition = false;
        fingerImg.gameObject.SetActive(true);
        tmp.text = textNitro;
        nitroGO.SetActive(true);

        InputManagerSingleton.Instance.onSwipeUp += MeetCondition;
        while(!metCondition)
        {
            yield return new WaitForSeconds(0.5f);
        }
        InputManagerSingleton.Instance.onSwipeUp -= MeetCondition;

        yield return new WaitForSeconds(2);
        nitroGO.SetActive(false);

        // tutorial nitro 2
        fingerImg.gameObject.SetActive(false);
        tmp.text = textNitro_2;
        yield return new WaitForSeconds(5);

        //tutorial brake
        metCondition = false;
        fingerImg.gameObject.SetActive(true);
        tmp.text = textBrake;
        BrakeGO.SetActive(true);
        InputManagerSingleton.Instance.onSwipeDown += MeetCondition;
        while(!metCondition)
        {
            yield return new WaitForSeconds(0.5f);
        }
        InputManagerSingleton.Instance.onSwipeDown -= MeetCondition;
        yield return new WaitForSeconds(2);
        BrakeGO.SetActive(false);

        //tutorial brake 2
        tmp.text = textBrake_2;
        fingerImg.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);

        //tutorial shoot
        metCondition = false;
        fingerImg.gameObject.SetActive(true);
        tmp.text = textShoot;
        InputManagerSingleton.Instance.onTap += MeetCondition;
        while(!metCondition)
        {
            yield return new WaitForSeconds(0.5f);
        }
        InputManagerSingleton.Instance.onTap -= MeetCondition;
        yield return new WaitForSeconds(2);

        fingerImg.gameObject.SetActive(false);
        tmp.text = textShoot_2;
        yield return new WaitForSeconds(2);

        tmp.text = textShoot_3;
        yield return new WaitForSeconds(2);

        EndTutorial();
    }
    private void MeetCondition()
    {
        metCondition = true;
    }

    private void EndTutorial()
    {
        if(SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.mainGameDataSO.didPlayTutorial = true;
            SaveLoadManager.Instance.SaveMainGame();
        }
        GameManager.Instance.GameStart();
        transform.gameObject.SetActive(false);

        Destroy(transform.gameObject, 2);
    }
}
