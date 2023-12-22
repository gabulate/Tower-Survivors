using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class PassiveItemManager : MonoBehaviour
    {
        public static PassiveItemManager Instance;

        /// <summary>
        /// Checks if the item is already in the inventory, and if not spawns it
        /// </summary>
        /// <param name="item">ScriptableObject of the item.</param>
        /// <returns>Game object containing the PassiveItem component.</returns>
        public GameObject AddOrLevelUp(PassiveItemSO item)
        {
            if (!InInventory(item))
            {
                GameObject go = Instantiate(item.prefab, transform);
                Player.Instance.ApplyBuffs();

                return go;
            }
            else
            {
                foreach (PassiveItem p in GetPassives())
                {
                    if (p.item.itemName == item.itemName)
                    {
                        if (p.level >= p.item.levels.Count)
                        {
                            Debug.LogWarning("Tried to updgrade item beyond its maximum level.");
                            return p.gameObject;
                        }

                        p.level++;
                        if (p.level == p.item.levels.Count)
                            p.isMaxed = true;
                        Player.Instance.ApplyBuffs();
                        break;
                    }
                }
            }

            //Should never happen since it checks with InInventory()
            return null;
        }

        public PassiveItem[] GetPassives()
        {
            return GetComponentsInChildren<PassiveItem>();
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        internal PassiveItem GetFromInventory(PassiveItemSO item)
        {
            foreach (PassiveItem p in GetPassives())
            {
                if (p.item.itemName == item.itemName)
                {
                    return p;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the current level of the item specified. Returns 0 if the item is not currently in the inventory.
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns></returns>
        public int GetCurrentLevel(PassiveItemSO item)
        {
            foreach(PassiveItem p in GetPassives())
            {
                if(p.item.itemName == item.itemName)
                {
                    return p.level;
                }
            }
            return 0;
        }

        public bool InInventory(PassiveItemSO item)
        {
            foreach (PassiveItem p in GetPassives())
            {
                if (p.item.itemName == item.itemName)
                {
                    return true;
                }
            }
            return false;
        }

        internal void RemoveAllItems()
        {
            foreach(PassiveItem p in GetPassives())
            {
                p.RemoveEffect();
                Destroy(p.gameObject);
            }
            Player.Instance.RemoveAllBuffs();
        }
    }
}
