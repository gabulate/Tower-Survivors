using System.Collections;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class Sniper : Structure
    {
        [SerializeField]
        protected Transform _mark;

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
            if (targetEnemy == null || !targetEnemy.isAlive)
            {
                if (!GetClosestTargetToPlayer())
                {
                    return;
                }
            }

            //Store the position in a different variable to avoid null references
            _targetPos = targetEnemy.transform.position;

            //if target has left the range, forget it
            if (Vector2.Distance(transform.position, _targetPos) > stats.range)
            {
                targetObject = null;
                return;
            }

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
                if(GetClosestTargetToPlayer())
                    _targetPos = targetEnemy.transform.position;
            }

            RotateMark();

            //Instantiates the projectile////////////////////////////////////////////////
            GameObject e = Instantiate(prefab, _firePoint.position, _firePoint.rotation);

            //Sets the direction to be the position of the target
            Vector3 direction = _targetPos;
            //Sets the corresponding attributes to the projectile
            e.GetComponent<SniperBullet>().SetAttributes(stats, direction);

            //Play firing animation and firing sound
            _animator.ResetTrigger("fire");
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

        protected bool GetClosestTargetToPlayer()
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
                float distanceToTarget = Vector2.Distance(Player.Instance.transform.position, targetTransform.position);

                //Check if the current target is closer than the previous closest target.
                if (distanceToTarget < closestDistance)
                {
                    targetEnemy = targetTransform.GetComponent<Enemy>();
                    closestDistance = distanceToTarget;
                }
            }
            return true;
        }

        /// <summary>
        /// Rotates Mark's sprite for visual feedback
        /// </summary>
        protected void RotateMark()
        {
            if (targetEnemy == null)
                return;

            //Flip the sprite based on the target's position
            if ((_mark.transform.position.x - targetEnemy.transform.position.x) > 0)
            {
                _mark.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                _mark.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
