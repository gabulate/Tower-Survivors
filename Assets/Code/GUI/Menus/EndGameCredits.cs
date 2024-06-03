using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerSurvivors.GUI
{
    public class EndGameCredits : MonoBehaviour
    {
        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
