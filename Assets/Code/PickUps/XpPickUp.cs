using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

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
