using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TowerSurvivors
{
    public class SaveData
    {
        //Meta
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
        public bool chEngineer = true;
        public bool chPrisoner = false;
        public bool chSpaceCowboy = false;
        public bool chMima = false;
        public bool chJen = false;

        //Structures
        public bool unCannon = true;
        public bool unRoundabout = true;
        public bool unBarbedWire = true;
        public bool unRocketLauncher = true;
        public bool unGunStick = true;
        public bool unVendingMachine = true;

        public bool unSniper = false;
        public bool unPawn = false;
        public bool unQueen = false;
        public bool unDartMonkey = false;
        public bool unHardDrive = false;

        //Items
        public bool unFrogsTail = true;
        public bool unLubricator = true;
        public bool unRunningShoes = true;
        public bool unTelescope = true;

        public bool unHammer = false;
        public bool unOverClocker = false;
        public bool unScope = false;
        public bool unDuplicator = false;
        public bool unPremiumGunpowder = false;
        public bool unMushroom = false;

        public bool HasEveryItem()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.Name.StartsWith("un") && field.Name != "unQueen")
                {
                    if (field.FieldType == typeof(bool) && !(bool)field.GetValue(this))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
