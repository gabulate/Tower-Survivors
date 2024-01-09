using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class Lubricator : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of cooldown reduction set by the levels Scriptable Object
            Player.Instance.stats.projectileSpeedBoost += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.projectileSpeedBoost = 0;
        }
    }
}
