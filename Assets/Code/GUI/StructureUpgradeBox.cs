using System;
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

            icon.gameObject.SetActive(true);
            Requirements.gameObject.SetActive(true);
            iconBack.gameObject.SetActive(true);
            icon.sprite = structure.item.icon;
            Requirements.color = canUpgrade ? yesColor : noColor;
            iconBack.color = canUpgrade ? yesColor : noColor;
            
        }

        internal void SetValuesMax(Structure structure)
        {
            transform.position = structure.transform.position;
            Title.text = "Max level " + structure.item.itemName + ".";
            Description.text = "Maximum level reached!";

            
            icon.gameObject.SetActive(false);
            Requirements.gameObject.SetActive(false);
            iconBack.gameObject.SetActive(false);
        }
    }
}
