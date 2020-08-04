using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnHelpers;

    Dictionary<GameObject, SpawnHelper> spawnDict = new Dictionary<GameObject, SpawnHelper>();

    private ObjectPooler.Key enemyKey = ObjectPooler.Key.Enemy;

    public float spawnBuffer = 0f;
    public float waveLaneIncrease = 0f;

    private bool hasWaveStarted = false;

    void Awake()
    {
        for (int i = 0; i < spawnHelpers.Length; i++)
        {
            spawnDict.Add(spawnHelpers[i], spawnHelpers[i].GetComponent<SpawnHelper>());
            spawnHelpers[i].SetActive(false);
        }
    }

    void Start()
    {
        EnableSpawnHelper();
    }

    void Update()
    {
        CheckForWaveStart();

        if (hasWaveStarted)
        {
            /*if (Time.time - spawnBuffer >= GameManager.instance.timeToNextSpawn && GameManager.instance.spawnedNumber < GameManager.instance.fib[0])
            {
                SpawnEnemy();
                GameManager.instance.spawnedNumber++;
                spawnBuffer = Time.time;
            }

            if (GameManager.instance.spawnedNumber == GameManager.instance.fib[0] && GameManager.instance.currentWaveKilledNumber == GameManager.instance.fib[0])
            {
                hasWaveStarted = false;
            }*/
        }
    }

    void CheckForWaveStart()
    {

        if ((int)(GameManager.instance.timeToNextWave - (Time.timeSinceLevelLoad - GameManager.instance.waveTimeBuffer)) <= 0 && !hasWaveStarted)
        {
            spawnBuffer = Time.time;

            if ((GameManager.instance.currentWave + 1) % waveLaneIncrease == 0)
            {
                EnableSpawnHelper();
            }

            hasWaveStarted = true;
        }
    }

    public void SpawnEnemy()
    {
        List<GameObject> activeSpawnHelpers = ActiveSpawnHelpers(true);

        int rand = Random.Range(0, activeSpawnHelpers.Count);

        GameObject pooledObj = ObjectPooler.GetPooler(enemyKey).GetPooledObject();

        pooledObj.transform.position = spawnDict[activeSpawnHelpers[rand]].spawnPoint.position;

        Quaternion targetRotation = Quaternion.LookRotation(spawnDict[activeSpawnHelpers[rand]].wayPoint.position);
        pooledObj.transform.rotation = targetRotation;

        pooledObj.SetActive(true);
        //pooledObj.GetComponent<EnemyBehavior>().FindPath(spawnDict[activeSpawnHelpers[rand]].wayPoint);
    }

    void EnableSpawnHelper()
    {
        List<GameObject> tempSpawnHelpers = ActiveSpawnHelpers(false);

        int rand = Random.Range(0, tempSpawnHelpers.Count);
        if (tempSpawnHelpers.Count != 0)
        {
            tempSpawnHelpers[rand].SetActive(true);
        }
        else
        {
            GameManager.instance.infiniteWave = true;
        }
        return;
    }

   List<GameObject> ActiveSpawnHelpers(bool checkingActive)
    {
        List<GameObject> tempSpawnHelpers = new List<GameObject>();

        for (int i = 0; i < spawnHelpers.Length; i++)
        {
            if (!spawnHelpers[i].gameObject.activeInHierarchy && !checkingActive)
            {
                tempSpawnHelpers.Add(spawnHelpers[i]);
            }
            else if (spawnHelpers[i].gameObject.activeInHierarchy && checkingActive)
            {
                tempSpawnHelpers.Add(spawnHelpers[i]);
            }
        }

        return tempSpawnHelpers;
    }
}
