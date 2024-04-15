using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class Disk : BasicProjectile
    {
        [SerializeField]
        private SoundClip _bounceSound;
        [SerializeField]
        private TrailRenderer _trail;

        protected override void FixedUpdate()
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

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enough)
                return;

            //When hitting an enemy, deals damage and subtract the passThrough property by 1.
            if (_enemyLayer == (_enemyLayer | (1 << collision.gameObject.layer)))
            {
                Enemy e = collision.gameObject.GetComponent<Enemy>();
                if (!e.isAlive)
                    return;

                Bounce(collision);

                e.TakeDamage(damage);
                passThrough--;
                //If it can't pass through anymore enemies, destroys itself.
                if (passThrough <= -1)
                {
                    DestroyAnim();
                }
            }
        }

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

                Bounce(other);

                e.TakeDamage(damage);
                passThrough--;
                //If it can't pass through anymore enemies, destroys itself.
                if (passThrough <= -1)
                {
                    DestroyAnim();
                }
            }
        }

        private void Bounce(Collision2D collision)
        {
            // Get the normal vector of the surface the object collided with
            Vector2 surfaceNormal = collision.contacts[0].normal;

            // Calculate the reflected direction vector
            direction = Vector2.Reflect(direction, surfaceNormal);

            _timeLeft += 0.2f;

            AudioPlayer.Instance.PlaySFX(_bounceSound, transform.position);
        }

        private void Bounce(Collider2D collision)
        {
            // Get the normal vector of the surface the object collided with
            Vector2 surfaceNormal = new Vector2(collision.transform.position.x, collision.transform.position.y).normalized;

            // Calculate the reflected direction vector
            direction = Vector2.Reflect(direction, surfaceNormal);

            AudioPlayer.Instance.PlaySFX(_bounceSound, transform.position);
        }

        protected override void DestroyAnim()
        {
            base.DestroyAnim();
            _trail.enabled = false;
        }
    }
}
