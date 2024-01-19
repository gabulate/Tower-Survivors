using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class Duplicator : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of extra projectiles set by the levels Scriptable Object
            Player.Instance.stats.ProjectileAmntIncrease += (int)item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.ProjectileAmntIncrease = 0;
        }
    }
}
