using TowerSurvivors.Game;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PickUps
{
    public class XpPickUp : PickUp
    {
        public int Xp = 1;

        private void OnEnable()
        {
            if (Xp == 1)
                _sprite.color = new Color(1.0f, 0.12156862745098039f, 0.20392156862745098f); // Red
            else if (Xp <= 4)
                _sprite.color = new Color(0.2549019607843137f, 0.6941176470588235f, 0.984313725490196f); // Blue
            else if (Xp <= 9)
                _sprite.color = new Color(1.0f, 0.807843137254902f, 0.0f); // Yellow
            else if (Xp <= 19)
                _sprite.color = new Color(0.24705882352941178f, 0.9137254901960784f, 0.5529411764705883f); // BlueGreenish
            else if (Xp <= 49)
                _sprite.color = new Color(1.0f, 0.596078431372549f, 0.0f); // Orange
            else
                _sprite.color = new Color(0.6705882352941176f, 0.0f, 1.0f); // Purple

        }

        protected override void ExecPickUp()
        {
            Player.Instance.AddXp(Xp);
            XpObjectPool.Instance.enabledXpObjects--;
            gameObject.SetActive(false);
        }
    }
}
