using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class ItemOption : MonoBehaviour
    {
        public ItemSO item;
        public Image backgorund;
        public Image sprite;

        public static UnityEvent Unselect = new UnityEvent();
        public static UnityEvent<ItemSO> OnSelected = new UnityEvent<ItemSO>();
        public void SelectThisItem()
        {
            Unselect.Invoke();
            ItemsPage.SelectItem(item);
            backgorund.color = new Color(0.255f, 0.153f, 0.243f);
            OnSelected.Invoke(item);
        }

        private void ChangeColorBackToNormal()
        {
            backgorund.color = new Color(0.427f, 0.455f, 0.518f);
        }

        private void CheckUnlocked()
        {
            if (Items.IsUnlocked(item))
            {
                sprite.color = Color.white;
            }
            else
            {
                sprite.color = Color.black;
            }
        }

        private void OnEnable()
        {
            if (ItemsPage.selected.itemNameKey == item.itemNameKey)
            {
                backgorund.color = new Color(0.255f, 0.153f, 0.243f);
            }

            sprite.sprite = item.icon;

            if (!Items.IsUnlocked(item))
            {
                sprite.color = Color.black;
            }

            Unselect.AddListener(this.ChangeColorBackToNormal);
            ItemsPage.e_UnlockedItem.AddListener(CheckUnlocked);
        }

        private void OnDisable()
        {
            Unselect.RemoveListener(this.ChangeColorBackToNormal);
            ItemsPage.e_UnlockedItem.RemoveListener(CheckUnlocked);
        }
    }
}
