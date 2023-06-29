using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffectControl : MonoBehaviour
{
    public static float slowDownFactor = 0.1f; // Adjust this value to control the slow motion effect

    public static void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        // Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public static void DoSlowMotion(float factor)
    {
        Time.timeScale = factor;
        // Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public static void DoNormalTime()
    {
        Time.timeScale = 1f;
        // Time.fixedDeltaTime = 0.02f;
    }
}
