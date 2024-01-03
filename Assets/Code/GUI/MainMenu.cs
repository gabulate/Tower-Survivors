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
        public TextMeshProUGUI playText;

        private void Start()
        {
            playText.text = Language.Get("PLAY");
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
        }
    }
}
