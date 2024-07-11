using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.Game;
using TowerSurvivors.ScriptableObjects;

namespace TowerSurvivors
{
    public class DebugInfoText : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public bool isEnabled = false;

        void Update()
        {
            if (!isEnabled)
                return;

            string s = "";
            
            s += "Player Health: " + Player.Health.health.ToString("#.#") + " / " + Player.Health.maxHealth + "\n";
            s += "Player XP: " + Player.Instance._xp +" / " + Player.Instance.XpForNextLevel + "\n";
            s += "Player is Invincible: " + Player.Health.isInvincible.ToString() + "\n";
            s += "Enemies: " + EnemySpawner.Instance.currentEnemies + " / " + EnemySpawner.Instance.MaxEnemies + "\n";
            s += "Spawner Speed: " + EnemySpawner.Instance.speed * 100 + "%\n";
            s += "Spawner Cooldown: " + EnemySpawner.Instance.currentCooldown.ToString("0.00") + "\n";
            s += "Current Wave: " + EnemySpawner.Instance.currentWave.name + "\n";
            s += "Spawning Enemies: ";

            int enemiesLeft = 0;

            foreach(WavePair wp in EnemySpawner.Instance.currentWavePairs)
            {
                s += wp.prefab.name + ", ";
                enemiesLeft += wp.amount;
            }
            s += "\nEnemies Left in Wave: " + enemiesLeft + "\n";

            float ehb = 0;
            if (Player.Instance.Level >= 10)
                ehb = (Player.Instance.Level); //Adds % of health according to the Player level

            s += "Enemy Health Bonus: " + ehb + "%";

            text.text = s;
        }

        internal void Toggle()
        {
            isEnabled = !isEnabled;

            text.gameObject.SetActive(isEnabled);
        }

        private void Start()
        {
            isEnabled = false;
            text.gameObject.SetActive(false);
        }
    }
}
