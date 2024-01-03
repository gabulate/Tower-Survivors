using TMPro;
using TowerSurvivors.Game;
using TowerSurvivors.Localisation;
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
            itemName.text = Language.Get(item.itemNameKey);

            if (item.GetType() == typeof(StructureItemSO))
            {
                description.text = Language.Get(item.descriptionKey);
            }
            else
            {
                PassiveItemSO i = item as PassiveItemSO;
                int currentLevel = PassiveItemManager.Instance.GetCurrentLevel(i);
                description.text = Language.Get(i.levels[currentLevel].DescriptionKey);
            }

        }

        public void GiveItem()
        {
            LevelUpMenu.Instance.GiveItem(item);
        }
    }
}
