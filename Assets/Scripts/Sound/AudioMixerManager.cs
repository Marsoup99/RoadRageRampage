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
    private float master_v, sfx_v, bg_v;
    void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        master_v = master.value;
        ChangeMasterVolume(master.value);
        
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        sfx_v = sfx.value;
        ChangeSFXvolume(sfx.value);

        music.value = PlayerPrefs.GetFloat("BGVolume", 1);
        bg_v = music.value;
        ChangeMusicVolume( music.value);
    }
    public void ChangeMasterVolume(float v)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20f);
        master_v = v;
        // PlayerPrefs.SetFloat("MasterVolume", v);
    }

    public void ChangeSFXvolume(float v)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(v) * 20f);
        sfx_v = v;
        // PlayerPrefs.SetFloat("SFXVolume", v);
    }

    public void ChangeMusicVolume(float v)
    {
        audioMixer.SetFloat("BGVolume", Mathf.Log10(v) * 20f);
        bg_v = v;
        // PlayerPrefs.SetFloat("BGVolume", v);
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
        PlayerPrefs.SetFloat("MasterVolume", master_v);
        PlayerPrefs.SetFloat("SFXVolume", sfx_v);
        PlayerPrefs.SetFloat("BGVolume", bg_v);
        
        TimeEffectControl.DoNormalTime();
        SettingPanelGO.SetActive(false);

        SoundManager.Instance?.PlayButtonPress();
        if(InputManagerSingleton.Instance != null)
        {
            InputManagerSingleton.Instance.isRecevingInput = true;
        }
    }
}
