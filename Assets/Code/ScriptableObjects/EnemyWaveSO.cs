using System.Collections.Generic;
using TowerSurvivors.Util;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Wave of enemies", menuName = "ScriptableObjects/EnemyWave")]
    public class EnemyWaveSO : ScriptableObject
    {
        [Tooltip("Enemies that will be able to spawn in this wave.")]
        public List<WavePair> enemies;
        [Tooltip("How many enemies spawn at a time.")]
        public float amountAtATime;
        [Tooltip("Number of seconds for each mini wave.")]
        public float cooldown;
        
        [Tooltip("The type of distribution for the wave.")]
        public DistributionType distribution;

        [SerializeField, ShowOnly]
        private string totalDuration = "";
        [SerializeField, ShowOnly]
        private int totalEnemies = 0;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (enemies != null /*|| !ListEquals(previousWaves, waves)*/)
            {
                float totalDurationF = 0;
                totalEnemies = 0;
                for (int i = 0; i < enemies.Count; i++)
                {
                    totalDurationF += enemies[i].amount * cooldown / amountAtATime;
                    totalEnemies += enemies[i].amount;
                }

                totalDuration = FormatTime(totalDurationF);
                // Update the previousWaves
            }
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{minutes:D2}:{seconds:D2}";
        }
#endif
    }

    [System.Serializable]
    public class WavePair
    {
        public GameObject prefab;
        public int amount = 1;
    }

    public enum DistributionType
    {
        Random, Horde
    }
}
