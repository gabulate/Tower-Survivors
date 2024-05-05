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
using TowerSurvivors.Structures;

namespace TowerSurvivors.Game
{
    public class LevelUpMenu : MonoBehaviour
    {
        public static LevelUpMenu Instance;
        [SerializeField]
        private List<ItemSO> _currentItemPool;
        [SerializeField]
        private GameObject _menu;
        [SerializeField]
        private LevelUpOption[] _options;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private SoundClip _levelUpSound;

        [SerializeField]
        private TextMeshProUGUI _levelUpText; 

        [SerializeField]
        private List<ItemSO> selectedItems = new List<ItemSO>();

        [SerializeField]
        private PassiveItemSO _nothingItem;

        private void LoadCurrentItemPool()
        {
            foreach (ItemSO item in AssetsHolder.Instance.itemListSO.itemList)
            {
                if (Items.IsUnlocked(item))
                {
                    _currentItemPool.Add(item);
                }
            }
        }

        public void LevelUp()
        {
            selectedItems = GetRandomItems(_currentItemPool);

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
                    //If there are no available item slots
                    if (!Player.Inventory.AvailableStrucutreSlot())
                    {
                        availableItems.RemoveAt(i);
                    } 
                    //else if (!StructureManager.Instance.CanPlace()) //If the player has reached maximum allowed structures
                    //{
                    //    Structure[] sts = StructureManager.Instance.GetStructures();
                    //    Structure structure = sts.Where(x => x.item.itemNameKey == availableItems[i].itemNameKey).FirstOrDefault();
                        
                    //    //Removes the available item if there are not structures of its kind placed
                    //    if (!structure)
                    //    {
                    //        availableItems.RemoveAt(i);
                    //    }
                    //}
                }
                else //If item is Passive
                {
                    PassiveItem pi = PassiveItemManager.Instance.GetFromInventory(availableItems[i] as PassiveItemSO);
                    //If the passive item is already maxed, remove it from the available items
                    if (pi != null && pi.isMaxed)
                    {
                        availableItems.RemoveAt(i);
                    }
                    else if (!Player.Inventory.AvailablePassiveItemSlot()) //If there are no more available item slots
                    {
                        PassiveItem[] pis = PassiveItemManager.Instance.GetPassives();
                        PassiveItem item = pis.Where(x => x.item.itemNameKey == availableItems[i].itemNameKey).FirstOrDefault();

                        //Removes the available item if the player doesn't have it
                        if (!item)
                        {
                            availableItems.RemoveAt(i);
                        }
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

            //Gets random elements from the list and removes them after to avoid duplicate options
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

        //Gets a random element taking into consideration the item's probability attribute
        public ItemSO GetRandomFromList(List<ItemSO> list)
        {
            if (list.Count == 0)
            {
                return null;
            }


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
            AudioPlayer.Instance.PlaySFX(_levelUpSound);
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.SuperPauseGame(true);
            AudioPlayer.Instance.LowerVolume(true);
        }

        private IEnumerator HideMenu()
        {
            if (_menu.activeSelf)
            {
                GameManager.Instance.SuperPauseGame(false);
                _animator.SetBool("show", false);
                AudioPlayer.Instance.LowerVolume(false);

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
            LoadCurrentItemPool();
            _levelUpText.text = Language.Get("LEVELUP");
        }

        
    }
}
