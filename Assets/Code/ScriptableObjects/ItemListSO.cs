using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    /// <summary>
    /// NOT IMPLEMENTED YET, just testing
    /// Will be used to hold the list of structures and their corresponding stats, including levelups
    /// </summary>
    [CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList")]
    public class ItemListSO : ScriptableObject
    {
        public List<ItemSO> itemList;
    }
}
