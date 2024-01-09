using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StructureItem", menuName = "ScriptableObjects/Item/Structure")]
    public class StructureItemSO : ItemSO
    {
        public List<StructureItemLevels> levels;

        public string GetUpgradeDescription(int level)
        {
            List<DescriptionPair> dp = levels[level].upgradeDescriptions;
            string s = "";
            for (int i = 0; i < dp.Count; i++)
            {
                s += string.Format(Language.Get(dp[i].descriptionKey), dp[i].value);
                if(i < dp.Count - 1)
                {
                    s += " ";
                }
            }

            return s;
        }
    }

    [System.Serializable]
    public class StructureItemLevels
    {
        public int neededLevel = 1;
        public List<DescriptionPair> upgradeDescriptions;
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
