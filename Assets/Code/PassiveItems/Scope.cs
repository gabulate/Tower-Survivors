using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class Scope : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of range increase set by the levels Scriptable Object
            Player.Instance.stats.rangeIncrease += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.rangeIncrease = 0;
        }
    }
}
