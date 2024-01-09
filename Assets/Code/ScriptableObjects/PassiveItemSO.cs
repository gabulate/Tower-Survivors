using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PassiveItem", menuName = "ScriptableObjects/Item/PassiveItem")]
    public class PassiveItemSO : ItemSO
    {
        public List<PassiveItemLevels> levels;

        public string GetUpgradeDescription(int level)
        {
            List<DescriptionPair> dp = levels[level].upgradeDescriptions;
            string s = "";
            for (int i = 0; i < dp.Count; i++)
            {
                s += string.Format(Language.Get(dp[i].descriptionKey), dp[i].value);
                if (i < dp.Count - 1)
                {
                    s += " ";
                }
            }

            return s;
        }
    }

    [System.Serializable]
    public class PassiveItemLevels
    {
        public float EffectIntensityByLevel;
        public List<DescriptionPair> upgradeDescriptions;
    }
}
