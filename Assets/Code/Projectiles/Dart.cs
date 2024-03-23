using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class Dart : BasicProjectile
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_enough)
                return;

            //When hitting an enemy, deals damage and subtract the passThrough property by 1.
            if (_enemyLayer == (_enemyLayer | (1 << other.gameObject.layer)))
            {
                Enemy e = other.gameObject.GetComponent<Enemy>();
                if (!e.isAlive)
                    return;

                e.TakeDamage(damage);
                passThrough--;
                //If it can't pass through anymore enemies, destroys itself.
                if (passThrough <= -1)
                {
                    DestroyAnim();
                }
            }
        }
        protected override void DestroyAnim()
        {
            _enough = true;
            _collider.enabled = false;
            speed = 0;
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            Destroy(gameObject, 1);
        }
    }
}
