using System.Collections.Generic;
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
