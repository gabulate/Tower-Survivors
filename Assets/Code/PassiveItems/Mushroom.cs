using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class Mushroom : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of area size set by the levels Scriptable Object
            Player.Instance.stats.areaSizeIncrease += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.areaSizeIncrease = 0;
        }
    }
}
