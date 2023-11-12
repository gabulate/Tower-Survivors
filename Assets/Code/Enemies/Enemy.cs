using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.Enemies
{
    /// <summary>
    /// Parent class of enemies meant to be extended.
    /// Has basic health and damage functions.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        protected static readonly LayerMask CheckLayers = 1 << 3;

        protected Animator _animator;
        protected Rigidbody2D _rb;

        public float HP = 100f;
        public float invulnerableTime = 0.2f;
        public bool isInvincible = false;

        public float speed = 1f;
        public float damage = 10f;
        public float attackCooldown = 1f;
        protected GameObject _player;


        protected SpriteRenderer _sprite;

        public UnityEvent<Enemy> e_Die = new UnityEvent<Enemy>();

        // Start is called before the first frame update
        void Start()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _rb);
            TryGetComponent(out _sprite);
            _player = PlayerScripts.Player.Instance.gameObject;
        }
        private void FixedUpdate()
        {
            Move();
        }

        #region Movement
        private void Move()
        {
            Vector3 targetPosition = _player.transform.position;

            // Calculate the direction to move towards
            Vector2 moveDirection = (targetPosition - transform.position).normalized;

            // Apply force to the Rigidbody
            _rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);

            // Flip the sprite based on the movement direction
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
            }

            if (gameObject.activeSelf)
                StartCoroutine(Invincivility(invulnerableTime));
        }

        IEnumerator Invincivility(float seconds)
        {
            isInvincible = true;

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
