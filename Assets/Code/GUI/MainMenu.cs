using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TowerSurvivors.Localisation;
using System;
using System.Linq;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _MainMenu;
        [SerializeField]
        private GameObject LanguagesMenu;
        [SerializeField]
        private GameObject AboutPage;
        [SerializeField]
        protected GameObject SettingsMenu;

        [SerializeField]
        private TMP_Dropdown displayDropdown;
        [SerializeField]
        private TMP_Dropdown ResolutionsDropdown;
        [SerializeField]
        private Resolution[] resolutions;

        [SerializeField]
        private Slider sfxSlider;
        [SerializeField]
        private Slider musicSlider;

        [SerializeField]
        private TextMeshProUGUI confirmDeleteText;
        private bool _deleteConfig = false;

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

            LoadSettings();
            AboutPage.SetActive(false);
            SettingsMenu.SetActive(false);
        }

        protected void LoadSettings()
        {

            GameSettings.LoadSettings();
            LoadDisplayOptions();
            LoadResolutions();
            LoadVolumeSettings();
        }

        private void LoadVolumeSettings()
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        }

        public void ChangeSFXVolume(float volume)
        {
            GameSettings.SetSFXVolume(volume);
        }

        public void ChangeMusicVolume(float volume)
        {
            GameSettings.SetMusicVolume(volume);
        }

        private void LoadResolutions()
        {
            ResolutionsDropdown.onValueChanged.RemoveAllListeners();
            resolutions = Screen.resolutions;
            ResolutionsDropdown.ClearOptions();
            List<string> options = new List<string>();
            int resIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if(resolutions[i].height >= 480)
                {
                    options.Add(resolutions[i].width + " x " + resolutions[i].height);
                    if (resolutions[i].width == Screen.currentResolution.width &&
                        resolutions[i].height == Screen.currentResolution.height) resIndex = i;
                }
            }
            ResolutionsDropdown.AddOptions(options);
            ResolutionsDropdown.value = resIndex;
            ResolutionsDropdown.RefreshShownValue();
            ResolutionsDropdown.onValueChanged.AddListener(ChangeResolution);
        }

        private void LoadDisplayOptions()
        {
            List<string> displayOptions = new List<string>();
            displayOptions.Add(Language.Get("FULLSCREEN"));
            displayOptions.Add(Language.Get("WINDOWED_FULLSCREEN"));
            displayOptions.Add(Language.Get("WINDOWED"));

            int displayIndex = 0;
            switch (Screen.fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    displayIndex = 0;
                    break;
                case FullScreenMode.FullScreenWindow:
                    displayIndex = 1;
                    break;
                case FullScreenMode.Windowed:
                    displayIndex = 2;
                    break;
            }

            displayDropdown.ClearOptions();
            displayDropdown.AddOptions(displayOptions.Distinct().ToList());
            displayDropdown.value = displayIndex;
            displayDropdown.RefreshShownValue();
        }

        public void ChangeResolution(int index)
        {
            string[] res = ResolutionsDropdown.options[index].text.Split("x", StringSplitOptions.None);

            GameSettings.SetResolution(int.Parse(res[0].Trim()), int.Parse(res[1].Trim()));
            LoadResolutions();
        }

        public void SetDisplay(int type)
        {
            GameSettings.SetFullScreen(type);
        }

        public void SetLanguage(string language)
        {
            AppManager.Instance.SetLanguage(language);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowAbout(bool show)
        {
            _MainMenu.SetActive(!show);
            AboutPage.SetActive(show);
        }

        public void ShowSettings(bool show)
        {
            _MainMenu.SetActive(!show);
            SettingsMenu.SetActive(show);
        }

        public void ShowLanguageMenu()
        {
            PlayerPrefs.DeleteKey("language");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestoreConfiguration()
        {
            if (_deleteConfig)
            {
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                _deleteConfig = true;
                confirmDeleteText.text = Language.Get("CONFIRM");
            }
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
