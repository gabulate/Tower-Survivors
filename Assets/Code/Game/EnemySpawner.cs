using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerSurvivors.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeReference]
        public List<EnemyWaveSO> waves;
        [SerializeField]
        private EnemyWaveSO currentWave;
        public float currentCooldown;
        public Vector2 min;
        public Vector2 max;

        private void Start()
        {
            currentWave = waves[0];
            currentCooldown = waves[0].cooldown;
        }

        private void Update()
        {
            currentCooldown -= Time.deltaTime;
            if(currentCooldown <= 0)
            {
                SpawnEnemies();
                currentCooldown = currentWave.cooldown;
            }
        }

        private void SpawnEnemies()
        {
            switch (currentWave.distribution)
            {
                case DistributionType.Random:
                    SpawnRandom();
                    break;
                case DistributionType.Horde:
                    SpawnHorde();
                    break;
            }
        }

        private void SpawnHorde()
        {
            throw new NotImplementedException();
        }

        private void SpawnRandom()
        {
            for (int i = 0; i < currentWave.amountAtATime; i++)
            {
                int random = Random.Range(1, 5);
                float x = 0;
                float y = 0;
                switch (random)
                {
                    case 1:
                        x = Random.Range(min.x, max.x);
                        y = min.y;
                        break;
                    case 2:
                        x = Random.Range(min.x, max.x);
                        y = max.y;
                        break;
                    case 3:
                        x = min.x;
                        y = Random.Range(min.y, max.y);
                        break;
                    case 4:
                        x = max.x;
                        y = Random.Range(min.y, max.y);
                        break;
                }

                Vector3 position = new(x, y, y);

                //Get Random enemy
                GameObject randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Count)];

                Instantiate(randomEnemy, position, Quaternion.identity);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            // Calculate the center and size of the box
            Vector3 center = new Vector3((max.x + min.x) / 2f, (max.y + min.y) / 2f, 0f);
            Vector3 size = new Vector3(max.x - min.x, max.y - min.y, 0f);

            // Draw the wireframe box
            Gizmos.DrawWireCube(center, size);
        }
    }
}
