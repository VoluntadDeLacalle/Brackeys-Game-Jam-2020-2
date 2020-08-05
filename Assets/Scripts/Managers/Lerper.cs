using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ListExtensions;

public class Lerper : MonoBehaviour
{
    private Rewind.DataList data = null;

    public void SetDataList(Rewind.DataList rewindDataList)
    {
        data = rewindDataList;
    }

    private int count = 0;
    private float simulationTime;
    public bool isSimulating = false;

    public void CommenceSimulation()
    {
        count = data.dataList.Count;
        simulationTime = data.dataList[data.dataList.Count - 1].timeSinceStart;
        isSimulating = true;
    }

    private void Simulate()
    {
        simulationTime += Time.deltaTime;

        Rewind.TrackableData currentTrackableData = data.dataList[data.dataList.Count - 1];
        Rewind.TrackableData previousTrackableData = null;

        while (currentTrackableData.timeSinceStart > simulationTime)
        {
            count--;

            if (count <= 0)
            {
                count = data.dataList.Count;
                simulationTime = data.dataList[data.dataList.Count - 1].timeSinceStart;
            }

            currentTrackableData = data.dataList[count];
        }

        Vector3 currentPos = currentTrackableData.position.ToVector();
        Quaternion currentRot = currentTrackableData.rotation.ToQuaternion();

        if (count < data.dataList.Count - 1)
        {
            previousTrackableData = data.dataList[count + 1];

            currentPos = Vector3.Lerp(currentTrackableData.position.ToVector(), previousTrackableData.position.ToVector(), previousTrackableData.timeSinceStart - simulationTime);
            currentRot = Quaternion.Lerp(currentTrackableData.rotation.ToQuaternion(), previousTrackableData.rotation.ToQuaternion(), previousTrackableData.timeSinceStart - simulationTime);
        }

        gameObject.transform.position = currentPos;
        gameObject.transform.rotation = currentRot;

        count++;

        if (count <= 0)
        {
            count = data.dataList.Count;
            simulationTime = data.dataList[data.dataList.Count - 1].timeSinceStart;
            isSimulating = false;
        }

        data.dataList.pop_back();
    }

    public void Stop()
    {
        if (data != null)
        {
            if (data.dataList.Count > 0)
            {
                count = data.dataList.Count;
                simulationTime = data.dataList[data.dataList.Count - 1].timeSinceStart;
                isSimulating = false;
            }
        }
    }

    void Update()
    {
        if (isSimulating && data.dataList.Count > 0)
        {
            Simulate();
        }
    }
}
