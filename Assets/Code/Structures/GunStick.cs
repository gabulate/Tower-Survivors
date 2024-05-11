using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class GunStick : Structure
    {
        [SerializeField]
        protected Transform _gun;

        protected override void FixedUpdate()
        {
            if (!canAttack)
                return;

            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            stats.currentCooldown = stats.currentCooldown <= 0 ? 0 : stats.currentCooldown - Time.fixedDeltaTime;
            RotateGun();

            if (stats.currentCooldown <= 0)
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            //Spawns the amount of projectiles given by calling the Coroutine multiple times each other with some delay.
            for (int i = 0; i < stats.projectileAmnt; i++)
            {
                StartCoroutine(SpawnProjectile(i * stats.timeBetweenMultipleShots));
            }
            //Reset cooldown
            stats.currentCooldown = stats.attackCooldown + stats.projectileAmnt * stats.timeBetweenMultipleShots;
        }

        protected override IEnumerator SpawnProjectile(float delay)
        {
            yield return new WaitForSeconds(delay);
            //Instantiates the projectile////////////////////////////////////////////////
            GameObject e = Instantiate(prefab, _firePoint.position, _firePoint.rotation);
            //Calculates the direction of the target
            Vector2 direction = _firePoint.transform.right;
            //Sets the corresponding attributes to the projectile
            e.GetComponent<BasicProjectile>().SetAttributes(stats, direction);

            StartCoroutine(RotateGun());

            //Play firing animation and firing sound
            _animator.SetTrigger("fire");
            AudioPlayer.Instance.PlaySFX(firingSound, transform.position);
        }

        /// <summary>
        /// Rotates the gun
        /// </summary>
        protected IEnumerator RotateGun()
        {
            float duration = stats.timeBetweenMultipleShots;
            float rotation = Random.Range(180, 540);
            float timePassed = 0;

            while (timePassed < duration)
            {
                // Calculate the rotation increment per frame based on the total rotation and duration
                float rotationIncrement = rotation * Time.deltaTime / duration;

                _gun.rotation *= Quaternion.Euler(0, 0, rotationIncrement);

                // Add a slight shake effect to the transform position
                Vector3 shakeOffset = new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), 0);
                _gun.position += shakeOffset;

                timePassed += Time.deltaTime;
                yield return null; 
            }

            _gun.localPosition = Vector3.zero;
        }
    }
    }
