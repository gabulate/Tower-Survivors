using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that keeps track of the player's health, takes damage and dies.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private bool _isInvincible = false;
        //private LayerMask _enemyLayer = 1 << 6;

        public float health = 100f;
        public float maxHealth = 100f;
        public float invulnerableTime = 0.2f;

        public UnityEvent<float, float> e_healthChanged;

        private void Start()
        {
            health = maxHealth;
            e_healthChanged.Invoke(health, maxHealth);
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible)
            {
                return;
            }

            health -= damage;
            e_healthChanged.Invoke(health, maxHealth);

            Debug.Log("Current health: " + health);
            if (health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Invincivility(invulnerableTime));
            }
        }

        /// <summary>
        /// Makes the player inmune to attacks for the amount of seconds given.
        /// </summary>
        /// <param name="seconds">Number of seconds that the player will be invulnerable.</param>
        /// <returns></returns>
        IEnumerator Invincivility(float seconds)
        {
            _isInvincible = true;

            float elapsedTime = 0f;

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                Player.Sprite.material.SetFloat("_Fade", 1 - (elapsedTime / seconds));
                yield return null;
            }

            _isInvincible = false;
        }

        /// <summary>
        /// Plays an animation when dying and calls the method Die() on The Player class
        /// </summary>
        public void Die()
        {
            _isInvincible = true;
            Player.Instance.Die();
        }


    }
}
