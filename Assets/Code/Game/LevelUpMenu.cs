using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.GUI;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class LevelUpMenu : MonoBehaviour
    {
        public static LevelUpMenu Instance;

        [SerializeField]
        private GameObject _menu;
        [SerializeField]
        private LevelUpOption[] _options;

        [SerializeField]
        private List<ItemSO> selectedItems = new List<ItemSO>();

        [SerializeField]
        private PassiveItemSO _nothingItem;

        public void LevelUp()
        {
            GameManager.PauseGame(true);
            ItemListSO items = AssetsHolder.Instance.itemList;

            selectedItems = GetRandomItems(items.itemList);

            for (int i = 0; i < selectedItems.Count; i++)
            {
                _options[i].SetValues(selectedItems[i]);
                _options[i].gameObject.SetActive(true);
            }

            ShowMenu();
        }

        private List<ItemSO> GetRandomItems(List<ItemSO> list)
        {
            //Create a duplicate of the list
            List<ItemSO> availabelItems = list.Select(i => Instantiate(i)).ToList();

            //Iterate backwards for safe removing
            for (int i = availabelItems.Count - 1; i >= 0; i--)
            {
                if(availabelItems[i].GetType() == typeof(StructureItemSO))
                {
                    //TODO: Maybe add a maximum of allowed structures.
                }
                else
                {
                    PassiveItem pi = PassiveItemManager.Instance.InInventory(availabelItems[i] as PassiveItemSO);
                    //If the passive item is already maxed, remove it from the available items
                    if(pi != null && pi.isMaxed)
                    {
                        availabelItems.RemoveAt(i);
                    }
                }
            }

            List<ItemSO> resultList = new List<ItemSO>();

            //If there are no available items
            if (availabelItems.Count == 0)
            {
                resultList.Add(_nothingItem);
                return resultList;
            }

            for (int i = 0; i < 3; i++)
            {
                ItemSO item = GetRandomFromList(availabelItems);
                if(item != null)
                {
                    resultList.Add(item);
                    availabelItems.Remove(item);
                }
                else
                {
                    break;
                }
            }

            return resultList;
        }

        public ItemSO GetRandomFromList(List<ItemSO> list)
        {
            if(list.Count == 0)
            {
                return null;
            }

            List<ItemSO> newList = new List<ItemSO>();

            float totalRange = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalRange += list[i].probability;
            }

            float winner = Random.Range(0, totalRange);
            float threshold = 0;
            for (int i = 0; i < list.Count; i++)
            {
                threshold += list[i].probability;
                if (threshold > winner)
                {
                    return list[i];
                }
            }
            return null;
        }

        public void GiveItem(ItemSO item)
        {
            Player.Inventory.AddItem(item);
            HideMenu();
            GameManager.PauseGame(false);
        }

        private void ShowMenu()
        {
            _menu.SetActive(true);
        }

        private void HideMenu()
        {
            foreach(LevelUpOption op in _options)
            {
                op.gameObject.SetActive(false);
            }
            _menu.SetActive(false);
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
        private void Start()
        {
            HideMenu();
        }
    }
}
