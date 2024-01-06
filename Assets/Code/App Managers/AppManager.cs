using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;

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

            if (PlayerPrefs.HasKey("language"))
            {
                Language.InitialiseLanguage(languagesCSV, PlayerPrefs.GetString("language"));
            }

            GameSettings.LoadSettings();
        }

        public void SetLanguage(string language)
        {
            PlayerPrefs.SetString("language", language);
            Language.InitialiseLanguage(languagesCSV, PlayerPrefs.GetString("language"));
        }
    }
}
