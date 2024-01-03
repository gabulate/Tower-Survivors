using System.Collections;
using TowerSurvivors.Audio;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class Cannon : Structure
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
            if (targetObject == null)
            {
                if (!GetClosestTarget())
                {
                    return;
                }
            }

            //Store the position in a different variable to avoid null references
            _targetPos = targetObject.position;

            //if target has left the range, forget it
            if (Vector2.Distance(transform.position, _targetPos) > stats.range)
            {
                targetObject = null;
                return;
            }

            RotateCannon();

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

            e.transform.localScale = new Vector3(stats.areaSize, stats.areaSize, 1);

            //Play firing animation and firing sound
            _animator.ResetTrigger("fire");
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
                    targetObject = targetTransform;
                    closestDistance = distanceToTarget;
                }
            }
            return true;
        }

        /// <summary>
        /// Rotates the cannon's sprite for visual feedback
        /// </summary>
        protected void RotateCannon()
        {
            if (targetObject == null)
                return;

            var dir = targetObject.position - _cannon.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //Flip the sprite based on the target's position
            if ((transform.position.x - targetObject.position.x) > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                angle -= 180;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            _cannon.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
