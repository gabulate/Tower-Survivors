using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.PassiveItems
{
    public class OverClocker : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of cooldown reduction set by the levels Scriptable Object
            Player.Instance.stats.coolDownReduction += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.coolDownReduction = 0;
        }
    }
}
