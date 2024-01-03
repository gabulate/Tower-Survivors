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
                Destroy(gameObject);
            DontDestroyOnLoad(this.gameObject);
            Language.InitialiseLanguage(languagesCSV, language);
            PlayerPrefs.SetString("language", language);
        }

        private void Start()
        {
            
        }
    }
}
