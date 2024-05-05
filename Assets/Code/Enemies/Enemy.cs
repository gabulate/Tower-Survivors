using System.Collections;
using TowerSurvivors.Audio;
using TowerSurvivors.Game;
using TowerSurvivors.GUI;
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

        [Header("Must Have References")]
        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected Rigidbody2D _rb;
        [SerializeField]
        protected Collider2D _collider;
        [SerializeField]
        protected SpriteRenderer _sprite;
        [SerializeField]
        protected SoundClip hurtSound;

        [Header("MetaAttributes")]
        public bool isAlive = true;
        public bool scaleXp = true;
        [SerializeField]
        protected float _timeAlive = 0;
        [Range(0, 1)]
        public float ChanceToDropXp = 0.4f;
        public uint Xp = 1;

        [Header("Health")]
        public float HP = 100f;
        public float invulnerableTime = 0.2f;
        public bool isInvincible = false;
        public float stunTime = 0.1f;
        public bool isStunned = false;

        [Header("Attack")]
        protected Vector2 targetPosition = new Vector2();
        public float speed = 1f;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;

        protected void Start()
        {
            if (scaleXp && Player.Instance.Level >= 10)
            {
                HP += HP * (Player.Instance.Level / 100f); //Adds % of health according to the Player level
            }
        }

        protected virtual void FixedUpdate()
        {
            //currentCooldown is always counting down to 0, when it reaches 0, the enemy is allowed to attack
            currentCooldown = currentCooldown <= 0 ? 0 : currentCooldown - Time.fixedDeltaTime;

            //If the enemy has been alive for a long time, it starts gaining speed
            //This is to avoid having the player just run around the map
            _timeAlive += Time.fixedDeltaTime;
            if(_timeAlive > 200)
            {
                speed += 10 * Time.fixedDeltaTime;
            }
            Move();
        }

        #region Attack
        /// <summary>
        /// Checks if is touching the player and attacks if not on cooldown
        /// </summary>
        /// <param name="collision"></param>
        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
                if (currentCooldown <= 0 && isAlive)
                    AttackPlayer();
        }

        /// <summary>
        /// Gets the player health component and does damage accordingly
        /// </summary>
        protected virtual void AttackPlayer()
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
        protected virtual void Move()
        {
            if (!isAlive || isStunned)
            {
                _rb.velocity = Vector2.zero;
                return;
            }


            targetPosition.x = Player.Instance.transform.position.x;
            targetPosition.y = Player.Instance.transform.position.y;

            //Calculate the direction to move towards
            Vector2 moveDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

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
        /// <param name="countIfKilled">If the enemy is killed, its added to the kill count. 
        /// True by default, only false in special cases.</param>
        public void TakeDamage(float damage, bool countIfKilled = true)
        {
            if (isInvincible)
            {
                return;
            }

            HP -= damage;

            try
            {
                AudioPlayer.Instance.PlaySFX(hurtSound, transform.position);
                DamageNumberController.Instance.ShowDamageNumber((int)damage, transform.position);
            }
            catch { }
            //Debug.Log("Damage taken: " + damage);

            StartCoroutine(Invincivility(invulnerableTime));

            if (HP <= 0)
            {
                Die(countIfKilled);
            } else
                StartCoroutine(Stun(stunTime));
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

        protected IEnumerator Stun(float seconds)
        {
            isStunned = true;

            float elapsedTime = 0f;

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isStunned = false;
        }

        /// <summary>
        /// Dies! :(
        /// </summary>
        public virtual void Die(bool addToKillCount)
        {
            isAlive = false;

            if(addToKillCount)
                GameManager.Instance.AddToKillCount(1);

            DropXp();
            EnemySpawner.Instance.currentEnemies--;
            DestroyAnim();
        }

        //Kills the enemy without adding it to the kill count
        /// <summary>
        /// Decides to drop xp or not based on RNG.
        /// Gets the xp Object from a pool.
        /// </summary>
        protected void DropXp()
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= AssetsHolder.Instance.cookieChance)
            {
                Instantiate(AssetsHolder.Instance.cookiePrefab, transform.position, Quaternion.identity);
            }
            else if (randomValue <= ChanceToDropXp)
            {
                //Spawn cookie
                XpObjectPool.Instance.SpawnXp(Xp, transform.position);
            }

            randomValue = Random.Range(0f, 1f);
            if(randomValue <= AssetsHolder.Instance.coinChance)
                Instantiate(AssetsHolder.Instance.coinPrefab, transform.position, Quaternion.identity);

        }

        protected void DestroyAnim()
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
