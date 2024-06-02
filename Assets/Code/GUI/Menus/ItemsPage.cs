using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.Audio;
using TowerSurvivors.Game;
using TowerSurvivors.Localisation;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class ItemsPage : MonoBehaviour
    {
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDsc;
        public Button buyButton;
        public TextMeshProUGUI buyText;
        public TextMeshProUGUI coinsText;
        public Image coinImage;

        public SoundClip errorSound;

        public static ItemSO selected;

        [SerializeField]
        private ItemSO _defaultItem;
        public ItemListSO itemList;

        [SerializeField]
        public GameObject _itemPrefab;

        [SerializeField]
        private GameObject _itemGrid;

        public static UnityEvent e_UnlockedItem = new UnityEvent();

        private void Start()
        {
            selected = _defaultItem;
            LoadItems();
        }

        private void LoadItems()
        {
            for (int i = 0; i < itemList.itemList.Count; i++)
            {
                //Only show the queen item if it has already been discovered
                if(itemList.itemList[i].itemNameKey == "QUEEN_NAME")
                {
                    if (!SaveSystem.csd.unQueen)
                    {
                        continue;
                    }
                }

                ItemOption io = Instantiate(_itemPrefab, _itemGrid.transform).GetComponent<ItemOption>();
                io.item = itemList.itemList[i];
                io.enabled = true;
            }
        }

        private void ShowItemDeets(ItemSO item)
        {
            if (Items.IsUnlocked(item))
            {
                itemName.text = Language.Get(item.itemNameKey);
                itemDsc.text = Language.Get(item.descriptionKey);
                buyText.text = Language.Get("UNLOCKED");
                buyText.color = Color.gray;
                coinImage.enabled = false;
                buyButton.interactable = false;
            }
            else
            {
                buyButton.interactable = true;
                itemName.text = "???";
                itemDsc.text = "???";
                itemName.text = "???";
                buyText.text = item.price.ToString();
                coinImage.enabled = true;

                if (SaveSystem.csd.coins < item.price)
                    buyText.color = Color.red;
                else
                    buyText.color = Color.white;

            }
        }

        public void Buy()
        {
            if (Items.IsUnlocked(selected))
            {
                return;
            }
            else
            {
                if (Items.Unlock(selected))
                {
                    ShowItemDeets(selected);
                    coinsText.text = SaveSystem.csd.coins.ToString();
                    e_UnlockedItem.Invoke();
                }
                else
                {
                    AudioPlayer.Instance.PlaySFX(errorSound);
                }
            }
        }

        public static void SelectItem(ItemSO item)
        {
            selected = item;
        }

        private void OnEnable()
        {
            ItemOption.OnSelected.AddListener(ShowItemDeets);
            coinsText.text = SaveSystem.csd.coins.ToString();

            if (!selected)
            {
                SelectItem(_defaultItem);
                ShowItemDeets(_defaultItem);
            }
        }

        private void OnDisable()
        {
            ItemOption.OnSelected.RemoveListener(ShowItemDeets);
        }
    }
}
