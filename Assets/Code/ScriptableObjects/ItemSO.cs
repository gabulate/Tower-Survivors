using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]
    public class ItemSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemType type;
        public GameObject prefab;

    }

    public enum ItemType
    {
        Structure,
        Passive
    }
}
