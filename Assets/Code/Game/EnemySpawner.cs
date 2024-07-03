using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.Enemies;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerSurvivors.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        protected static readonly LayerMask _enemyLayer = 1 << 6;
        public GameObject bossPrefab;

        public bool activeSpawning = true;
        public float speed = 1;
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

        public float currentRadius;
        public float targetRadius;

        private void Start()
        {
            currentWaveIndex = 0;
            currentWave = waves[currentWaveIndex];
            currentCooldown = 0;
            currentWavePairs = ClonePairs(currentWave);
        }

        //Copies the wave pairs by value, not reference
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
            if(!activeSpawning)
                return;

            currentCooldown -= Time.deltaTime * speed;

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
                currentWavePairs = ClonePairs(currentWave);
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
            if (currentRadius < targetRadius)
                currentRadius++;
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

                // Calculate the total amount of enemies left
                int totalAmount = 0;
                foreach (var wavePair in currentWavePairs)
                {
                    totalAmount += wavePair.amount;
                }

                // If no enemies left, return
                if (totalAmount == 0)
                    return;

                // Determine the winner
                int winner = Random.Range(0, totalAmount);
                int threshold = 0;
                int selectedIndex = 0;

                // Find the selected enemy based on the winner value
                for (int j = 0; j < currentWavePairs.Count; j++)
                {
                    threshold += currentWavePairs[j].amount;
                    if (threshold > winner)
                    {
                        selectedIndex = j;
                        break;
                    }
                }

                // Get the selected enemy
                GameObject randomEnemy = currentWavePairs[selectedIndex].prefab;
                Instantiate(randomEnemy, position, Quaternion.identity);
                currentEnemies++;

                // Subtract one from the enemy queue and check if the amount of enemies of that kind has reached 0.
                // If it has, it removes it from the current pool
                currentWavePairs[selectedIndex].amount--;
                if (currentWavePairs[selectedIndex].amount <= 0)
                {
                    currentWavePairs.RemoveAt(selectedIndex);
                }

                // If there are no more wave pairs, return to start the next wave
                if (currentWavePairs.Count == 0)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Kills every enemy on screen and stops spawning enemies
        /// </summary>
        public void KillAllEnemies()
        {
            activeSpawning = false;
            StartCoroutine(KillEnemies());
        }

        public void SpawnBoss()
        {
            //TODO: Make the boss spawn closer to the player
            Vector3 position = GetRandomPosition();
            position = (position - Player.Instance.transform.position).normalized;
            position = Player.Instance.transform.position - position * 10;

            GameObject bp = Instantiate(bossPrefab, position, Quaternion.identity);
            currentEnemies++;

            Boss e = bp.GetComponent<Boss>();
            e.e_die.AddListener(GameManager.Instance.GameWon);
        }

        private IEnumerator KillEnemies()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector3(0,0,0), 150, _enemyLayer);

            for (int i = hits.Length - 1; i >= 0; i--)
            {
                if (hits[i] == null)
                    continue;

                Enemy e = hits[i].GetComponent<Enemy>();
                if (e && e.isAlive)
                {
                    e.ChanceToDropXp = 0;
                    e.TakeDamage(99999, false);
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }

        //Gets a random position along the circumference of the spawning range
        private Vector3 GetRandomPosition()
        {
            //Generate a random angle in radians
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            //Calculate the x and y coordinates using trigonometry
            float x = currentRadius * Mathf.Cos(randomAngle);
            float y = currentRadius * Mathf.Sin(randomAngle);

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
            //Draws the current radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, currentRadius);

            //Draws target radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetRadius);
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
