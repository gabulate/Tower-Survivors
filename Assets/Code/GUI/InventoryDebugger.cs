using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Game;
using TowerSurvivors.PlayerScripts;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class InventoryDebugger : MonoBehaviour
    {
        public TMP_Dropdown dropdown;
        public ItemListSO items;
        private List<string> _options;
        [SerializeField]
        private string _itemName;
        private bool _active = true;

        void Start()
        {
            items = AssetsHolder.Instance.itemList;
            _options = new List<string>();
            for (int i = 0; i < items.itemList.Count; i++)
            {
                _options.Add(items.itemList[i].itemName);
            }

            dropdown.AddOptions(_options);
            _itemName = _options[0];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _active = !_active;

                GetComponent<Image>().enabled = _active;
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    child.gameObject.SetActive(_active);
                }
            }
        }

        public void SelectItem(int itemId)
        {
            _itemName = items.itemList[itemId].itemName;
        }

        public void AddItem()
        {
            ItemSO foundItem = items.itemList.Find(item => item.itemName == _itemName);

            if (foundItem != null)
            {
                Player.Inventory.AddItem(foundItem);
            }
            else
            {
                Debug.LogWarning("Couldn't find item with name: " + _itemName);
            }
        }

        public void DeleteItems()
        {
            Player.Inventory.DeleteAllItems();
        }
    }
}
