using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.Localisation;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using TowerSurvivors.PlayerScripts;

namespace TowerSurvivors.GUI
{
    public class PauseMenu : MainMenu
    {
        [SerializeField]
        private GameObject _confirmMenu;
        [SerializeField]
        private TextMeshProUGUI _statsText;
        void Start()
        {
            if (PlayerPrefs.HasKey("language"))
            {
                GetComponent<AutoTranslateChildren>().Translate();
                UpdateStats();
            }

            LoadSettings();
            SettingsMenu.SetActive(false);
        }


        public void ShowReturnToMenu(bool show)
        {
            _confirmMenu.SetActive(show);
        }

        public void ResumeGame()
        {
            GameManager.Instance.ShowPauseMenu(false);
        }

        public void ReturnToMenu()
        {
            GameManager.Instance.LoadStats();
            GameManager.Instance.ShowPauseMenu(false);
            SceneManager.LoadScene("GameOver");
        }

        internal void UpdateStats()
        {
            string s = "";
            PlayerStats stats = Player.Instance.stats;

            s += string.Format("{0}: +{1}%\n", Language.Get("STAT_RANGE"), stats.rangeIncrease * 100);
            s += string.Format("{0}: +{1}%\n", Language.Get("STAT_DAMAGE"), stats.damageIncrease * 100);
            s += string.Format("{0}: -{1}%\n", Language.Get("STAT_COOLDOWN"), stats.coolDownReduction * 100);
            s += string.Format("{0}: +{1}%\n", Language.Get("STAT_SIZE"), stats.areaSizeIncrease * 100);
            s += string.Format("{0}: +{1}%\n", Language.Get("STAT_PRJSPEED"), stats.projectileSpeedBoost * 100);
            s += string.Format("{0}: +{1}%\n", Language.Get("STAT_PRJDURATION"), stats.durationIncrease * 100);
            s += string.Format("{0}: +{1}\n", Language.Get("STAT_PRJAMNT"), stats.ProjectileAmntIncrease);
            s += string.Format("{0}: +{1}%\n", Language.Get("STAT_SPEED"), stats.speedBoost * 100);
            s += string.Format("{0}: +{1}\n", Language.Get("STAT_VISION"), stats.visionBoost);
            s += string.Format("{0}: {1}\n", Language.Get("STAT_STRUCTURELIMIT"), StructureManager.Instance.MaximumStructures);
            s += string.Format("{0}: {1}p/s", Language.Get("STAT_HEALTHREGEN"), stats.healthRegen);


            _statsText.text = s;
        }
    }
}
