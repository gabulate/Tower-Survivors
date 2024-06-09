using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    public class CoinItem : PassiveItem
    {
        public uint coins = 10;
        public override void ApplyEffect()
        {
        }

        public override void NonPhysical()
        {
            //Gives the player coins
            GameManager.Instance.AddCoins(coins);
        }

        public override void RemoveEffect()
        {
        }
    }
}
