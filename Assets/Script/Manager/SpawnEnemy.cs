using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnEnemy : MonoBehaviour
{
    private SpawnPosition spawnPosition;

    void Start()
    {
        spawnPosition = GetComponentInChildren<SpawnPosition>();

        if (spawnPosition != null)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

    }
    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("ontrigger");
        if (player.CompareTag("Player"))
        {
            spawnPosition.SpawnEnemy();
        }
    }

}
