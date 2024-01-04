using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TowerSurvivors.Localisation;

namespace TowerSurvivors.GUI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject LanguagesMenu;

        private void Start()
        {
            if (PlayerPrefs.HasKey("language"))
            {
                LanguagesMenu.SetActive(false);
                GetComponent<AutoTranslateChildren>().Translate();
            }
            else
            {
                LanguagesMenu.SetActive(true);
            }
        }

        public void SetLanguage(string language)
        {
            AppManager.Instance.SetLanguage(language);
            LanguagesMenu.SetActive(false);
            GetComponent<AutoTranslateChildren>().Translate();
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit(1);
        }
    }
}
