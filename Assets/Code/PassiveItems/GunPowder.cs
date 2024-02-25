using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.PassiveItems
{
    public class GunPowder : PassiveItem
    {
        public override void ApplyEffect()
        {
            //Gives the player the amount of damage increase set by the levels Scriptable Object
            Player.Instance.stats.damageIncrease += item.levels[level - 1].EffectIntensityByLevel;
        }

        public override void RemoveEffect()
        {
            Player.Instance.stats.damageIncrease = 0;
        }
    }
}
