using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class RocketLauncher : Structure
    {
        [SerializeField]
        protected Transform _cannon;

        protected override void FixedUpdate()
        {
            if (!canAttack)
                return;

            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            stats.currentCooldown = stats.currentCooldown <= 0 ? 0 : stats.currentCooldown - Time.fixedDeltaTime;
            RotateCannon();

            if (stats.currentCooldown <= 0)
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (targetEnemy == null || !targetEnemy.isAlive)
            {
                if (!GetRandomTarget())
                {
                    return;
                }
            }

            //Store the position in a different variable to avoid null references
            _targetPos = targetEnemy.transform.position;

            //if target has left the range, forget it
            if (Vector2.Distance(transform.position, _targetPos) > stats.range)
            {
                targetEnemy = null;
                return;
            }

            RotateCannon();

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

            if (targetEnemy == null || !targetEnemy.isAlive)
            {
                if (GetRandomTarget())
                    _targetPos = targetEnemy.transform.position;
            }
            //Instantiates the projectile////////////////////////////////////////////////
            GameObject p = Instantiate(prefab, _firePoint.position, _firePoint.rotation);
            //Calculates the direction of the target
            Vector2 direction = _targetPos - p.transform.position;
            direction.Normalize();
            //Sets the corresponding attributes to the projectile
            p.GetComponent<BasicProjectile>().SetAttributes(stats, direction);

            //Play firing animation and firing sound
            _animator.SetTrigger("fire");
            AudioPlayer.Instance.PlaySFX(firingSound, transform.position);
        }

        //Tries to find the closest target and returns true if it finds one within range
        protected bool GetRandomTarget()
        {
            //GET THE CLOSEST TARGET WITHIN RANGE/////////////////////////
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stats.range, _enemyLayer);

            //If didn't find any targets, return
            if (hits.Length == 0)
                return false;

            int rand = Random.Range(0, hits.Length);

            Transform targetTransform = hits[rand].transform;

            targetEnemy = targetTransform.GetComponent<Enemy>();

            return true;
        }

        /// <summary>
        /// Rotates the cannon's sprite for visual feedback
        /// </summary>
        protected void RotateCannon()
        {
            if (targetEnemy == null)
                return;

            var dir = targetEnemy.transform.position - _cannon.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //Flip the sprite based on the target's position
            if ((transform.position.x - targetEnemy.transform.position.x) < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                angle -= 180;
            }

            _cannon.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

