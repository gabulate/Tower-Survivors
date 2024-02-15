using System;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerSurvivors.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public int MaxEnemies = 30;
        public int currentEnemies = 0;

        public static EnemySpawner Instance;

        [SerializeReference]
        public List<EnemyWaveSO> waves;
#if UNITY_EDITOR
        private List<EnemyWaveSO> previousWaves;

        private void OnValidate()
        {
            if (waves != null /*|| !ListEquals(previousWaves, waves)*/)
            {
                float totalDurationF = 0;
                totalEnemies = 0;
                for (int i = 0; i < waves.Count; i++)
                {
                    for (int j = 0; j < waves[i].enemies.Count; j++)
                    {
                        totalDurationF += waves[i].enemies[j].amount * waves[i].cooldown / waves[i].amountAtATime;
                        totalEnemies += waves[i].enemies[j].amount;
                    }
                }

                totalDuration = FormatTime(totalDurationF);
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
        private string totalDuration = "";
        [SerializeField, ShowOnly]
        private int totalEnemies = 0;
        [SerializeField]
        private EnemyWaveSO currentWave;
        [SerializeField]
        private List<WavePair> currentWavePairs;
        [SerializeField]
        private int currentWaveIndex = 0;
        [SerializeField]
        private float currentCooldown;
        //[SerializeField]
        //private float timeTilNextWave = 0;
        public Vector2 currentMin;
        public Vector2 currentMax;
        public Vector2 targetMin;
        public Vector2 targetMax;


        private void Start()
        {
            currentWaveIndex = 0;
            currentWave = waves[currentWaveIndex];
            currentCooldown = 0;
            currentWavePairs = ClonePairs(currentWave);
        }

        private List<WavePair> ClonePairs(EnemyWaveSO wave)
        {
            List<WavePair> list = new List<WavePair>();
            for (int i = 0; i < wave.enemies.Count; i++)
            {
                WavePair wp = new WavePair()
                {
                    prefab = wave.enemies[i].prefab,
                    amount = wave.enemies[i].amount,
                };

                list.Add(wp);
            }
            return list;
        }

        private void Update()
        {
            currentCooldown -= Time.deltaTime;

            //Check if the queue ran out of enemies, go to the next wave
            if (currentWavePairs.Count == 0)
            {
                currentWaveIndex++;
                //If it reaches the end of the waves
                if (currentWaveIndex == waves.Count)
                {
                    Debug.Log("The enemy spawner wave queue has reached its end, will restart from the first one");
                    currentWaveIndex = 0;
                    currentWave = waves[currentWaveIndex];
                    currentCooldown = currentWave.cooldown;
                    currentWavePairs = currentWavePairs = ClonePairs(currentWave);
                    return;
                }
                //If it just reaches the end of the current wave
                currentWave = waves[currentWaveIndex];
                currentCooldown = currentWave.cooldown;
                currentWavePairs = currentWavePairs = ClonePairs(currentWave);
            }

            if (currentCooldown <= 0)
            {
                SpawnEnemies();
                UpdateBoundaries();
                currentCooldown = currentWave.cooldown;
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
                if (currentEnemies >= MaxEnemies)
                    return;

                Vector3 position = GetRandomPosition();

                int random = Random.Range(0, currentWavePairs.Count);

                //Get Random enemy
                GameObject randomEnemy = currentWavePairs[random].prefab;
                Instantiate(randomEnemy, position, Quaternion.identity);
                currentEnemies++;

                //Subtract one from the enemy queue and check if the amount of enemies of that kind has reached 0.
                //If it has, it removes it from the current pool
                currentWavePairs[random].amount--;
                if(currentWavePairs[random].amount <= 0)
                {
                    currentWavePairs.Remove(currentWavePairs[random]);
                }

                //If there are no more wave pairs, return to start the next wave
                if (currentWavePairs.Count == 0)
                {
                    return;
                }
            }
        }

        private Vector3 GetRandomPosition()
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

            return new(transform.position.x + x, transform.position.y + y, y);
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        void LateUpdate()
        {
            //Follows the player
            //Super high level complicated stuff really
            transform.position = new Vector3(
                Player.Instance.transform.position.x,
                Player.Instance.transform.position.y,
                Player.Instance.transform.position.y - 10);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            // Calculate the center and size of the box
            Vector3 center = new Vector3(transform.position.x +((currentMax.x + currentMin.x) / 2f), transform.position.y + ((currentMax.y + currentMin.y) / 2f), 0f);
            Vector3 size = new Vector3(currentMax.x - currentMin.x, currentMax.y - currentMin.y, 0f);

            // Draw the wireframe box
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = Color.red;

            // Calculate the center and size of the box
            Vector3 centerr = new Vector3(transform.position.x + ((targetMax.x + targetMin.x) / 2f), transform.position.y + ((targetMax.y + targetMin.y) / 2f), 0f);
            Vector3 sizee = new Vector3(targetMax.x - targetMin.x, targetMax.y - targetMin.y, 0f);

            // Draw the wireframe box
            Gizmos.DrawWireCube(centerr, sizee);
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
