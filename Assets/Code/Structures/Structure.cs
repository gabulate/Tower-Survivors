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
        protected Animator _animator;

        public GameObject prefab;
        public Transform firePoint;
        public float range;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;
        public float areaSize = 1;
        public float projectileSpeed = 2;
        public float duration = 5f;
        public int projectileAmnt = 1;
        public int passThroughAmnt = 0;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //If current cooldown is bigger than 0, it will subtract the amount of time passed since last FixedUpdate
            currentCooldown = currentCooldown <= 0 ? 0 : currentCooldown - Time.fixedDeltaTime;
            if(currentCooldown <= 0)
            {
                Attack();
                currentCooldown = attackCooldown;
            }
        }

        protected void Attack()
        {
            //GET THE CLOSEST TARGET WITHIN RANGE
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, _enemyLayer);
            Transform targetObject = null;
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

            if (targetObject != null)
            {
                //TODO: TAKE INTO ACCOUNT THE ROTATION OF THE TOWER AND APPLY IT TO THE PROJECTILE
                //TODO: FLIP THE SPRITE ACCORDINGLY
                GameObject e = Instantiate(prefab, firePoint.position, firePoint.rotation);

                //Calculates the direction of the target
                Vector3 direction = (targetObject.position - e.transform.position).normalized;
                //Sets the corresponding attributes to the projectile
                e.GetComponent<BasicProjectile>().SetAttributes(damage, passThroughAmnt, projectileSpeed, direction, duration);
                e.transform.localScale = new Vector3(areaSize, areaSize, 1);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
