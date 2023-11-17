using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerSurvivors.PlayerScripts
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sprite;

        [SerializeField]
        private bool _isInvincible = false;
        //private LayerMask _enemyLayer = 1 << 6;

        public float health = 100f;
        public float invulnerableTime = 0.2f;

        private void Awake()
        {
            TryGetComponent(out _sprite);
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible)
            {
                return;
            }

            health -= damage;
            Debug.Log("Current health: " + health);
            if (health <= 0)
            {
                Die();
            }

            if (gameObject.activeSelf)
                StartCoroutine(Invincivility(invulnerableTime));
        }

        /// <summary>
        /// Makes the player inmune to attacks for the amount of seconds given.
        /// </summary>
        /// <param name="seconds">Number of seconds that the player will be invulnerable.</param>
        /// <returns></returns>
        IEnumerator Invincivility(float seconds)
        {
            _isInvincible = true;

            Color initialColor = Color.red;
            Color targetColor = Color.white;

            float elapsedTime = 0f;

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                _sprite.material.color = Color.Lerp(initialColor, targetColor, elapsedTime / seconds);
                yield return null;
            }

            _isInvincible = false;
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void Die()
        {
            Debug.Log("te has morido :(");
            StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel()
        {
            _sprite.enabled = false;
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
