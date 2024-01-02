using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that gives static acces to player scripts, stats and components.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Player Controllers
        public static Player Instance { get; private set; }
        public static PlayerHealth Health { get; private set; }
        public static PlayerInputController PlayerInput { get; private set; }
        public static InventoryManager Inventory { get; private set; }
        public static SpriteRenderer Sprite { get; private set; }
        public static Animator PlayerAnimator { get; private set; }
        #endregion

        #region Player Properties
        [Header("Player Level Properties")]
        public ItemSO startingItem;

        [SerializeField]
        private int _level = 1;
        public int Level
        {
            get => _level;
            private set => _level = value;
        }

        [SerializeField]
        private int _xp = 0;
        [SerializeField]
        private int _XpForNextLevel = 5;
        public int XpForNextLevel
        {
            get => _XpForNextLevel;
            private set => _XpForNextLevel = value;
        }
        public int TotalXpCollected = 0;
        public UnityEvent<int, int> e_xpChanged;
        public UnityEvent<int> e_leveledUp;
        #endregion

        #region Player Stats & Buffs
        [Header("Stats & Buffs")]
        public PlayerStats stats;
        #endregion

        void Awake()
        {
            Instance = this;
            Health = GetComponent<PlayerHealth>();
            PlayerInput = GetComponent<PlayerInputController>();
            Sprite = GetComponentInChildren<SpriteRenderer>();
            PlayerAnimator = GetComponentInChildren<Animator>();
            Inventory = GetComponent<InventoryManager>();
        }

        private void Start()
        {
            Inventory.AddItem(startingItem);
        }

        public void ApplyBuffs()
        {
            //Set every buff to 0 to not apply buffs on top of eachother
            stats.rangeIncrease = 0f;
            stats.damageIncrease = 0f;
            stats.coolDownReduction = 0f;
            stats.areaSizeIncrease = 0f;
            stats.projectileSpeedBoost = 0f;
            stats.durationIncrease = 0f;
            stats.ProjectileAmntIncrease = 0;
            stats.speedBoost = 0f;
            stats.visionBoost = 5f;
            stats.extraStructures = 0;

            Structure[] structures = StructureManager.Instance.GetStructures();
            PassiveItem[] passives = PassiveItemManager.Instance.GetPassives();

            foreach (PassiveItem p in passives)
            {
                p.ApplyEffect();
            }

            foreach(Structure s in structures)
            {
                s.ApplyBuffs(stats);
            }
            //Buffs not related to structures
            PlayerInput.Speed = PlayerInput.initialSpeed + stats.speedBoost;
            Camera.main.orthographicSize = stats.visionBoost;
            StructureManager.Instance.IncreaseStructureLimit(stats.extraStructures);
        }

        internal void RemoveAllBuffs()
        {
            //Set every buff to 0 to not apply buffs on top of eachother
            stats.rangeIncrease = 0f;
            stats.damageIncrease = 0f;
            stats.coolDownReduction = 0f;
            stats.areaSizeIncrease = 0f;
            stats.projectileSpeedBoost = 0f;
            stats.durationIncrease = 0f;
            stats.ProjectileAmntIncrease = 0;
            stats.speedBoost = 0f;
            stats.visionBoost = 0f;
        }

        #region Level and Xp

        /// <summary>
        /// Adds xp to the player and levels up when necessary.
        /// </summary>
        /// <param name="xp">Amount of xp to be added.</param>
        public void AddXp(int xp)
        {
            _xp += xp;
            TotalXpCollected += xp;
            e_xpChanged.Invoke(_xp, XpForNextLevel);

            if(_xp >= XpForNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;

            _xp -= XpForNextLevel;

            if(Level < 20)
            {
                XpForNextLevel += 5;
            }
            else if(Level < 40)
            {
                XpForNextLevel += 10;
            }
            else
            {
                XpForNextLevel += 15;
            }

            e_xpChanged.Invoke(_xp, XpForNextLevel);
            e_leveledUp.Invoke(Level);

            //TODO: Display LevelUp Menu
            GameManager.Instance.LevelUp();

        }
        #endregion

        public void CheckForLevelUp()
        {
            if (_xp >= XpForNextLevel) 
            {
                LevelUp();
            }
        }

        /// <summary>
        /// Dies! :(
        /// Restarts the level and other things.
        /// </summary>
        public void Die()
        {
            StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel()
        {
            GameManager.Instance.Restart();
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    [System.Serializable]
    public class PlayerStats
    {
        public float rangeIncrease = 0f;
        public float damageIncrease = 0f;
        public float coolDownReduction = 0f;
        public float areaSizeIncrease = 0f;
        public float projectileSpeedBoost = 0f;
        public float durationIncrease = 0f;
        public int ProjectileAmntIncrease = 0;
        public float speedBoost = 0f;
        public float visionBoost = 0f;
        public int extraStructures = 0;
    }
}
