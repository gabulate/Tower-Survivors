using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class Queen : Structure
    {
        //All the 8 straight and diagonal directions
        protected static readonly Vector2[] directions = { Vector2.up, Vector2.one, Vector2.right, new Vector2(1, -1),
                          Vector2.down, new Vector2(-1, -1), Vector2.left, new Vector2(-1, 1) };

        protected override void FixedUpdate()
        {
            if (!canAttack)
                return;

            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            stats.currentCooldown = stats.currentCooldown <= 0 ? 0 : stats.currentCooldown - Time.fixedDeltaTime;

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
            //Spawns 8 projectiles, each with a diferent direction
            //Sets the corresponding attributes to the projectile
            foreach (Vector2 direction in directions)
            {
                GameObject e = Instantiate(prefab, _firePoint.position, _firePoint.rotation);
                e.GetComponent<BasicProjectile>().SetAttributes(stats, direction);
            }

            //Play firing animation and firing sound
            _animator.SetTrigger("fire");
            AudioPlayer.Instance.PlaySFX(firingSound, transform.position);
        }
    }
}
