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
        [SerializeField]
        private GameObject _pauseMenu;

        public static float secondsPassed = 0;
        private static int _enemiesKilled = 0;

        public SoundClip gameMusic;

        public UnityEvent<bool> e_Paused;
        public UnityEvent<int> e_KillCountUpdated;

        [Header("Object Pools")]
        public ObjectPool XpPool;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        public void AddToKillCOunt(int amount)
        {
            _enemiesKilled += amount;
            e_KillCountUpdated.Invoke(_enemiesKilled);
        }

        private void Start()
        {
            AudioPlayer.Instance.PlayMusic(gameMusic);
            secondsPassed = 0;
            _enemiesKilled = 0;
            GameSettings.LoadSettings();
            _pauseMenu.SetActive(false);
        }

        private void FixedUpdate()
        {
            secondsPassed += Time.fixedDeltaTime;
        }

        public void LevelUp()
        {
            LevelUpMenu.Instance.LevelUp();
        }

        public void PauseGame(bool paused)
        {
            isPaused = paused;
            Time.timeScale = paused ? 0 : 1;
        }

        public void ShowPauseMenu(bool show)
        {
            isPaused = show;
            Time.timeScale = show ? 0 : 1;
            _pauseMenu.SetActive(show);
        }

        internal static void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }

        internal void Restart()
        {
            secondsPassed = 0;
        }
    }
}
