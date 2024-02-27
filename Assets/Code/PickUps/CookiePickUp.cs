using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PickUps
{
    public class CookiePickUp : PickUp
    {
        [SerializeField]
        private float _healAmount = 10f;
        protected override void ExecPickUp()
        {
            Player.Health.Heal(_healAmount);
            _sprite.enabled = false;
            Destroy(gameObject);
        }
    }
}
