using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class LevelUpOption : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI description;

        public void SetValues(ItemSO item)
        {
            icon.sprite = item.icon;
            itemName.text = item.itemName;
            description.text = item.description;
        }
    }
}
