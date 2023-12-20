using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using TowerSurvivors.Game;

namespace TowerSurvivors.GUI
{
    public class GameHUD : MonoBehaviour
    {
        public Slider xpBar;
        public TextMeshProUGUI levelText;
        public Slider hpBar;
        public StructureUpgradeBox upBox;
        public TextMeshProUGUI structureQtyText;

        private void Start()
        {
            Player.Instance.e_xpChanged.AddListener(UpdateXpBar);
            Player.Instance.e_leveledUp.AddListener(UpdateLevel);
            Player.Health.e_healthChanged.AddListener(UpdateHealth);
            StructureManager.Instance.e_StAmntChanged.AddListener(UpdateStructureQty);
        }

        public void UpdateStructureQty(int current, int max)
        {
            structureQtyText.text = current + "/" + max;

            if (current == max)
                structureQtyText.color = Color.red;
            else
                structureQtyText.color = Color.white;
        }

        public void HoverStructure(Structure structure, bool canUpgrade)
        {
            if(structure != null)
                upBox.gameObject.SetActive(true);

            upBox.SetValues(structure, canUpgrade);
        }

        public void HoverStructureMax(Structure structure)
        {
            if (structure != null)
                upBox.gameObject.SetActive(true);

            upBox.SetValuesMax(structure);
        }

        public void HideUpBox()
        {
            upBox.gameObject.SetActive(false);
        }

        public void UpdateXpBar(int current, int max)
        {
            StartCoroutine(SmoothChangeXp(0.2f, current, max));
        }

        public void UpdateLevel(int level)
        {
            levelText.text = "Level " + level;
        }

        public void UpdateHealth(float current, float max)
        {
            StartCoroutine(SmoothChangeHealth(0.2f, current, max));
        }

        IEnumerator SmoothChangeXp(float seconds, float current, float max)
        {

            float elapsedTime = 0f;
            float oldValue = xpBar.value;
            float targetValue = current / max;

            while (elapsedTime < seconds)
            {
                xpBar.value = Mathf.Lerp(oldValue, targetValue, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator SmoothChangeHealth(float seconds, float current, float max)
        {

            float elapsedTime = 0f;
            float oldValue = hpBar.value;
            float targetValue = current / max;

            while (elapsedTime < seconds)
            {
                hpBar.value = Mathf.Lerp(oldValue, targetValue, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public void SelectItem(int index)
        {
            Player.Inventory.SelectItem(index);
        }

        public void OnDisable()
        {
            Player.Instance.e_xpChanged.RemoveListener(UpdateXpBar);
            Player.Health.e_healthChanged.RemoveListener(UpdateHealth);
        }
    }
}
