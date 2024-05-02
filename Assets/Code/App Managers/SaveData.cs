using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors
{
    public class SaveData
    {
        public string cheatAllYouWant = "Thanks for playing.";
        public bool firstBoot = false;

        public int coins = 0;

        //Stats
        public uint timesDied = 0;
        public uint totalEnemiesKilled = 0;
        public float totalSecondsSurvived = 0;
        public uint maxLevelReached = 0;

        //Characters
        public bool unEngineer = true;
        public bool unPrisoner = false;

        public bool unRocket = false;
    }
}
