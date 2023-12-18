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

        public void AddPassiveItem(ItemSO item)
        {
            GameObject obj = Instantiate(item.prefab, transform);
            obj.GetComponent<PassiveItem>().ApplyEffect();
            Player.Instance.ApplyBuffs();
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
