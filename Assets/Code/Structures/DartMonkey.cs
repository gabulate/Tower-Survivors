using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class DartMonkey : Structure
    {
        [SerializeField]
        private Transform _monke;
        protected override void FixedUpdate()
        {
            if (!canAttack)
                return;

            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            stats.currentCooldown = stats.currentCooldown <= 0 ? 0 : stats.currentCooldown - Time.fixedDeltaTime;
            RotateMonke();

            if (stats.currentCooldown <= 0)
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (targetEnemy == null || !targetEnemy.isAlive)
            {
                if (!GetClosestTarget())
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

            RotateMonke();

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
            Vector2 direction = _targetPos - e.transform.position;
            direction.Normalize();
            //Sets the corresponding attributes to the projectile
            e.GetComponent<BasicProjectile>().SetAttributes(stats, direction);

            //Play firing animation and firing sound
            _animator.SetTrigger("fire");
            AudioPlayer.Instance.PlaySFX(firingSound, transform.position);
        }

        //Tries to find the closest target and returns true if it finds one within range
        protected bool GetClosestTarget()
        {
            //GET THE CLOSEST TARGET WITHIN RANGE/////////////////////////
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stats.range, _enemyLayer);

            //If didn't find any targets, return
            if (hits.Length == 0)
                return false;


            float closestDistance = float.MaxValue;

            foreach (Collider2D col in hits)
            {
                Transform targetTransform = col.transform;

                //Calculate the distance between the current target and the current position.
                float distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);

                //Check if the current target is closer than the previous closest target.
                if (distanceToTarget < closestDistance)
                {
                    targetEnemy = targetTransform.GetComponent<Enemy>();
                    closestDistance = distanceToTarget;
                }
            }
            return true;
        }

        protected void RotateMonke()
        {
            if (targetEnemy == null)
                return;
            //Flip the sprite based on the target's position
            if ((transform.position.x - targetEnemy.transform.position.x) > 0)
            {
                _monke.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                _monke.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
