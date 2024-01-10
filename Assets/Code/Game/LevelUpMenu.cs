using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.Audio;
using TowerSurvivors.GUI;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using TMPro;
using TowerSurvivors.Localisation;

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
        private Animator _animator;

        [SerializeField]
        private TextMeshProUGUI _levelUpText; 

        [SerializeField]
        private List<ItemSO> selectedItems = new List<ItemSO>();

        [SerializeField]
        private PassiveItemSO _nothingItem;

        public void LevelUp()
        {
            ItemListSO items = AssetsHolder.Instance.itemList;

            selectedItems = GetRandomItems(items.itemList);

            for (int i = 0; i < selectedItems.Count; i++)
            {
                _options[i].SetValues(selectedItems[i]);
                _options[i].gameObject.SetActive(true);
            }

            StartCoroutine(ShowMenu());
        }

        private List<ItemSO> GetRandomItems(List<ItemSO> list)
        {
            //Create a duplicate of the list
            List<ItemSO> availableItems = list.Select(i => Instantiate(i)).ToList();

            //Iterate backwards for safe removing
            for (int i = availableItems.Count - 1; i >= 0; i--)
            {
                if (availableItems[i].GetType() == typeof(StructureItemSO))
                {
                    //TODO: Maybe add a maximum of allowed structures.
                    if (!Player.Inventory.AvailableStrucutreSlot())
                    {
                        availableItems.RemoveAt(i);
                    }
                }
                else
                {
                    PassiveItem pi = PassiveItemManager.Instance.GetFromInventory(availableItems[i] as PassiveItemSO);
                    //If the passive item is already maxed, remove it from the available items
                    if (pi != null && pi.isMaxed)
                    {
                        availableItems.RemoveAt(i);
                    }
                    else if (!Player.Inventory.AvailablePassiveItemSlot())
                    {
                        availableItems.RemoveAt(i);
                    }
                }
            }

            List<ItemSO> resultList = new List<ItemSO>();

            //If there are no available items
            if (availableItems.Count == 0)
            {
                Debug.Log("No available items.");
                resultList.Add(_nothingItem);
                return resultList;
            }

            for (int i = 0; i < 3; i++)
            {
                ItemSO item = GetRandomFromList(availableItems);
                if (item != null)
                {
                    resultList.Add(item);
                    availableItems.Remove(item);
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
            if (list.Count == 0)
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
            StartCoroutine(HideMenu());
        }

        private IEnumerator ShowMenu()
        {
            _menu.SetActive(true);
            _animator.SetBool("show", true);
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.SuperPauseGame(true);
            AudioPlayer.Instance.PauseMusic(true);
        }

        private IEnumerator HideMenu()
        {
            if (_menu.activeSelf)
            {
                GameManager.Instance.SuperPauseGame(false);
                _animator.SetBool("show", false);
                AudioPlayer.Instance.PauseMusic(false);

                yield return new WaitForSeconds(0.1f);
                foreach (LevelUpOption op in _options)
                {
                    op.gameObject.SetActive(false);
                }
                _menu.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                Player.Instance.CheckForLevelUp();
            }
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
            StartCoroutine(HideMenu());
            _levelUpText.text = Language.Get("LEVELUP");
        }
    }
}
