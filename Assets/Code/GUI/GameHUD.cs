using System.Collections;
using TMPro;
using TowerSurvivors.Game;
using TowerSurvivors.Localisation;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class GameHUD : MonoBehaviour
    {
        public Slider xpBar;
        public TextMeshProUGUI levelText;
        public Slider hpBar;
        public StructureUpgradeBox upBox;
        public TextMeshProUGUI structureQtyText;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI enemiesKilledText;

        private void Start()
        {
            Player.Instance.e_xpChanged.AddListener(UpdateXpBar);
            Player.Instance.e_leveledUp.AddListener(UpdateLevel);
            Player.Health.e_healthChanged.AddListener(UpdateHealth);
            StructureManager.Instance.e_StAmntChanged.AddListener(UpdateStructureQty);
            GameManager.Instance.e_KillCountUpdated.AddListener(UpdateKillCount);
            UpdateStructureQty(0, StructureManager.Instance.initialMaximumStructures);
            UpdateLevel(1);
        }

        private void LateUpdate()
        {
            timerText.text = FormatTime(GameManager.Instance.secondsPassed);
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{minutes:D2}:{seconds:D2}";
        }

        public void UpdateKillCount(int count)
        {
            enemiesKilledText.text = count.ToString();
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
            if (structure != null)
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
            levelText.text = Language.Get("CURRENTLEVEL") +" " + level;
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

        public void OnEnable()
        {
            Player.Instance.e_xpChanged.AddListener(UpdateXpBar);
            Player.Instance.e_leveledUp.AddListener(UpdateLevel);
            Player.Health.e_healthChanged.AddListener(UpdateHealth);
            StructureManager.Instance.e_StAmntChanged.AddListener(UpdateStructureQty);
        }

        public void OnDisable()
        {
            Player.Instance.e_xpChanged.RemoveListener(UpdateXpBar);
            Player.Health.e_healthChanged.RemoveListener(UpdateHealth);
            Player.Health.e_healthChanged.RemoveListener(UpdateHealth);
            StructureManager.Instance.e_StAmntChanged.RemoveListener(UpdateStructureQty);
        }
    }
}
