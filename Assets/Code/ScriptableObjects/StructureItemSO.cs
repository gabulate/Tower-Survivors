using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StructureItem", menuName = "ScriptableObjects/Item/Structure")]
    public class StructureItemSO : ItemSO
    {
        public List<StructureItemLevels> levels;
    }

    [System.Serializable]
    public class StructureItemLevels
    {
        public int neededLevel = 1;
        [TextArea]
        public string DescriptionKey;
        public float range;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;
        public float areaSize = 1;
        public float projectileSpeed = 2;
        public float duration = 5f;
        public int projectileAmnt = 1;
        public float timeBetweenMultipleShots = 0.5f; //In case projectileAmnt is bigger than 1;
        public int passThroughAmnt = 0;
    }
}
