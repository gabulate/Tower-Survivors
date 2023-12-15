using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class OverClocker : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of cooldown reduction set by the levels Scriptable Object
            Player.Instance.coolDownReduction = EffectLevels.EffectIntensityByLevel[level - 1];
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
        }
    }
}
