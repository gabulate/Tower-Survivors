using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class InventoryItem : MonoBehaviour
    {
        public ItemSO item;
        public Image icon;

        public void InitialiseItem(ItemSO newItem)
        {
            item = newItem;
            icon.sprite = newItem.icon;
        }
    }
}
