using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.GUI;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// Handles Item Inventory
    /// ref: https://www.youtube.com/watch?v=oJAE6CbsQQA&ab_channel=CocoCode
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        public InventorySlot[] StructureSlots;
        public InventorySlot[] PassiveItemSlots;

        public GameObject inventoryItemPrefab;

        public void AddItem(ItemSO item)
        {
            if(item.type == ItemType.Structure)
            {
                for (int i = 0; i < StructureSlots.Length; i++)
                {
                    InventorySlot slot = StructureSlots[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot == null)
                    {
                        SpawnNewItem(item, slot);
                        return;
                    }
                }
                Debug.LogWarning("Inventory full.");
                //TODO: DO Something if there aren't any available slots
            }
            else
            {
                for (int i = 0; i < PassiveItemSlots.Length; i++)
                {
                    InventorySlot slot = PassiveItemSlots[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot == null)
                    {
                        SpawnNewItem(item, slot);
                        return;
                    }
                }
                Debug.LogWarning("Inventory full.");

                //TODO: DO Something if there aren't any available slots
            }
        }

        private void SpawnNewItem(ItemSO item, InventorySlot slot)
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item);
        }

        public void DeleteAllItems()
        {
            Debug.LogWarning("Not implemented yet");
        }
    }
}
