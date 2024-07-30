using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public GameObject enemyType;

    public void SpawnEnemy()
    {
        Instantiate(enemyType, transform.position, transform.rotation);
    }
}
