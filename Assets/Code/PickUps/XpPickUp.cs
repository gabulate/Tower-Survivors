using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.PickUps
{
    public class XpPickUp : PickUp
    {
        public int Xp = 1;
        protected override void ExecPickUp()
        {
            Player.Instance.AddXp(Xp);
            gameObject.SetActive(false);
        }
    }
}
