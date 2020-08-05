using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ListExtensions;

public class Rewind : MonoBehaviour
{
    [System.Serializable]
    public class SerializableVector
    {
        public float x, y, z;

        public SerializableVector(Vector3 Vector)
        {
            x = Vector.x;
            y = Vector.y;
            z = Vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }

    [System.Serializable]
    public class SerializableQuaternion
    {
        public float x, y, z, w;

        public SerializableQuaternion(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }

    [System.Serializable]
    public class TrackableData
    {
        public SerializableVector position;

        public SerializableQuaternion rotation;

        public float timeSinceStart;
    }

    [System.Serializable]
    public class DataList
    {
        public List<TrackableData> dataList;

        public DataList(List<TrackableData> data)
        {
            dataList = data;
        }

    }

    Lerper lerper;

    public float rewindTime = 0;
    private float maxRewindTime = 0;
    private float timeStartedRewinding = 0;
    private float rewindTemp = 0;

    private bool canRewind = false;
    private bool isRewinding = false;
    private bool shouldDelete = false;

    public List<TrackableData> TrackableDataList = new List<TrackableData>();
    public DataList trackedDataList = null;

    void Awake()
    {
        maxRewindTime = GameManager.instance.maxRewindTime;
        rewindTime = maxRewindTime;

        lerper = GetComponent<Lerper>();
    }

    void OnEnable()
    {
        canRewind = false;
        isRewinding = false;
        shouldDelete = false;

        rewindTemp = maxRewindTime;

        trackedDataList.dataList.Clear();
        TrackableDataList.Add(new TrackableData()
        {
            position = new SerializableVector(gameObject.transform.position),
            rotation = new SerializableQuaternion(gameObject.transform.rotation),
            timeSinceStart = 0
        });

        TrackableDataList.Add(new TrackableData()
        {
            position = new SerializableVector(gameObject.transform.position),
            rotation = new SerializableQuaternion(gameObject.transform.rotation),
            timeSinceStart = 0
        });
    }

    void Record()
    {
        if (Time.deltaTime > 0)
        {
            float tempTime = 0;
            if (TrackableDataList.Count > 0)
            {
                tempTime = Time.deltaTime + TrackableDataList[TrackableDataList.Count - 1].timeSinceStart;
            }

            TrackableDataList.Add(new TrackableData()
            {
                position = new SerializableVector(gameObject.transform.position),
                rotation = new SerializableQuaternion(gameObject.transform.rotation),
                timeSinceStart = tempTime
            });

            if (trackedDataList == null)
            {
                trackedDataList = new DataList(TrackableDataList);
            }
            else
            {
                trackedDataList.dataList.Add(TrackableDataList[TrackableDataList.Count - 1]);
            }

            if (shouldDelete)
            {
                trackedDataList.dataList.pop_front();
            }
        }
    }

    void BuildRewindBuffer()
    {
        rewindTime -= Time.deltaTime;
        rewindTemp -= Time.deltaTime;

        if (rewindTime <= 0)
        {
            if (rewindTemp <= 0)
            {
                shouldDelete = true;
            }
            
            canRewind = true;
        }
    }

    void RewindTime()
    {
        GameManager.instance.isRewinding = true;
        isRewinding = true;
        canRewind = false;

        lerper.SetDataList(trackedDataList);
        lerper.CommenceSimulation();
    }

    void StopRewindTime(float nextRewindTime)
    {
        lerper.Stop();
        lerper.isSimulating = false;
        rewindTime = nextRewindTime;
        shouldDelete = false;

        canRewind = false;
        isRewinding = false;
        GameManager.instance.isRewinding = false;
    }

    void Update()
    {
        if (!shouldDelete && !isRewinding)
        {
            BuildRewindBuffer();
        }

        if (!isRewinding && gameObject.activeSelf)
        {
            Record();
        }

        if (Input.GetMouseButtonDown(1) && canRewind && !isRewinding)
        {
            RewindTime();
            timeStartedRewinding = maxRewindTime;
        }

        if (isRewinding && !canRewind)
        {
            timeStartedRewinding -= Time.deltaTime;
            if (timeStartedRewinding <= 0)
            {
                shouldDelete = false;
                StopRewindTime(maxRewindTime);
            }
        }

        //this may get refactored later
        if (Input.GetMouseButtonUp(1) && isRewinding)
        {
            StopRewindTime(Mathf.Clamp(Math.Abs(timeStartedRewinding - maxRewindTime), 0, maxRewindTime));
        }

        //IF REWIND DOESNT WORK BLAME THIS.

        Player p;
        if (TryGetComponent<Player>(out p) || transform.parent.TryGetComponent<Player>(out p))
        {
            GameManager.instance.PlayerRewindTime = rewindTime;
        }
        else
        {
            rewindTime = GameManager.instance.PlayerRewindTime;
        }
    }
}
