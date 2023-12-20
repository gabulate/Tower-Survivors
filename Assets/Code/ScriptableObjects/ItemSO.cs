using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    /// <summary>
    /// Base class for items, gets extended to PassiveItemSO ans StructureItemSO
    /// </summary>
    public class ItemSO : ScriptableObject
    {
        public string itemName;
        [TextArea]
        public string description;
        public Sprite icon;
        //public ItemType type;
        public GameObject prefab;

        public float probability = 5;
    }
}
