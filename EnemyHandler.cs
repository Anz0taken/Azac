using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public static float ENEMY_Z = -3;
    private List<GameObject> EnemyList = new List<GameObject>();

    public GameObject Enemy;
    Movement playerMovement;

    private int NumberOfEnemyToSpawn;
    private float LastTimeSpawn;
    private float SpawingEnemyTimeLow;
    private float SpawingEnemyTimeHigh;
    private float SpawingTimeEnemies;

    void Start()
    {
        playerMovement = FindObjectOfType<Movement>();
        NumberOfEnemyToSpawn = 1;
        SpawingEnemyTimeLow = 3;
        SpawingEnemyTimeHigh = 7;
        SpawnEnemies();
    }

    void Update()
    {
        if (Time.time - LastTimeSpawn > SpawingTimeEnemies)
            SpawnEnemies();

        EnemyList.RemoveAll(segment =>
        {
            if ((CalculateDistanceToSegment(segment.transform.position, playerMovement.transform.position) > 30))
            {
                Destroy(segment);
                return true;
            }
            return false;
        });
    }

    private void SpawnEnemies()
    {
        LastTimeSpawn = Time.time;
        SpawingTimeEnemies = Random.Range(SpawingEnemyTimeLow, SpawingEnemyTimeHigh);

        for (int i = 0; i < NumberOfEnemyToSpawn; i++)
        {
            InstantiateEnemy(Enemy, playerMovement.transform.position.x + 11, playerMovement.transform.position.y + (float)4.3);
        }
    }

    private void InstantiateEnemy(GameObject prefab, float x, float y)
    {
        GameObject Enemy = Instantiate(prefab, new Vector3(x, y, ENEMY_Z), Quaternion.identity);
        EnemyList.Add(Enemy);
    }

    float CalculateDistanceToSegment(Vector3 segmentPosition, Vector3 playerPosition)
    {
        float distance = Vector2.Distance(new Vector2(playerPosition.x, playerPosition.y), new Vector2(segmentPosition.x, segmentPosition.y));
        return distance;
    }
}
