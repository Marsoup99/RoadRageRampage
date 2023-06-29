using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    void Awake()//Set Instance.
    {
        if (Instance != null && Instance != this) 
        { 
            Debug.LogWarning("there more than one SoundManager");
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
    }

    public SFXManager sfxPlayer;
    public BackgroundMusic bgPlayer;
    [Header("UI Sound")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip ButtonPressSFX;

    void Reset()
    {
        sfxPlayer = GetComponentInChildren<SFXManager>();
        bgPlayer = GetComponentInChildren<BackgroundMusic>();
        source = GetComponent<AudioSource>();
    }
    public void PlayButtonPress()
    {
        source.PlayOneShot(ButtonPressSFX);
    }
}
