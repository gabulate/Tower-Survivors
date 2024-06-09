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

        //Structures
        public bool unCannon = true;
        public bool unSniper = true;
        public bool unRoundabout = true;
        public bool unBarbedWire = true;
        public bool unRocketLauncher = true;

        public bool unPawn = true;
        public bool unQueen = false;
        public bool unDartMonkey = true;
        public bool unHardDrive = true;
        public bool unVendingMachine = true;
        public bool unGunStick = true;

        //Items
        public bool unOverClocker = false;
        public bool unHammer = false;
        public bool unRunningShoes = false;
        public bool unTelescope = true;
        public bool unLubricator = false;

        public bool unDuplicator = false;
        public bool unFrogsTail = false;
        public bool unScope = false;
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
