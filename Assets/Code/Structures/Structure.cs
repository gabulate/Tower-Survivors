using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Projectiles;
using UnityEngine;

namespace TowerSurvivors.Structures
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
        

        public GameObject prefab;
        public Transform targetObject;
        protected Vector3 _targetPos;
        public float range;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;
        public float areaSize = 1;
        public float projectileSpeed = 2;
        public float duration = 5f;
        public int projectileAmnt = 1;
        public float timeBetweenMultipleShots = 0.5f; //In case projectileAmnt is bigger than 1;
        public int passThroughAmnt = 0;

        protected virtual void FixedUpdate()
        {
            //Method to override
        }

        protected virtual void Attack()
        {
             //TODO: Add a stat counter for everytime this structure attacks
             //Override for the attack
        }

        protected virtual IEnumerator SpawnProjectile(float delay)
        {
            yield return new WaitForSeconds(delay);
            //Meant to be overridden
        }

        //Draws a circle of the structure's attack range
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
