using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Wave of enemies", menuName = "ScriptableObjects/EnemyWave")]
    public class EnemyWaveSO : ScriptableObject
    {
        [Tooltip("Enemies that will be able to spawn in this wave.")]
        public List<GameObject> enemies;
        [Tooltip("How many enemies spawn at a time.")]
        public float amountAtATime;
        [Tooltip("Number of seconds for each mini wave.")]
        public float cooldown;
        [Tooltip("How long will this wave last until the next one starts.")]
        public float duration;
        [Tooltip("The type of distribution for the wave.")]
        public DistributionType distribution;
    }

    public enum DistributionType
    {
        Random, Horde
    }
}
