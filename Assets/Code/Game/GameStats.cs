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

        public static List<PassiveItemSO> passiveItems;
        public static List<StructureItemSO> structures;
    }
}
