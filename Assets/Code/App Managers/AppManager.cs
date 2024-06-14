using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerSurvivors
{
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance;
        public TextAsset languagesCSV;

        public string language = "English";

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
                
            DontDestroyOnLoad(this.gameObject);

            Language.SetCSV(languagesCSV);
            if (PlayerPrefs.HasKey("language"))
            {
                Language.InitialiseLanguage(PlayerPrefs.GetString("language"));
            }

            GameSettings.LoadSettings();

            InitialiseSaveData();
        }

        private void InitialiseSaveData()
        {
            try
            {
                if (!SaveSystem.LoadSaveFromDisk())
                {
                    SaveSystem.CreateNewSave();
                }
            }
            catch (Exception)
            {
                SaveSystem.CreateNewSave();
                throw;
            }
        }

        public void SetLanguage(string language)
        {
            PlayerPrefs.SetString("language", language);
            Language.InitialiseLanguage(PlayerPrefs.GetString("language"));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
