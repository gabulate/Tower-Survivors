using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class Roundabout : Structure
    {
        [SerializeField]
        protected Transform _cannon;

        [SerializeField]
        private float _rotationSpeed = 30;

        protected override void FixedUpdate()
        {
            if (!canAttack)
                return;

            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            stats.currentCooldown = stats.currentCooldown <= 0 ? 0 : stats.currentCooldown - Time.fixedDeltaTime;
            Rotate();

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
            stats.currentCooldown = stats.attackCooldown;
        }

        protected override IEnumerator SpawnProjectile(float delay)
        {
            yield return new WaitForSeconds(delay);
            //Instantiates the projectile////////////////////////////////////////////////
            GameObject e = Instantiate(prefab, _firePoint.position, _firePoint.rotation);
            //Calculates the direction of the target
            Vector2 direction = _cannon.up;
            direction.Normalize();
            //Sets the corresponding attributes to the projectile
            e.GetComponent<BasicProjectile>().SetAttributes(stats, direction);

            e.transform.localScale = new Vector3(stats.areaSize, stats.areaSize, 1);

            //Play firing animation and firing sound
            _animator.SetTrigger("fire");
            AudioPlayer.Instance.PlaySFX(firingSound, transform.position);
        }

        private void Rotate()
        {
            float rotation = _cannon.localRotation.eulerAngles.z - _rotationSpeed * Time.fixedDeltaTime;
            //returns to 0
            if (rotation <= -360)
                rotation += 360;

            _cannon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }
    }
}
