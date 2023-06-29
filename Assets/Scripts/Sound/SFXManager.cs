using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] ExplosiveSFX;
    [SerializeField] private AudioClip[] CollideSFX;
    [SerializeField] private AudioClip DefaultWarningSound;
    [SerializeField] private AudioClip[] DefaultBulletHit;

    private List<AudioSource> sourcesPool = new List<AudioSource>();
   

    public void PlayExplosiveSFX(Vector3 pos)
    {
        // SpawnObject(sours)
        PlayAudioAtPos(ExplosiveSFX[Random.Range(0, ExplosiveSFX.Length)], pos);
    }

    public void PlayCollideSFX(Vector3 pos)
    {
        PlayAudioAtPos(CollideSFX[Random.Range(0, CollideSFX.Length)], pos);
    }
    public AudioSource PlayAudioAtPos(AudioClip clip, Vector3 pos)
    {
        AudioSource tmp;
        if(sourcesPool.Count > 0)
        {
            tmp = sourcesPool[0];
            tmp.gameObject.SetActive(true);
            sourcesPool.RemoveAt(0);
        }    
        else
        {
            tmp = Instantiate(source, transform);
        }
        tmp.clip = clip;
        tmp.transform.position = pos;
        tmp.Play();
        StartCoroutine(BackToPool(tmp, tmp.clip.length));
        return tmp;

        // AudioSource tmp = Instantiate(source, transform);
        // tmp.clip = clip;
        // tmp.transform.position = pos;
        // tmp.Play();
        // Destroy(tmp, tmp.clip.length);
        // return tmp;
        // Invoke(BackToPool, tmp.clip.length);
    }
    public AudioSource PlayAudioAtPos(AudioClip clip, Vector3 pos, Transform parent)
    {
        AudioSource tmp = PlayAudioAtPos(clip, pos);
        tmp.transform.parent = parent;
        return tmp;
        // Invoke(BackToPool, tmp.clip.length);
    }

    private IEnumerator BackToPool(AudioSource tmp, float time)
    {
        yield return new WaitForSeconds(time);
        tmp.transform.parent = this.transform;
        sourcesPool.Add(tmp);
        tmp.gameObject.SetActive(false);
    }

    public AudioSource PlayWarningSound(AudioClip clip, Transform parent)
    {
        if(clip == null)
            return PlayAudioAtPos(DefaultWarningSound, parent.position, parent);
        else return PlayAudioAtPos(clip, parent.position, parent);
    }

    public void PlayBulletHitSFX(AudioClip clip, Vector3 pos)
    {
        if(clip == null)
        {
            PlayAudioAtPos(DefaultBulletHit[Random.Range(0, DefaultBulletHit.Length)], pos);
        }
        else PlayAudioAtPos(clip, pos);
    }
}
