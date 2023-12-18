using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class GameHUD : MonoBehaviour
    {
        public Slider xpBar;
        public Slider hpBar;

        private void Start()
        {
            Player.Instance.e_xpChanged.AddListener(UpdateXpBar);
            Player.Health.e_healthChanged.AddListener(UpdateHealth);
        }

        public void UpdateXpBar(int current, int max)
        {
            StartCoroutine(SmoothChangeXp(0.2f, current, max));
        }

        public void UpdateHealth(float current, float max)
        {
            StartCoroutine(SmoothChangeHealth(0.2f, current, max));
        }

        IEnumerator SmoothChangeXp(float seconds, float current, float max)
        {

            float elapsedTime = 0f;
            float oldValue = xpBar.value;
            float targetValue = current / max;

            while (elapsedTime < seconds)
            {
                xpBar.value = Mathf.Lerp(oldValue, targetValue, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator SmoothChangeHealth(float seconds, float current, float max)
        {

            float elapsedTime = 0f;
            float oldValue = hpBar.value;
            float targetValue = current / max;

            while (elapsedTime < seconds)
            {
                hpBar.value = Mathf.Lerp(oldValue, targetValue, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public void SelectItem(int index)
        {
            Player.Inventory.SelectItem(index);
        }

        public void OnDisable()
        {
            Player.Instance.e_xpChanged.RemoveListener(UpdateXpBar);
            Player.Health.e_healthChanged.RemoveListener(UpdateHealth);
        }
    }
}
