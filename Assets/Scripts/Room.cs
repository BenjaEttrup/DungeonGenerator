using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Exit[] exits;
    public Transform enemySpawnPoint;
    public int procntChanceEnemySpawn;
    public GameObject enemyPrefab;
    public bool isStartRoom = false;

    private void Start()
    {
        if (!isStartRoom)
        {
            if (Random.Range(0, 101) < procntChanceEnemySpawn)
            {
                Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            }
        }
    }
}
