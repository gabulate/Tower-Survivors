using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public static class GameStats
    {
        public static float secondsSurvived;
        public static uint levelReached;
        public static uint enemiesKilled;
        public static uint structuresUpgraded;
        public static uint coinsCollected;

        public static List<PassiveItemSO> passiveItems;
        public static List<StructureItemSO> structures;

        internal static void Reset()
        {
            secondsSurvived = 0;
            levelReached = 0;
            enemiesKilled = 0;
            structuresUpgraded = 0;
            coinsCollected = 0;

            passiveItems.Clear();
            structures.Clear();
        }
    }
}
