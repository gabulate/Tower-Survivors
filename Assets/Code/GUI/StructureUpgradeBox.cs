using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class StructureUpgradeBox : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Title;
        [SerializeField]
        private TextMeshProUGUI Description;
        [SerializeField]
        private TextMeshProUGUI Requirements;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Image iconBack;

        [SerializeField]
        private Color yesColor;
        [SerializeField]
        private Color noColor;

        public void SetValues(Structure structure, bool canUpgrade)
        {
            transform.position = structure.transform.position;
            int nextLevel = structure.level + 1;
            Title.text = "Upgrade to level: " + nextLevel;
            Description.text = structure.item.levels[structure.level].upgradeDescription;
            icon.sprite = structure.item.icon;
            Requirements.color = canUpgrade ? yesColor : noColor;
            iconBack.color = canUpgrade ? yesColor : noColor;
            
        }
    }
}
