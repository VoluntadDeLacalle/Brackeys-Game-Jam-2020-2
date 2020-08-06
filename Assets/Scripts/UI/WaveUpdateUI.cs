using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUpdateUI : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMesh;
    private int wave = 1;

    void Awake()
    {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();

        textMesh.text = "Wave " + GameManager.instance.currentWave;
    }

    void Update()
    {
        if (wave < GameManager.instance.currentWave && !GameManager.instance.lose)
        {
            textMesh.text = "Wave " + GameManager.instance.currentWave;
            wave = GameManager.instance.currentWave;
        }
        else if (GameManager.instance.lose)
        {
            textMesh.text = "LOSER.";
            textMesh.color = Color.red;
        }

    }
}
