using System;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerSurvivors.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeReference]
        public List<EnemyWaveSO> waves;
#if UNITY_EDITOR
        private List<EnemyWaveSO> previousWaves;

        private void OnValidate()
        {
            if (previousWaves == null || !ListEquals(previousWaves, waves))
            {
                totalDuration = 0;
                for (int i = 0; i < waves.Count; i++)
                {
                    totalDuration += waves[i].duration;
                }

                // Update the previousWaves
                previousWaves = new List<EnemyWaveSO>(waves);
            }
        }

        private bool ListEquals(List<EnemyWaveSO> list1, List<EnemyWaveSO> list2)
        {
            if (list1 == null || list2 == null)
            {
                return list1 == list2;
            }

            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }

            return true;
        }
#endif

        [SerializeField, ShowOnly]
        private float totalDuration = 0;
        [SerializeField]
        private EnemyWaveSO currentWave;
        [SerializeField]
        private int currentWaveIndex = 0;
        [SerializeField]
        private float currentCooldown;
        [SerializeField]
        private float timeTilNextWave = 0;
        public Vector2 currentMin;
        public Vector2 currentMax;
        public Vector2 targetMin;
        public Vector2 targetMax;


        private void Start()
        {
            currentWaveIndex = 0;
            currentWave = waves[currentWaveIndex];
            currentCooldown = 0;
            timeTilNextWave = currentWave.duration;
        }

        private void Update()
        {
            currentCooldown -= Time.deltaTime;
            timeTilNextWave -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                SpawnEnemies();
                UpdateBoundaries();
                currentCooldown = currentWave.cooldown;
            }
            if (timeTilNextWave <= 0)
            {
                currentWaveIndex++;
                if (currentWaveIndex == waves.Count)
                {
                    Debug.Log("The enemy spawner wave queue has reached its end, will restart from the first one");
                    currentWaveIndex = 0;
                    currentWave = waves[currentWaveIndex];
                    currentCooldown = currentWave.cooldown;
                    timeTilNextWave = currentWave.duration;
                    return;
                }
                currentWave = waves[currentWaveIndex];
                currentCooldown = currentWave.cooldown;
                timeTilNextWave = currentWave.duration;
            }
        }

        private void UpdateBoundaries()
        {
            if (targetMin.x <= currentMin.x)
                currentMin.x--;

            if (targetMin.y <= currentMin.y)
                currentMin.y--;

            if (targetMax.x > currentMax.x)
                currentMax.x++;

            if (targetMax.y > currentMax.y)
                currentMax.y++;
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
                        //Bottom
                        x = Random.Range(currentMin.x, currentMax.x);
                        y = currentMin.y;
                        break;
                    case 2:
                        //Top
                        x = Random.Range(currentMin.x, currentMax.x);
                        y = currentMax.y;
                        break;
                    case 3:
                        //Left
                        x = currentMin.x;
                        y = Random.Range(currentMin.y, currentMax.y);
                        break;
                    case 4:
                        //Right
                        x = currentMax.x;
                        y = Random.Range(currentMin.y, currentMax.y);
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
            Vector3 center = new Vector3((currentMax.x + currentMin.x) / 2f, (currentMax.y + currentMin.y) / 2f, 0f);
            Vector3 size = new Vector3(currentMax.x - currentMin.x, currentMax.y - currentMin.y, 0f);

            // Draw the wireframe box
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = Color.red;

            // Calculate the center and size of the box
            Vector3 centerr = new Vector3((targetMax.x + targetMin.x) / 2f, (targetMax.y + targetMin.y) / 2f, 0f);
            Vector3 sizee = new Vector3(targetMax.x - targetMin.x, targetMax.y - targetMin.y, 0f);

            // Draw the wireframe box
            Gizmos.DrawWireCube(centerr, sizee);
        }
    }
}
