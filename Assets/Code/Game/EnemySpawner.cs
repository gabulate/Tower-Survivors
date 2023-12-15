using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public float cooldown;
        public float currentCooldown;

        public GameObject enemy;
        // Update is called once per frame
        void Update()
        {
            currentCooldown = currentCooldown <= 0 ? 0 : currentCooldown - Time.deltaTime;
            if (currentCooldown <= 0)
                SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            currentCooldown = cooldown;
        }
    }
}
