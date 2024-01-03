using TowerSurvivors.Game;
using TowerSurvivors.GUI;
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
                return;

            item.HighLight();
            selectedItem = item;
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
            InventoryItem itemInSlot = StructureSlots[selectedIndex].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(structure.item, StructureSlots[selectedIndex], structure.gameObject);
                StructureManager.Instance.PickUpStructure(structure);
                SelectItem(selectedIndex);
                return;
            }

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
