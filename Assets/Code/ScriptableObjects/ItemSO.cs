using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
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

    public enum ItemType
    {
        Structure,
        Passive
    }
}
