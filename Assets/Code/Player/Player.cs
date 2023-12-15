using System.Collections;
using System.Collections.Generic;
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

        public Transform PassiveItems;
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
        #endregion

        #region Player Stats & Buffs
        [Header("Stats & Buffs")]
        public float rangeIncrease = 0f;
        public float damageIncrease = 0f;
        public float coolDownReduction = 0f;
        public float areaSizeIncrease = 0f;
        public float projectileSpeedBoost = 0f;
        public float durationIncrease = 0f;
        public int ProjectileAmntIncrease = 0;

        public float speedBoost = 0f;
        public float visionBoost = 0f;
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

        public void AddPassiveItem(ItemSO item)
        {
            GameObject obj = Instantiate(item.prefab, PassiveItems);
            obj.GetComponent<PassiveItem>().ApplyEffect();
            ApplyBuffs();
        }

        public void ApplyBuffs()
        {
            Structure[] structures = StructureManager.Instance.GetStructures();

            foreach(Structure s in structures)
            {
                s.range += rangeIncrease;
                s.damage += damageIncrease;
                s.attackCooldown -= s.attackCooldown * coolDownReduction;
                s.areaSize += areaSizeIncrease;
                s.projectileSpeed += projectileSpeedBoost;
                s.duration += durationIncrease;
                s.projectileAmnt += ProjectileAmntIncrease;

            }
            PlayerInput.Speed = PlayerInput.initialSpeed + speedBoost;
            Camera.main.orthographicSize = 5 + visionBoost;
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
                XpForNextLevel += 10;
            }
            else if(Level < 40)
            {
                XpForNextLevel += 13;
            }
            else
            {
                XpForNextLevel += 16;
            }

            e_xpChanged.Invoke(0, XpForNextLevel);

            //TODO: Display LevelUp Menu

            if (_xp >= XpForNextLevel) //Remember to take this into account
            {
                LevelUp();
            }
        }
        #endregion


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
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
