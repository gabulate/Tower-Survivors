using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerSurvivors
{
    public class GameHUD : MonoBehaviour
    {
        public Slider xpBar;
        public Slider hpBar;

        private void Start()
        {
            Player.Instance.e_xpChanged.AddListener(UpdateXpBar);
            //Player.Health.e_healthChanged.AddListener(UpdateHealth);
        }

        public void UpdateXpBar(int current, int max)
        {
            xpBar.value = (float)current / max;
        }

        public void UpdateHealth(float current, float max)
        {
            hpBar.value = current / max;
        }

        public void OnDisable()
        {
            Player.Instance.e_xpChanged.RemoveListener(UpdateXpBar);
            Player.Health.e_healthChanged.RemoveListener(UpdateHealth);
        }
    }
}
