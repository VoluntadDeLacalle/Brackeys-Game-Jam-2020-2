using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool isRewinding = false;
    public bool infiniteWave = true;

    public float timeToNextSpawn = 0f;
    public float timeToNextWave = 0f;
    public float waveTimeBuffer = 0f;

    private const float waveNumber1 = 0.000058f;
    private const float waveNumber2 = 0.074032f;
    private const float waveNumber3 = 0.718119f;
    private const float waveNumber4 = 14.738699f;

    public int maxNumberToSpawn = 0;
    public int currentlyspawnedNumber = 0;
    public int currentWaveKilledNumber = 0;
    public int currentWave = 0;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void GetNumberToSpawn()
    {

    }
}
