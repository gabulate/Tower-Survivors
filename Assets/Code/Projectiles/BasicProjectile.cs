using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
using TowerSurvivors.Structures;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    /// <summary>
    /// Basic Projectile script that does damage when touching an enemy
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class BasicProjectile : MonoBehaviour
    {
        protected static readonly LayerMask _enemyLayer = 1 << 6;

        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected Rigidbody2D _rb;
        [SerializeField] 
        protected Collider2D _collider;
        protected bool _enough = false;

        protected float _timeLeft;

        public float damage = 1;
        public float speed = 0;
        public float duration = 5f;
        public int passThrough = 0; //The amount of enemies the projectile can go through before destroying itself
        public Vector2 direction;

        /// <summary>
        /// Sets the projectile's attributes.
        /// </summary>
        /// <param name="damage">Amount of damage the projetile deals.</param>
        /// <param name="passThrough">The amount of enemies the projectile can go through before destroying itself.</param>
        /// <param name="speed">Speed of the projectile.</param>
        /// <param name="direction">Direction the projetile will move towards.</param>
        /// <param name="duration">Amount of time the projectile will exist before despawning.</param>
        /// <param name="size">The scale size of the projectile</param>
        public virtual void SetAttributes(StructureStats stats, Vector2 direction)
        {
            this.damage = stats.damage;
            this.passThrough = stats.passThroughAmnt;
            this.speed = stats.projectileSpeed;
            this.direction = direction;
            this.duration = stats.duration;
            _timeLeft = stats.duration;
            transform.localScale = new Vector3(stats.areaSize, stats.areaSize, stats.areaSize);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enough)
                return;

            //When hitting an enemy, deals damage and subtract the passThrough property by 1.
            if(_enemyLayer == (_enemyLayer | (1 << collision.gameObject.layer)))
            {
                
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                passThrough--;
                //If it can't pass through anymore enemies, destroys itself.
                if(passThrough <= -1)
                {
                    DestroyAnim();
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            //Moves the projectile at a uniform speed in the direction given.
            _rb.velocity = direction * speed;


            if (_timeLeft <= 0)
            {
                DestroyAnim();
            }
            else
            {
                _timeLeft -= Time.fixedDeltaTime;
            }
        }

        protected virtual void DestroyAnim()
        {
            _enough = true;
            _collider.enabled = false;
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            Destroy(gameObject, 1);
        }
    }
}
