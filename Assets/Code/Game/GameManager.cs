using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.Game
{
    /// <summary>
    /// MonoBehaviour that controls the flow of the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public static bool isPaused = false;

        public SoundClip gameMusic;

        public UnityEvent<bool> e_Paused;

        [Header("Object Pools")]
        public ObjectPool XpPool;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            AudioPlayer.Instance.PlayMusic(gameMusic);
        }

        public void LevelUp()
        {
            LevelUpMenu.Instance.LevelUp();
        }

        public static void PauseGame(bool paused)
        {
            isPaused = paused;
            Time.timeScale = paused ? 0 : 1;
        }

        internal static void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }
    }
}
