using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that gives static acces to player scripts, stats and components.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public static PlayerHealth Health { get; private set; }
        public static MovementController Movement { get; private set; }

        public static SpriteRenderer Sprite { get; private set; }
        public static Animator PlayerAnimator { get; private set; }

        void Awake()
        {
            Instance = this;
            Health = GetComponent<PlayerHealth>();
            Movement = GetComponent<MovementController>();
            Sprite = GetComponentInChildren<SpriteRenderer>();
            PlayerAnimator = GetComponentInChildren<Animator>();
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
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
