using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public int volumn = 1;
    public AudioSource source;

    public List<AudioClip> BGMusic;
    void Reset()
    {
        source = GetComponent<AudioSource>();
    }
    void Start()
    {
        PlayBG(0);
    }
    public void PlayBG(int index)
    {
        source.clip = BGMusic[index];
        source.Play();
    }

    public void PlayBossMusic(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    public void StopMusic()
    {
        source.Stop();
    }
}
