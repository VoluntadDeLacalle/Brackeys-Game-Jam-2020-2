using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    float timeStart;

    public void OnEnable()
    {
        timeStart = Time.time;
    }

    void Update()
    {
        if (timeStart + 10f <= Time.time)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.instance.currentWaveKilledNumber++;
        GameManager.instance.currentlyspawnedNumber--;

        gameObject.SetActive(false);
    }
}
