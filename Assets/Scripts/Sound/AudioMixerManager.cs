using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider master, sfx, music; 
    [SerializeField] private GameObject SettingPanelGO;
    void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        ChangeMasterVolume(master.value);
        
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        ChangeSFXvolume(sfx.value);

        music.value = PlayerPrefs.GetFloat("BGVolume", 1);
        ChangeMusicVolume( music.value);
    }
    public void ChangeMasterVolume(float v)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", v);
    }

    public void ChangeSFXvolume(float v)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", v);
    }

    public void ChangeMusicVolume(float v)
    {
        audioMixer.SetFloat("BGVolume", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("BGVolume", v);
    }

    public void OpenSetting()
    {
        TimeEffectControl.DoSlowMotion(0);
        SettingPanelGO.SetActive(true);

        SoundManager.Instance?.PlayButtonPress();
        if(InputManagerSingleton.Instance != null)
        {
            InputManagerSingleton.Instance.isRecevingInput = false;
        }
    }

    public void CloseSetting()
    {
        TimeEffectControl.DoNormalTime();
        SettingPanelGO.SetActive(false);

        SoundManager.Instance?.PlayButtonPress();
        if(InputManagerSingleton.Instance != null)
        {
            InputManagerSingleton.Instance.isRecevingInput = true;
        }
    }
}
