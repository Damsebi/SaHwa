using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnEnemy : MonoBehaviour
{
    public static SpawnEnemy Instance { get; private set; }
    private SpawnPosition spawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void TriggerSpawn()
    {
        spawnPosition.SpawnEnemy();
    }

    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("ontrigger");
        if (player.CompareTag("Player"))
        {
            TriggerSpawn();
        }
    }

}
