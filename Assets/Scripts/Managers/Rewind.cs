﻿using System.Collections;
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

    private bool canRewind = false;
    private bool shouldDelete = false;

    private List<TrackableData> TrackableDataList = new List<TrackableData>();
    private DataList trackedDataList = null;

    void Awake()
    {
        maxRewindTime = GameManager.instance.maxRewindTime;
        rewindTime = maxRewindTime;

        lerper = GetComponent<Lerper>();
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
            
        if (rewindTime <= 0)
        {
            shouldDelete = true;
            canRewind = true;
        }
    }

    void RewindTime()
    {
        GameManager.instance.isRewinding = true;

        lerper.SetDataList(trackedDataList);
        lerper.CommenceSimulation();
    }

    void StopRewindTime(float nextRewindTime)
    {
        //Debug.Log("Finished");
        lerper.Stop();
        rewindTime = nextRewindTime;
        shouldDelete = false;

        canRewind = false;
        GameManager.instance.isRewinding = false;
    }

    void Update()
    {
        if (!shouldDelete)
        {
            BuildRewindBuffer();
        }

        if (!GameManager.instance.isRewinding)
        {
            Record();
        }

        if (Input.GetMouseButtonDown(1) && canRewind)
        {
            RewindTime();
            timeStartedRewinding = maxRewindTime;

            //Debug.Log("Rewinding");
        }

        if (GameManager.instance.isRewinding)
        {
            timeStartedRewinding -= Time.deltaTime;
            if (timeStartedRewinding <= 0)
            {
                shouldDelete = false;
                StopRewindTime(maxRewindTime);
            }
        }

        //this may get refactored later
        if (Input.GetMouseButtonUp(1) && GameManager.instance.isRewinding)
        {
            StopRewindTime(Mathf.Clamp(Math.Abs(timeStartedRewinding - maxRewindTime), 0, maxRewindTime));
        }
    }
}
