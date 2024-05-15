using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors
{
    public class SaveData
    {
        public string cheatAllYouWant = "Thanks for playing.";
        public bool developerMode = false;
        public bool firstBoot = false;

        public uint coins = 0;

        //Stats
        public uint timesDied = 0;
        public uint totalEnemiesKilled = 0;
        public float totalSecondsSurvived = 0;
        public uint maxLevelReached = 0;

        //Characters
        public bool unEngineer = true;
        public bool unPrisoner = false;
        public bool unSpaceCowboy = false;

        //Structures
        public bool unCannon = true;
        public bool unSniper = true;
        public bool unRoundabout = true;
        public bool unBarbedWire = true;
        public bool unRocketLauncher = true;

        public bool unPawn = false;
        public bool unQueen = false;
        public bool unDartMonkey = false;
        public bool unHardDrive = false;
        public bool unVendingMachine = false;
        public bool unGunStick = false;

        //Items
        public bool unOverClocker = true;
        public bool unHammer = true;
        public bool unRunningShoes = true;
        public bool unTelescope = true;
        public bool unLubricator = true;

        public bool unDuplicator = false;
        public bool unFrogsTail = false;
        public bool unScope = false;
        public bool unPremiumGunpowder = false;
        public bool unMushroom = false;
    }
}
