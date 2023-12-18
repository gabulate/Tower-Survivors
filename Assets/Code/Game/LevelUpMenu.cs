using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.GUI;
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
        private ItemSO[] selectedItems = new ItemSO[3];

        public void LevelUp()
        {
            GameManager.PauseGame(true);
            ItemListSO items = AssetsHolder.Instance.itemList;
            //TODO: don't allow it to display duplicate items
            selectedItems[0] = GetRandomItem(items.itemList);
            selectedItems[1] = GetRandomItem(items.itemList);
            selectedItems[2] = GetRandomItem(items.itemList);

            for (int i = 0; i < selectedItems.Length; i++)
            {
                _options[i].SetValues(selectedItems[i]);
            }

            ShowMenu();
        }

        private ItemSO GetRandomItem(List<ItemSO> list)
        {
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

        private void ShowMenu()
        {
            _menu.SetActive(true);
        }

        private void HideMenu()
        {
            _menu.SetActive(false);
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
    }
}
