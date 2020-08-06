using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Player player;

    public bool isRewinding = false;
    public bool changeWave = false;
    public bool lose = false;
    public bool infiniteWave = true;

    public float maxTimeToNextSpawn = 0f;
    public float minTimeToNextSpawn = 0f;
    [HideInInspector] public float timeToNextSpawn = 0f;
    public float timeToNextWave = 0f;
    [HideInInspector] public float waveTimeBuffer = 0f;

    public float maxRewindTime = 0f;
    [HideInInspector] public float PlayerRewindTime = 0f;

    private const float waveNumber1 = 0.000058f;
    private const float waveNumber2 = 0.074032f;
    private const float waveNumber3 = 0.718119f;
    private const float waveNumber4 = 14.738699f;

    public int maxNumberToSpawn = 0;
    public int maxCanSpawnAtOnce = 0;

    [Header("Don't touch these.")]
    public int currentlyspawnedNumber = 0;
    public int totalSpawnedNumber = 0;
    public int currentWaveKilledNumber = 0;
    public int currentWave = 1;

    public int maxAmmo = 3;
    public int currentAmmo = 0;
    
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

        GetNumberToSpawn();
    }

    void GetNumberToSpawn()
    {
        if (currentWave == 1 || currentWave == 2)
        {
            maxNumberToSpawn = 2;
        }
        else if (currentWave > 2 && currentWave < 10)
        {
            maxNumberToSpawn = maxNumberToSpawn + (maxNumberToSpawn / 2); 
        }
        else
        {
            maxNumberToSpawn = (int)(waveNumber1 * Mathf.Pow(currentWave, 3) + waveNumber2 * Mathf.Pow(currentWave, 2) + waveNumber3 * currentWave + waveNumber4);
        }
    }

    IEnumerator WaveComplete()
    {
        yield return 0;

        currentWaveKilledNumber = 0;
        currentlyspawnedNumber = 0;
        totalSpawnedNumber = 0;

        waveTimeBuffer = Time.time;

        changeWave = false;
        currentWave++;
        GetNumberToSpawn();
    }

    void Update()
    {
        if (currentWaveKilledNumber == maxNumberToSpawn && !changeWave)
        {
            changeWave = true;
            StartCoroutine(WaveComplete());
        }
    }
}
