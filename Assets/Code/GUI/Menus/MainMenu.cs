using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TowerSurvivors.Localisation;
using System;
using System.Linq;
using UnityEngine.UI;
using TowerSurvivors.Audio;
using TowerSurvivors.ScriptableObjects;

namespace TowerSurvivors.GUI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private SoundClip _clickSound;

        [SerializeField]
        private GameObject _MainMenu;
        [SerializeField]
        private GameObject LanguagesMenu;
        [SerializeField]
        private GameObject AboutPage;
        [SerializeField]
        protected GameObject SettingsMenu;
        [SerializeField]
        private GameObject CharacterSelection;
        [SerializeField]
        private GameObject ItemsPage;

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

        [SerializeField]
        private TextMeshProUGUI confirmDeleteSaveText;
        private bool _deleteSave = false;

        [SerializeField]
        private CharacterSO _defaultCharacter;

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

            CharacterSelector.SelectCharacter(_defaultCharacter);

            LoadSettings();
            AboutPage.SetActive(false);
            SettingsMenu.SetActive(false);
            CharacterSelection.SetActive(false);
            ItemsPage.SetActive(false);

            AddButtonSounds();
        }

        protected void AddButtonSounds()
        {
            Button[] buttons = GetComponentsInChildren<Button>(true);

            foreach(Button b in buttons)
            {
                b.onClick.AddListener(ClickSound);
            }
        }

        public void ClickSound()
        {
            AudioPlayer.Instance.PlaySFX(_clickSound);
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
            sfxSlider.value = GameSettings.SFXVolume;
            musicSlider.value = GameSettings.MusicVolume;
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

            GameSettings.SetFullScreen(PlayerPrefs.GetInt("fullScreen", 0));

            int displayIndex = PlayerPrefs.GetInt("fullScreen", 0);
            

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

        public void ShowCharacters(bool show)
        {
            _MainMenu.SetActive(!show);
            CharacterSelection.SetActive(show);
        }

        public void ShowItems(bool show)
        {
            _MainMenu.SetActive(!show);
            ItemsPage.SetActive(show);
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

        public void DeleteSaveData()
        {
            if (_deleteSave)
            {
                SaveSystem.DeleteSave();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                _deleteSave = true;
                confirmDeleteSaveText.text = Language.Get("CONFIRM");
            }
        }

        public void ShowCharacters()
        {
            if (SaveSystem.csd.firstBoot)
            {
                CharacterSelector.SelectCharacter(_defaultCharacter);
                SceneManager.LoadScene(1);
                SaveSystem.csd.firstBoot = false;
            }
            else
            {
                ShowCharacters(true);
            }
        }

        public void Play()
        {
            if (CharacterSelector.IsUnlocked(CharacterSelector.selected))
            {
                SceneManager.LoadScene(1);
            }
        }

        public void Quit()
        {
            Application.Quit(1);
        }
    }
}
