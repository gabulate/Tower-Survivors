using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;
using TMPro;
using TowerSurvivors.Game;
using TowerSurvivors.ScriptableObjects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TowerSurvivors.Audio;

namespace TowerSurvivors.GUI
{
    public class StatsMenu : MonoBehaviour
    {
        [SerializeField]
        private SoundClip _clickSound;

        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private TextMeshProUGUI timeSurivedText;
        [SerializeField]
        private TextMeshProUGUI levelReachedText;
        [SerializeField]
        private TextMeshProUGUI enemiesKilledText;
        [SerializeField]
        private TextMeshProUGUI structureUpgradesText;

        [SerializeField]
        private GameObject iconPrefab;

        [SerializeField]
        private GameObject passiveGrid;
        [SerializeField]
        private GameObject structureGrid;

        void Start()
        {
            canvas.GetComponent<AutoTranslateChildren>().Translate();
            timeSurivedText.text = FormatTime(GameStats.secondsSurvived);
            levelReachedText.text = GameStats.levelReached.ToString();
            enemiesKilledText.text = GameStats.enemiesKilled.ToString();
            structureUpgradesText.text = GameStats.structuresUpgraded.ToString();
            
            foreach(ItemSO item in GameStats.passiveItems)
            {
                GameObject icon = Instantiate(iconPrefab, passiveGrid.transform);
                icon.GetComponent<Image>().sprite = item.icon;
            }

            foreach (ItemSO item in GameStats.structures)
            {
                GameObject icon = Instantiate(iconPrefab, structureGrid.transform);
                icon.GetComponent<Image>().sprite = item.icon;
            }
        }
        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{minutes:D2}:{seconds:D2}";
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
            AudioPlayer.Instance.PlaySFX(_clickSound);
        }
    }
}
