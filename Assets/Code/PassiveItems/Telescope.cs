using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.PassiveItems
{
    public class Telescope : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of vision boost set by the levels Scriptable Object
            Player.Instance.stats.visionBoost += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.coolDownReduction = 0;
        }
    }
}
