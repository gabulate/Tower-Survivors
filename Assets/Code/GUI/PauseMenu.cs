using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.Localisation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerSurvivors.GUI
{
    public class PauseMenu : MainMenu
    {
        [SerializeField]
        private GameObject _confirmMenu;
        void Start()
        {
            if (PlayerPrefs.HasKey("language"))
            {
                GetComponent<AutoTranslateChildren>().Translate();
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
    }
}
