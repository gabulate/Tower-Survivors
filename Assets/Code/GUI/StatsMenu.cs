using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Localisation;
using UnityEngine;
using TMPro;
using TowerSurvivors.Game;
using TowerSurvivors.ScriptableObjects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TowerSurvivors.Audio;
using System;

namespace TowerSurvivors.GUI
{
    public class StatsMenu : MonoBehaviour
    {
        [SerializeField]
        private SoundClip _clickSound;

        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private TextMeshProUGUI timeSurivedText;
        [SerializeField]
        private TextMeshProUGUI levelReachedText;
        [SerializeField]
        private TextMeshProUGUI enemiesKilledText;
        [SerializeField]
        private TextMeshProUGUI structureUpgradesText;
        [SerializeField]
        private TextMeshProUGUI coinsText;

        [SerializeField]
        private GameObject iconPrefab;

        [SerializeField]
        private GameObject passiveGrid;
        [SerializeField]
        private GameObject structureGrid;

        public uint totalCoins = 0;

        void Start()
        {
            LoadMatchStats();
            CalculateCoins();
            SaveMatchData();
        }

        private void CalculateCoins()
        {
            totalCoins = GameStats.coinsCollected;
            totalCoins += Convert.ToUInt32(GameStats.secondsSurvived * 0.20f);
            totalCoins += Convert.ToUInt32(GameStats.enemiesKilled * 0.25f);
            StartCoroutine(ShowCoins(totalCoins));
        }

        private void LoadMatchStats()
        {
            canvas.GetComponent<AutoTranslateChildren>().Translate();
            timeSurivedText.text = FormatTime(GameStats.secondsSurvived);
            levelReachedText.text = GameStats.levelReached.ToString();
            enemiesKilledText.text = GameStats.enemiesKilled.ToString();
            structureUpgradesText.text = GameStats.structuresUpgraded.ToString();

            foreach (ItemSO item in GameStats.passiveItems)
            {
                GameObject icon = Instantiate(iconPrefab, passiveGrid.transform);
                icon.GetComponent<Image>().sprite = item.icon;
            }

            foreach (ItemSO item in GameStats.structures)
            {
                GameObject icon = Instantiate(iconPrefab, structureGrid.transform);
                icon.GetComponent<Image>().sprite = item.icon;
            }
        }

        private void SaveMatchData()
        {
            SaveSystem.csd.timesDied++;
            SaveSystem.csd.totalEnemiesKilled += GameStats.enemiesKilled;
            SaveSystem.csd.totalSecondsSurvived += GameStats.secondsSurvived;
            SaveSystem.csd.coins += totalCoins;
            if (GameStats.levelReached > SaveSystem.csd.maxLevelReached)
                SaveSystem.csd.maxLevelReached = GameStats.levelReached;

            GameStats.Reset();

            SaveSystem.Save();
        }

        private IEnumerator ShowCoins(uint coins)
        {
            uint current = 0;

            float soundTimer = 0.2f;
            while(current <= coins)
            {
                current += (uint)(Time.deltaTime * 127);

                coinsText.text = current.ToString();

                soundTimer -= Time.deltaTime;
                if(soundTimer <= 0)
                {
                    AudioPlayer.Instance.PlaySFX(_clickSound);
                    soundTimer = 0.06f;
                }

                yield return new WaitForEndOfFrame();
            }

            coinsText.text = coins.ToString();
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{minutes:D2}:{seconds:D2}";
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
            AudioPlayer.Instance.PlaySFX(_clickSound);
        }
    }
}
