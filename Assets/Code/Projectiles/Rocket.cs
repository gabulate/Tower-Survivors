using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class Rocket : BasicProjectile
    {
        [SerializeField]
        private SoundClip _explosionSound;
        private bool _exploded = false;

        protected override void FixedUpdate()
        {
            //Moves the projectile at a uniform speed in the direction given.
            _rb.velocity = direction * speed;


            if (_timeLeft <= 0)
            {
                Explode();
                DestroyAnim();
            }
            else
            {
                _timeLeft -= Time.fixedDeltaTime;
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enough)
                return;

            //When hitting an enemy, deals damage and subtract the passThrough property by 1.
            if (_enemyLayer == (_enemyLayer | (1 << collision.gameObject.layer)))
            {
                Enemy e = collision.gameObject.GetComponent<Enemy>();
                if (!e.isAlive)
                    return;

                Explode();

                passThrough--;
                
                DestroyAnim();
            }
        }

        //Gets all the enemies in range and damages them
        private void Explode()
        {
            if (_exploded)
                return;

            _exploded = true;
            _rb.velocity = Vector2.zero;
            speed = 0;
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/1.5f, _enemyLayer);

            AudioPlayer.Instance.PlaySFX(_explosionSound, transform.position);
            foreach(Collider2D col in hits)
            {
                col.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
}
