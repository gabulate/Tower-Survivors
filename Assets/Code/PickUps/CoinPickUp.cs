using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using UnityEngine;

namespace TowerSurvivors.PickUps
{
    public class CoinPickUp : PickUp
    {
        public uint coins = 1;
        protected override void ExecPickUp()
        {
            GameManager.Instance.AddCoins(coins);
            _sprite.enabled = false;
            Destroy(gameObject);
        }
    }
}
