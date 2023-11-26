using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors
{
    /// <summary>
    /// Basic structure script that contains basic stats and methods.
    /// Can be extended.
    /// </summary>
    public class Structure : MonoBehaviour
    {
        protected static readonly LayerMask _enemyLayer = 1 << 6;

        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected Transform _firePoint;
        [SerializeField]
        protected Transform _cannon;

        public GameObject prefab;
        public Transform targetObject;
        public float range;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;
        public float areaSize = 1;
        public float projectileSpeed = 2;
        public float duration = 5f;
        public int projectileAmnt = 1;
        public int passThroughAmnt = 0;

        // Update is called once per frame
        void FixedUpdate()
        {
            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            currentCooldown = currentCooldown <= 0 ? 0 : currentCooldown - Time.fixedDeltaTime;
            Attack();                      
        }

        protected void Attack()
        {
            //GET THE CLOSEST TARGET WITHIN RANGE/////////////////////////
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, _enemyLayer);

            //If didn't find any targets, return
            if (hits.Length == 0)
                return;

            targetObject = null;
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

            //Rotate the cannon to face the target/////////////////////////////////
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

            //Only instantiates the projectile when the cooldown is 0;
            if (currentCooldown > 0)
                return;

            //Instantiates the projectile////////////////////////////////////////////////
            GameObject e = Instantiate(prefab, _firePoint.position, _firePoint.rotation);
            //Calculates the direction of the target
            Vector3 direction = (targetObject.position - e.transform.position).normalized;
            //Sets the corresponding attributes to the projectile
            e.GetComponent<BasicProjectile>().SetAttributes(damage, passThroughAmnt, projectileSpeed, direction, duration);
            e.transform.localScale = new Vector3(areaSize, areaSize, 1);

            //Reset cooldown
            currentCooldown = attackCooldown;       
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
