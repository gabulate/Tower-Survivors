using System;
using TowerSurvivors.Game;
using TowerSurvivors.GUI;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
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

        internal void RemoveItem(PassiveItemSO item)
        {
            throw new NotImplementedException();
        }

        public InventorySlot[] PassiveItemSlots;

        public InventoryItem selectedItem;
        public int selectedIndex;

        [SerializeField]
        private GameObject _inventoryItemPrefab;

        /// <summary>
        /// Selects the slot number specified.
        /// </summary>
        /// <param name="slot">Slot number, must be in range from 0 to the amount of slots avlaiable -1.</param>
        public void SelectItem(int slot)
        {
            if (slot >= StructureSlots.Length || slot < 0)
                return;
            selectedIndex = slot;

            if (selectedItem != null)
            {
                selectedItem.UnHighLight();
                selectedItem = null;
            }

            foreach (InventorySlot s in StructureSlots)
            {
                s.UnHighLight();
            }

            StructureSlots[slot].GetComponent<InventorySlot>().HighLight();
            InventoryItem item = StructureSlots[slot].GetComponentInChildren<InventoryItem>();
            if (item == null)
            {
                StructureManager.Instance.UnhighlightAll();
                return;
            }
                

            item.HighLight();
            selectedItem = item;
            StructureManager.Instance.UnhighlightAll();
            StructureManager.Instance.HighLigthTypeOf(selectedItem.itemInstance.GetComponent<Structure>());
        }

        /// <summary>
        /// Removes the selected Item from the Inventory
        /// </summary>
        public void UseItem()
        {
            Destroy(selectedItem.gameObject);
        }

        /// <summary>
        /// Looks for an available item slot and does different things depending on the item type.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ItemSO item)
        {

            if (item.GetType() == typeof(StructureItemSO))
            {
                for (int i = 0; i < StructureSlots.Length; i++)
                {
                    InventorySlot slot = StructureSlots[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot == null)
                    {
                        GameObject instance = StructureManager.Instance.AddToInventory(item as StructureItemSO, i);
                        SpawnNewItem(item, slot, instance);
                        SelectItem(i);
                        return;
                    }
                }
                Debug.LogWarning("Inventory full.");
            }
            else //Passive Item
            {
                PassiveItemSO pi = item as PassiveItemSO;
                if (!pi.physical) //If the item is non phyisical, it is not added to the inventory, but its method is called once
                {
                    PassiveItem p = Instantiate(pi.prefab).GetComponent<PassiveItem>();
                    p.NonPhysical();
                    Destroy(p.gameObject);
                    return;
                }

                //If the item is not already in the inventory, spawn it
                if (!PassiveItemManager.Instance.InInventory(item as PassiveItemSO))
                {
                    for (int i = 0; i < PassiveItemSlots.Length; i++)
                    {
                        InventorySlot slot = PassiveItemSlots[i];
                        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                        if (itemInSlot == null)
                        {
                            GameObject instance = PassiveItemManager.Instance.AddOrLevelUp(item as PassiveItemSO);
                            SpawnNewItem(item, slot, instance);
                            break;
                        }
                    }
                }
                else
                {
                    PassiveItemManager.Instance.AddOrLevelUp(item as PassiveItemSO);
                    foreach (InventorySlot it in PassiveItemSlots)
                    {
                        InventoryItem ii = it.GetComponentInChildren<InventoryItem>();
                        if (ii)
                            ii.UpdateInfo();
                    }
                }
            }
        }

        /// <summary>
        /// Adds an item to the indicated slot.
        /// </summary>
        /// <param name="item">Scriptable Object cointaining the item's atributes.</param>
        /// <param name="slot">Slot where the item will be added.</param>
        private void SpawnNewItem(ItemSO item, InventorySlot slot, GameObject instance)
        {
            GameObject newItemGO = Instantiate(_inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item, instance);
        }

        public void PickUpStructure(Structure structure)
        {
            int slotIndex = selectedIndex;

            InventoryItem itemInS = StructureSlots[slotIndex].GetComponentInChildren<InventoryItem>();
            if (itemInS != null) //If the selected slot is already taken, try to find an available slot
            {
                for (int i = 0; i < StructureSlots.Length; i++)
                {
                    InventorySlot slot = StructureSlots[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot == null)
                    {
                        slotIndex = i;
                        break;
                    }
                }
            }

            SpawnNewItem(structure.item, StructureSlots[slotIndex], structure.gameObject);
            StructureManager.Instance.PickUpStructure(structure);
            SelectItem(slotIndex);
            return;
        }

        /// <summary>
        /// Deletes all the items from all of the slots.
        /// </summary>
        public void DeleteAllItems()
        {
            for (int i = 0; i < StructureSlots.Length; i++)
            {
                InventorySlot slot = StructureSlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    Destroy(itemInSlot.gameObject);
                }
            }

            for (int i = 0; i < PassiveItemSlots.Length; i++)
            {
                InventorySlot slot = PassiveItemSlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    Destroy(itemInSlot.gameObject);
                }
            }
            PassiveItemManager.Instance.RemoveAllItems();
        }

        /// <summary>
        /// Indictes if there are any available Structure Slots.
        /// </summary>
        /// <returns>True if there is at least one available slot.</returns>
        public bool AvailableStrucutreSlot()
        {
            for (int i = 0; i < StructureSlots.Length; i++)
            {
                InventorySlot slot = StructureSlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the number of available structure slots
        /// </summary>
        /// <returns>True if there is at least one available slot.</returns>
        public int AvailableStructureSlots()
        {
            int number = 0;
            for (int i = 0; i < StructureSlots.Length; i++)
            {
                InventorySlot slot = StructureSlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                    number++;
            }

            return number;
        }

        /// <summary>
        /// Indictes if there are any available Passive Item Slots.
        /// </summary>
        /// <returns>True if there is at least one available slot.</returns>
        public bool AvailablePassiveItemSlot()
        {
            for (int i = 0; i < PassiveItemSlots.Length; i++)
            {
                InventorySlot slot = PassiveItemSlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
