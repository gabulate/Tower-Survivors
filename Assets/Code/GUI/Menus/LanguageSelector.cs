using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class LanguageSelector : MonoBehaviour
    {
        public VerticalLayoutGroup list;
        public GameObject optionPrefab;

        void Start()
        {
            string[] languages = Language.GetAvailableLanguages();

            for (int i = 0; i < languages.Length; i++)
            {
                LanguageOption lo = Instantiate(optionPrefab, list.transform).GetComponent<LanguageOption>();
                lo.Inistialise(languages[i]);
            }
        }
    }
}
