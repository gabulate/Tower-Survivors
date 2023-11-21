using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
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
        protected Animator _animator;
        protected Rigidbody2D _rb;

        public float damage = 1;
        public int passThrough = 0; //The amount of enemies the projectile can go through before destroying itself
        public float speed = 0;
        public Vector3 direction;

        /// <summary>
        /// Sets the projectile's attributes.
        /// </summary>
        /// <param name="damage">Amount of damage the projetile deals.</param>
        /// <param name="passThrough">The amount of enemies the projectile can go through before destroying itself.</param>
        /// <param name="speed">Speed of the projectile.</param>
        /// <param name="direction">Direction the projetile will move towards.</param>
        public virtual void SetAttributes(float damage, int passThrough, float speed, Vector3 direction)
        {
            this.damage = damage;
            this.passThrough = passThrough;
            this.speed = speed;
            this.direction = direction;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            //When hitting an enemy, deals damage and subtract the passThrough property by 1.
            if(_enemyLayer == (_enemyLayer | (1 << collision.gameObject.layer)))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                passThrough--;
                if(passThrough < 0)
                {
                    StartCoroutine(DestroyAnim());
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            //Moves the projectile at a uniform speed in the direction given.
            _rb.velocity = direction * speed;
        }

        protected virtual IEnumerator DestroyAnim()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}
