using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackGround : MonoBehaviour
{
    public Material mat;
    public float speed = 5;

    private float x = 0;
    void Reset()
    {
        mat = GetComponent<Material>();
    }
    void LateUpdate()
    {
        x += speed * Time.deltaTime;
        mat.mainTextureOffset = Vector2.right * x;
    }
}
