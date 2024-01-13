using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Localisation;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TowerSurvivors.Game
{
    /// <summary>
    /// MonoBehaviour that controls the flow of the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [Header("Must Have References")]
        [SerializeField]
        private GameObject _pauseMenu;
        [SerializeField]
        private GameObject _gameOverScreen;
        public SoundClip gameMusic;

        [Header("Game config")]
        [SerializeField]
        private bool _randomizeSpawn = true;

        [Header("Game state")]
        public static bool isPaused = false;
        public static bool isSuperPaused = false; //When super paused, the player can't unpause. eg: when the level up menu shows

        [Header("Game Stats")]
        public static float secondsPassed = 0;
        private static int _enemiesKilled = 0;
        public static int structuresUpgraded = 0;

        public UnityEvent<bool> e_Paused;
        public UnityEvent<int> e_KillCountUpdated;


        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        public void AddToKillCount(int amount)
        {
            _enemiesKilled += amount;
            e_KillCountUpdated.Invoke(_enemiesKilled);
        }

        private void Start()
        {
            AudioPlayer.Instance.PlayMusic(gameMusic);
            secondsPassed = 0;
            _enemiesKilled = 0;
            structuresUpgraded = 0;
            GameSettings.LoadSettings();
            _pauseMenu.SetActive(false);
            _gameOverScreen.SetActive(false);
            Time.timeScale = 1;
            Player.PlayerInput.EnableMovement(true);
            SuperPauseGame(false);

            if (_randomizeSpawn)
            {
                MovePlayerToRandomPosition();
            }
        }

        private void MovePlayerToRandomPosition()
        {
            while (true)
            {
                float x = Random.Range(-104, 127);
                float y = Random.Range(-45, 93);

                Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(x, y), 1);
                if (hits.Length == 0)
                {
                    Player.Instance.transform.position = new Vector3(x, y, y);
                    break;
                }
            }
        }

        private void FixedUpdate()
        {
            secondsPassed += Time.fixedDeltaTime;
        }

        public void LevelUp()
        {
            isSuperPaused = true;
            LevelUpMenu.Instance.LevelUp();
        }

        public void SuperPauseGame(bool paused)
        {
            isSuperPaused = paused;
            isPaused = paused;
            Time.timeScale = paused ? 0 : 1;
        }

        public void ShowPauseMenu(bool show)
        {
            if (isSuperPaused)
                return;
            isPaused = show;
            Time.timeScale = show ? 0 : 1;
            _pauseMenu.SetActive(show);
        }

        internal static void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void LoadGameFinishedScreen()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameOver");
        }

        internal void GameOver()
        {
            StartCoroutine(ShowGameOverScreen());
            LoadStats();
        }

        public void LoadStats()
        {
            GameStats.enemiesKilled = _enemiesKilled;
            GameStats.secondsSurvived = secondsPassed;
            GameStats.levelReached = Player.Instance.Level;
            GameStats.structuresUpgraded = structuresUpgraded;

            List<PassiveItemSO> passiveItems = new List<PassiveItemSO>();
            List<StructureItemSO> structures = new List<StructureItemSO>();

            foreach(PassiveItem pi in PassiveItemManager.Instance.GetPassives())
            {
                passiveItems.Add(pi.item);
            }

            foreach (Structure st in StructureManager.Instance.GetStructures())
            {
                structures.Add(st.item);
            }

            GameStats.passiveItems = passiveItems;
            GameStats.structures = structures;
        }

        private IEnumerator ShowGameOverScreen()
        {
            yield return new WaitForSeconds(1);
            _gameOverScreen.SetActive(true);
            int rand = Random.Range(1, 5);
            _gameOverScreen.GetComponent<Animator>().SetTrigger("over" +rand);
            SuperPauseGame(true);
        }
    }
}
