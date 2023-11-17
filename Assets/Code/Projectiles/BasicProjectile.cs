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
        public int passThrough = 0;
        public float speed = 0;
        public Vector3 direction;

        public virtual void SetAttributes(float damage, int passThrough, float speed, Vector3 direction)
        {
            this.damage = damage;
            this.passThrough = passThrough;
            this.speed = speed;
            this.direction = direction;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
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
