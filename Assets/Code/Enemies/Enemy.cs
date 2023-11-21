using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.Enemies
{
    /// <summary>
    /// Main enemy script that can be extended.
    /// Has basic health, movement and damage functions.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        protected static readonly LayerMask _playerLayer = 1 << 3;

        protected Animator _animator;
        protected Rigidbody2D _rb;

        public float HP = 100f;
        public float invulnerableTime = 0.2f;
        public bool isInvincible = false;

        public float speed = 1f;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;

        protected SpriteRenderer _sprite;

        public UnityEvent<Enemy> e_Die = new UnityEvent<Enemy>();

        void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _rb);
            TryGetComponent(out _sprite);
        }
        private void FixedUpdate()
        {
            //currentCooldown is always counting down to 0, when it reaches 0, the enemy is allowed to attack
            currentCooldown = currentCooldown <= 0 ? 0 : currentCooldown - Time.fixedDeltaTime;
            Move();
        }

        #region Attack
        /// <summary>
        /// Checks if is touching the player and attacks if not on cooldown
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (currentCooldown > 0)
                return;

            if(_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
            {
                //Debug.Log(gameObject.name + " HIT for " + damage + " damage!");
                Player.Health.TakeDamage(damage);

                //Resets the cooldown
                currentCooldown = attackCooldown;
            }
        }

        #endregion

        #region Movement
        /// <summary>
        /// Moves Towards the target position at a uniform speed.
        /// </summary>
        private void Move()
        {
            Vector3 targetPosition = Player.Instance.transform.position;

            //Calculate the direction to move towards
            Vector2 moveDirection = (targetPosition - transform.position).normalized;

            //Apply force to the Rigidbody
            _rb.velocity = new Vector2(moveDirection.x * speed * Time.fixedDeltaTime, moveDirection.y * speed * Time.fixedDeltaTime);

            //Flip the sprite based on the movement direction
            if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        #endregion

        #region Health

        /// <summary>
        /// Checks if its invincible and subtracts the amount of damage given or ignores it correspondigly.
        /// </summary>
        /// <param name="damage">Amount of damage to be taken by the enemy</param>
        public void TakeDamage(float damage)
        {
            if (isInvincible)
            {
                return;
            }

            HP -= damage;
            //Debug.Log("Damage taken: " + damage);

            if (HP <= 0)
            {
                Die();
            }
            else
            {
                //Something something
                //Maybe add a sound or visual effect in the future
            }

            if (gameObject.activeSelf)
                StartCoroutine(Invincivility(invulnerableTime));
        }

        /// <summary>
        /// Makes the enemy inmune to attacks for the amount of seconds given.
        /// </summary>
        /// <param name="seconds">Number of seconds that the enemy will be invulnerable.</param>
        /// <returns></returns>
        IEnumerator Invincivility(float seconds)
        {
            isInvincible = true;

            //Changes the enemie's color and fades it back to normal.
            Color initialColor = Color.red;
            Color targetColor = Color.white;

            float elapsedTime = 0f;

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                _sprite.material.color = Color.Lerp(initialColor, targetColor, elapsedTime / seconds);
                yield return null;
            }

            isInvincible = false;

        }

        /// <summary>
        /// Dies! :(
        /// </summary>
        public void Die()
        {
            e_Die.Invoke(this);
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            //Something something
            yield return new WaitForEndOfFrame();
            Destroy(this.gameObject);
        }
        #endregion
    }
}
