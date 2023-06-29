
using UnityEngine;
using DG.Tweening;

public class PropsInteract : MonoBehaviour
{
    public int damage = 10;
    public bool randomRotate = false;
    private bool interacted = false;
    private Vector3 pos;
    void OnEnable()
    {
        interacted = false;
        pos = transform.GetChild(0).localPosition;
        float rng = Random.value;
        if(rng < 0.33)
            transform.position += transform.right * 3;
        else if (rng < 0.66)
            transform.position -= transform.right * 3;
            
        if(randomRotate)
            transform.Rotate(0, Random.Range(0, 180), 0);
    }

    void OnDisable()
    {
        transform.GetChild(0).localPosition = pos;
    }
    void OnTriggerEnter(Collider col)
    {
        SoundManager.Instance?.sfxPlayer.PlayCollideSFX(transform.position);
        if(interacted) return;
        IDmgable target = col.GetComponentInParent<IDmgable>();
        if(target != null)
        {
            target.TakeDamage(damage, ELEMENT.normal);
            interacted = true;
        } 
        
        transform.DOMoveY(5f * 2 / damage, 1).SetEase(Ease.OutQuad);
    }
}
