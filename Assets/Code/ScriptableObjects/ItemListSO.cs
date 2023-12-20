using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    /// <summary>
    /// Scriptable object to hold a list of items
    /// </summary>
    public class ItemListSO : ScriptableObject
    {
        public List<ItemSO> itemList;
    }
}
