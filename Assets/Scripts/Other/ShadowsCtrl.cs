using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsCtrl : MonoBehaviour
{
    public SpriteRenderer sprite;

    void Reset()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if(sprite.enabled && transform.position.y > 1)
        {
            sprite.enabled = false;
        }
        else if(!sprite.enabled && transform.position.y < 1) sprite.enabled = true;
    }
}
