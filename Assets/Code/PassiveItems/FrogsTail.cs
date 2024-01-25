using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.PassiveItems
{
    public class FrogsTail : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of health regen set by the levels Scriptable Object
            Player.Instance.stats.healthRegen += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.healthRegen = 0;
        }
    }
}
