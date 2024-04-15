using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using TowerSurvivors.Structures;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class FireCan : BasicProjectile
    {
        [SerializeField]
        private SoundClip _explosionSound;
        private bool _exploded = false;
        [SerializeField]
        private GameObject _fireArea;

        public override void SetAttributes(StructureStats stats, Vector2 direction)
        {
            base.SetAttributes(stats, direction);
            _timeLeft /= 1.2f;
        }

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

            AreaDamage ad = Instantiate(_fireArea, transform.position, Quaternion.identity).GetComponent<AreaDamage>();
            ad.SetAttributes(duration, transform.localScale.x, damage, 0.25f);

            AudioPlayer.Instance.PlaySFX(_explosionSound, transform.position);

        }
    }
}
