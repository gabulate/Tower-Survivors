using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PassiveItemLevels", menuName = "ScriptableObjects/PassiveItemLevels")]
    public class PassiveItemLevelsSO : ScriptableObject
    {
        public List<float> EffectIntensityByLevel;
        [TextArea]
        public string Description;
    }
}
