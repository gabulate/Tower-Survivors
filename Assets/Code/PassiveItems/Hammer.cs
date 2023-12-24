using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class Hammer : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of extra structures set by the levels Scriptable Object
            Player.Instance.stats.extraStructures += (int)item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.extraStructures = 0;
        }

    }
}
