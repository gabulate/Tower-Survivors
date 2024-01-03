using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PassiveItem", menuName = "ScriptableObjects/Item/PassiveItem")]
    public class PassiveItemSO : ItemSO
    {
        public List<PassiveItemLevels> levels;
    }

    [System.Serializable]
    public class PassiveItemLevels
    {
        public float EffectIntensityByLevel;
        [TextArea]
        public string DescriptionKey;
    }
}
