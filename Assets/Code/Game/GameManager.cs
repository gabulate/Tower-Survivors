using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.GUI;
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
        public SoundClip gameOverSound;
        public SoundClip gameWonSound;
        public CharacterSO defaultCharacter;

        [Header("Game config")]
        [SerializeField]
        private bool _randomizeSpawn = true;
        [SerializeField]
        private bool _developerMode = false;

        [Header("Game state")]
        public static bool isPaused = false;
        public static bool isSuperPaused = false; //When super paused, the player can't unpause. eg: when the level up menu shows
        public static bool gameEnding = false;

        [Header("Game Stats")]
        public float secondsPassed = 0;
        private uint _enemiesKilled = 0;
        private uint _coinsCollected = 0;
        public uint structuresUpgraded = 0;

        public UnityEvent<bool> e_Paused;
        public UnityEvent<uint> e_KillCountUpdated;
        public UnityEvent<uint> e_CoinCountUpdated;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            if (CharacterSelector.selected)
                Instantiate(CharacterSelector.selected.prefab, Vector3.zero, Quaternion.identity);
            else
                Instantiate(defaultCharacter.prefab, Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            if (!_developerMode && !SaveSystem.csd.developerMode)
                return;

            if (Input.GetKeyDown(KeyCode.G)) //Inventory debugger
            {
                AssetsHolder.Instance.inventoryDebugger.Toggle();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                AssetsHolder.Instance.HUD.gameObject.SetActive(
                    !AssetsHolder.Instance.HUD.gameObject.activeSelf);
                Cursor.visible = AssetsHolder.Instance.HUD.gameObject.activeSelf;
            }
            if (Input.GetKeyDown(KeyCode.K)) //Kill all enemies
            {
                EnemySpawner.Instance.KillAllEnemies();
            }
            if (Input.GetKeyDown(KeyCode.J)) //Enable spawning
            {
                EnemySpawner.Instance.activeSpawning = true;
            }
            if (Input.GetKeyDown(KeyCode.H)) //Spawn boss
            {
                EnemySpawner.Instance.SpawnBoss();
            }
            if (Input.GetKeyDown(KeyCode.I)) //Toggle invincibility
            {
                Player.Health.isInvincible = !Player.Health.isInvincible;
                if (Player.Health.isInvincible)
                    Player.Sprite.material.SetFloat("_ShowOutline_ON", 0.0f);
                else
                    Player.Sprite.material.SetFloat("_ShowOutline_ON", 1.0f);
            }
            if (Input.GetKeyDown(KeyCode.L)) //Level Up
            {
                if (!isSuperPaused)
                {
                    Player.Instance.AddXp(Player.Instance.XpForNextLevel);
                }
            }
        }

        public void AddToKillCount(uint amount)
        {
            _enemiesKilled += amount;
            e_KillCountUpdated.Invoke(_enemiesKilled);
        }

        public void AddCoins(uint amount)
        {
            _coinsCollected += amount;
            e_CoinCountUpdated.Invoke(_coinsCollected);
        }

        private void Start()
        {

            secondsPassed = 0;
            _enemiesKilled = 0;
            structuresUpgraded = 0;
            GameSettings.LoadSettings();
            _pauseMenu.SetActive(false);
            _gameOverScreen.SetActive(false);
            Time.timeScale = 1;
            SuperPauseGame(false);


            Player.PlayerInput.EnableMovement(true);


            if (CharacterSelector.selected)
                Player.Inventory.AddItem(CharacterSelector.selected.startingItem);
            else
                Player.Inventory.AddItem(defaultCharacter.startingItem);

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

            if (!gameEnding)
            {
                //If 30 minutes have passed
                if(secondsPassed > 1800)
                {
                    EnemySpawner.Instance.KillAllEnemies();
                    EnemySpawner.Instance.SpawnBoss();
                    gameEnding = true;
                }
            }
        }

        public void LevelUp()
        {
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
            e_Paused.Invoke(show);
            Time.timeScale = show ? 0 : 1;
            _pauseMenu.SetActive(show);
            _pauseMenu.GetComponent<PauseMenu>().UpdateStats();
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

        public void GameWon()
        {
            GameStats.gameWon = true;
            Player.PlayerInput.EnableMovement(false);
            AssetsHolder.Instance.HUD.gameObject.SetActive(false);
            StartCoroutine(ShowGameWonScreen());
            AudioPlayer.Instance.LowerVolume(true);
            LoadStats();
        }

        internal void GameOver()
        {
            Player.PlayerInput.EnableMovement(false);
            StartCoroutine(ShowGameOverScreen());
            AudioPlayer.Instance.LowerVolume(true);
            LoadStats();
        }

        public void LoadStats()
        {
            GameStats.enemiesKilled = _enemiesKilled;
            GameStats.coinsCollected = _coinsCollected;
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
            yield return new WaitForSeconds(0.5f);
            AudioPlayer.Instance.StopMusic();
            AudioPlayer.Instance.PlaySFX(gameOverSound);
            _gameOverScreen.SetActive(true);
            int rand = Random.Range(1, 5);
            _gameOverScreen.GetComponent<Animator>().SetTrigger("over" +rand);
            SuperPauseGame(true);
        }

        private IEnumerator ShowGameWonScreen()
        {
            yield return new WaitForSeconds(5);
            AudioPlayer.Instance.StopMusic();
            AudioPlayer.Instance.PlaySFX(gameWonSound);
            _gameOverScreen.SetActive(true);
            _gameOverScreen.GetComponent<Animator>().SetTrigger("won");
            yield return new WaitForSeconds(1.5f);
            SuperPauseGame(true);
            LoadGameFinishedScreen();
        }
    }
}
