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
        protected static readonly LayerMask _structureLayer = 1 << 7;

        [Header("Must Have References")]
        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected Transform _firePoint;
        [SerializeField]
        protected SpriteRenderer _shadow;
        [SerializeField]
        protected SpriteRenderer _outline;
        [SerializeField]
        protected SpriteRenderer _rangeOutline;
        public GameObject prefab;

        [Header("Meta Atributtes")]
        public bool canAttack = false;
        public Transform targetObject;
        protected Vector3 _targetPos;
        [SerializeField]
        protected float _margin = 1;
        public bool uniqueOrientation = true;
        [SerializeField]
        protected Orientation _orientation;
        protected static Color _placeableColor = new(0f, 0.255f, 0.690f, 0.4f);
        protected static Color _notPlaceableColor = new(1f, 0.1f, 0f, 0.4f);

        [Header("Structure Stats")]
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


        private void OnEnable()
        {
            //Override this function for strucutres with multiple orientations.
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        protected virtual void FixedUpdate()
        {
            //Method to override
        }

        protected virtual void Attack()
        {
            //TODO: Add a stat counter for everytime this structure attacks
            //Override for the attack
        }

        public void EnableStructure(bool enabled)
        {
            canAttack = enabled;
            GetComponent<Collider2D>().enabled = enabled;
            _outline.enabled = !enabled;
            _rangeOutline.enabled = !enabled;
            _shadow.enabled = enabled;
            if (!enabled)
            {
                _outline.transform.localScale = Vector3.one * _margin;
                _rangeOutline.transform.localScale = Vector3.one * range;
            }
        }

        public bool CheckIfPlaceable()
        {
            _outline.enabled = true;

            Collider2D[] hits = Physics2D.OverlapBoxAll(_outline.transform.position, Vector2.one * _margin, 0, _structureLayer);

            bool placeable = hits.Length <= 0;

            _outline.color = placeable ? _placeableColor : _notPlaceableColor;

            return placeable;
        }

        public virtual void ChangeOrientation(Orientation orientation)
        {
            _orientation = orientation;
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
            Gizmos.DrawWireCube(_outline.transform.position, new Vector3(_margin, _margin, _margin));
        }
    }

    public enum Orientation
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
