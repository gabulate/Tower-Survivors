using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.PassiveItems
{
    public class RunningShoes : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of movement speed set by the levels Scriptable Object
            Player.Instance.stats.speedBoost += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.speedBoost = 0;
        }
    }
}
