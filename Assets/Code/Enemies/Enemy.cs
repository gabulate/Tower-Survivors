using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.PickUps;
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

        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected Rigidbody2D _rb;
        [SerializeField]
        protected Collider2D _collider;

        [Range(0, 1)]
        public float ChanceToDropXp = 0.4f;
        public int Xp = 1;

        public bool isAlive = true;
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
            if (currentCooldown <= 0 && isAlive)
                if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
                    AttackPlayer();
        }

        /// <summary>
        /// Gets the player health component and does damage accordingly
        /// </summary>
        protected void AttackPlayer()
        {
            Player.Health.TakeDamage(damage);

            //Resets the cooldown
            currentCooldown = attackCooldown;
        }

        #endregion

        #region Movement
        /// <summary>
        /// Moves Towards the target position at a uniform speed.
        /// </summary>
        private void Move()
        {
            if (!isAlive)
            {
                _rb.velocity = Vector2.zero;
                return;
            }
                

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

            StartCoroutine(Invincivility(invulnerableTime));

            if (HP <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Makes the enemy inmune to attacks for the amount of seconds given.
        /// </summary>
        /// <param name="seconds">Number of seconds that the enemy will be invulnerable.</param>
        /// <returns></returns>
        IEnumerator Invincivility(float seconds)
        {
            isInvincible = true;

            float elapsedTime = 0f;
            _sprite.material.SetFloat("_Fade", 1);

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _sprite.material.SetFloat("_Fade", 0);

            isInvincible = false;
        }

        /// <summary>
        /// Dies! :(
        /// </summary>
        public void Die()
        {
            isAlive = false;
            DropXp();
            e_Die.Invoke(this);
            DestroyAnim();
        }

        /// <summary>
        /// Decides to drop xp or not based on RNG.
        /// Gets the xp Object from a pool.
        /// </summary>
        protected void DropXp()
        {
            GameObject xp = GameManager.Instance.XpPool.GetPooledObject();
            if(xp != null)
            {
                float randomValue = Random.Range(0f, 1f);
                if (randomValue > ChanceToDropXp)
                    return;

                xp.GetComponent<XpPickUp>().Xp = Xp;
                xp.transform.position = transform.position;
                xp.SetActive(true);
            }
        }

        private void DestroyAnim()
        {
            _collider.enabled = false;
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            Destroy(gameObject, 1);
        }
        #endregion
    }
}
