using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnEnemy : MonoBehaviour,IEvent
{
    private List<SpawnPosition> spawnPositions;

    void Start()
    {
        spawnPositions = new List<SpawnPosition>(GetComponentsInChildren<SpawnPosition>());

        //statue 비활성화
        if (spawnPositions != null)
        {
            foreach (SpawnPosition spawnPosition in spawnPositions)
            {
                foreach (Transform child in spawnPosition.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void TriggerEvent()
    {
        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            spawnPosition.SpawnEnemy();
        }
    }
}
