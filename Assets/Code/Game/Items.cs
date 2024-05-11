using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class Items : MonoBehaviour
    {
        public static bool IsUnlocked(ItemSO item)
        {
            switch (item.itemNameKey.ToUpper())
            {
                default:
                    Debug.LogError("Didn't find key:" + item.itemNameKey.ToUpper());
                    return false;
                //Structures
                case "CANNON_NAME":
                    return SaveSystem.csd.unCannon;
                case "SNIPER_NAME":
                    return SaveSystem.csd.unSniper;
                case "BARBEDWIRE_NAME":
                    return SaveSystem.csd.unBarbedWire;
                case "MONKEY_NAME":
                    return SaveSystem.csd.unDartMonkey;
                case "HDD_NAME":
                    return SaveSystem.csd.unHardDrive;
                case "PAWN_NAME":
                    return SaveSystem.csd.unPawn;
                case "QUEEN_NAME":
                    return SaveSystem.csd.unPawn; //The queen is unlocked with the pawn
                case "ROCKETLAUNCHER_NAME":
                    return SaveSystem.csd.unRocketLauncher;
                case "ROUNDABOUT_NAME":
                    return SaveSystem.csd.unRoundabout;
                case "VENDMACH_NAME":
                    return SaveSystem.csd.unVendingMachine;
                case "GUNSTICK_NAME":
                    return SaveSystem.csd.unGunStick;
                //Passive items
                case "DUPLICATOR_NAME":
                    return SaveSystem.csd.unDuplicator;
                case "FROGSTAIL_NAME":
                    return SaveSystem.csd.unFrogsTail;
                case "HAMMER_NAME":
                    return SaveSystem.csd.unHammer;
                case "LUBRICATOR_NAME":
                    return SaveSystem.csd.unLubricator;
                case "MUSHROOM_NAME":
                    return SaveSystem.csd.unMushroom;
                case "OVERCLOCKER_NAME":
                    return SaveSystem.csd.unOverClocker;
                case "GUNPOWDER_NAME":
                    return SaveSystem.csd.unPremiumGunpowder;
                case "RUNNINGSHOES_NAME":
                    return SaveSystem.csd.unRunningShoes;
                case "SCOPE_NAME":
                    return SaveSystem.csd.unScope;
                case "TELESCOPE_NAME":
                    return SaveSystem.csd.unTelescope;
                
            }
        }

        internal static bool Unlock(ItemSO item)
        {
            if (item.price > SaveSystem.csd.coins)
                return false;

            switch (item.itemNameKey.ToUpper())
            {
                default:
                    Debug.LogError("Didn't find key:" + item.itemNameKey.ToUpper());
                    return false;
                // Structures
                case "CANNON_NAME":
                    SaveSystem.csd.unCannon = true;
                    break;
                case "SNIPER_NAME":
                    SaveSystem.csd.unSniper = true;
                    break;
                case "BARBEDWIRE_NAME":
                    SaveSystem.csd.unBarbedWire = true;
                    break;
                case "MONKEY_NAME":
                    SaveSystem.csd.unDartMonkey = true;
                    break;
                case "HDD_NAME":
                    SaveSystem.csd.unHardDrive = true;
                    break;
                case "PAWN_NAME":
                    SaveSystem.csd.unPawn = true;
                    break;
                case "QUEEN_NAME":
                    // The queen is unlocked with the pawn
                    SaveSystem.csd.unPawn = true;
                    break;
                case "ROCKETLAUNCHER_NAME":
                    SaveSystem.csd.unRocketLauncher = true;
                    break;
                case "ROUNDABOUT_NAME":
                    SaveSystem.csd.unRoundabout = true;
                    break;
                case "VENDMACH_NAME":
                    SaveSystem.csd.unVendingMachine = true;
                    break;
                case "GUNSTICK_NAME":
                    SaveSystem.csd.unGunStick = true;
                    break;
                // Passive items
                case "DUPLICATOR_NAME":
                    SaveSystem.csd.unDuplicator = true;
                    break;
                case "FROGSTAIL_NAME":
                    SaveSystem.csd.unFrogsTail = true;
                    break;
                case "HAMMER_NAME":
                    SaveSystem.csd.unHammer = true;
                    break;
                case "LUBRICATOR_NAME":
                    SaveSystem.csd.unLubricator = true;
                    break;
                case "MUSHROOM_NAME":
                    SaveSystem.csd.unMushroom = true;
                    break;
                case "OVERCLOCKER_NAME":
                    SaveSystem.csd.unOverClocker = true;
                    break;
                case "GUNPOWDER_NAME":
                    SaveSystem.csd.unPremiumGunpowder = true;
                    break;
                case "RUNNINGSHOES_NAME":
                    SaveSystem.csd.unRunningShoes = true;
                    break;
                case "SCOPE_NAME":
                    SaveSystem.csd.unScope = true;
                    break;
                case "TELESCOPE_NAME":
                    SaveSystem.csd.unTelescope = true;
                    break;
            }

            SaveSystem.csd.coins -= item.price;
            SaveSystem.Save();
            return true;
        }

    }
}
