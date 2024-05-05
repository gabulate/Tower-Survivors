using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    /// <summary>
    /// Base class for items, gets extended to PassiveItemSO ans StructureItemSO
    /// </summary>
    public abstract class ItemSO : ScriptableObject
    {
        public string itemNameKey;
        [TextArea]
        public string descriptionKey;
        public Sprite icon;
        //public ItemType type;
        public GameObject prefab;

        public float probability = 5;
        public uint price = 100;
    }

    [System.Serializable]
    public class DescriptionPair
    {
        public string descriptionKey;
        public string value;
    }
}
