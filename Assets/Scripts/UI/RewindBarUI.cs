using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindBarUI : MonoBehaviour
{
    Slider rewindBar;
    private bool isRewinding = false;
    private bool updateBarDown = false;
    private bool updateBarUp = true;
    private float rewindTime = 0f;
    
    void Awake()
    {
        rewindBar = GetComponent<Slider>();

        rewindTime = 0;
    }

    void StartOfRewind()
    {
        isRewinding = true;
        updateBarDown = true;
        rewindTime = GameManager.instance.maxRewindTime;
        
        updateBarUp = false;
    }

    void EndOfRewind()
    {
        isRewinding = false;
        updateBarDown = false;

        updateBarUp = true;
        rewindTime = 0;
    }

    void HandleRewind()
    {
        if (!isRewinding && GameManager.instance.isRewinding)
        {
            StartOfRewind();
        }

        if (isRewinding && !GameManager.instance.isRewinding)
        {
            EndOfRewind();
        }
    }

    void Update()
    {
        HandleRewind();

        if (updateBarDown)
        {
            rewindTime -= Time.deltaTime;

            rewindBar.value = Remap(rewindTime, 0, GameManager.instance.maxRewindTime, 0, 1);
        }

        if (updateBarUp && rewindTime <= GameManager.instance.maxRewindTime)
        {
            rewindTime += Time.deltaTime;

            rewindBar.value = Remap(rewindTime, 0, GameManager.instance.maxRewindTime, 0, 1);
        }
    }

    float Remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
}
