using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.Game;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class LevelUpOption : MonoBehaviour
    {
        public ItemSO item;
        public Image icon;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI description;

        public void SetValues(ItemSO item)
        {
            this.item = item;
            icon.sprite = item.icon;
            itemName.text = item.itemName;
            
            if(item.GetType() == typeof(StructureItemSO))
            {
                description.text = item.description;
            }
            else
            {
                PassiveItemSO i = item as PassiveItemSO;
                int currentLevel = PassiveItemManager.Instance.GetCurrentLevel(i);
                description.text = i.levels[currentLevel].Description;
            }

        }

        public void GiveItem()
        {
            LevelUpMenu.Instance.GiveItem(item);
        }
    }
}
