using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using UnityEngine;

namespace TowerSurvivors.Enemies
{
    public class GroupSpawner : MonoBehaviour
    {
        public int Amount = 5;
        public float radius = 3;
        public GameObject enemyPrefab;

        //Spawns the indicated amount of enemies in a circle, then destroys itself
        void Start()
        {
            for (int i = 0; i < Amount; i++)
            {
                Vector2 point = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                point = point.normalized * radius;

                Vector3 spawnPosition = new Vector3(
                    transform.position.x + point.x, 
                    transform.position.y + point.y, 
                    transform.position.y + point.y);

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                EnemySpawner.Instance.currentEnemies++;
            }
            EnemySpawner.Instance.currentEnemies--;
            Destroy(gameObject, 1);
        }

    }
}
