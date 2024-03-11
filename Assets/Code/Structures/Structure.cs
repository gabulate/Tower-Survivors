using System.Collections;
using TowerSurvivors.Audio;
using TowerSurvivors.Enemies;
using TowerSurvivors.Game;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.VFX;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    /// <summary>
    /// Basic structure script that contains basic stats and methods.
    /// Can be extended.
    /// </summary>
    public abstract class Structure : MonoBehaviour
    {
        protected static readonly LayerMask _enemyLayer = 1 << 6;
        protected static readonly LayerMask _structureLayer = 1 << 7;
        protected static readonly LayerMask _boundsLayer = 1 << 10;

        [Header("Must Have References")]
        public StructureItemSO item;
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
        [SerializeField]
        protected GameObject prefab;
        [SerializeField]
        protected SoundClip firingSound;
        [SerializeField]
        protected Collider2D _collider;

        [Header("Meta Atributtes")]
        public bool canAttack = false;
        public Transform targetObject;
        public Enemy targetEnemy;
        protected Vector3 _targetPos;
        [SerializeField]
        protected float _margin = 1;
        public bool uniqueOrientation = true;
        [SerializeField]
        protected Orientation _orientation;
        protected static Color _placeableColor = new(0f, 0.255f, 0.690f, 0.4f); //Blueish
        protected static Color _notPlaceableColor = new(1f, 0.1f, 0f, 0.4f);//Redish
        protected static Color _normalRangeColor = new Color(1f, 0.92f, 0.016f, 0.4f); // Yellowish
        protected static Color _increasedRangeColor = new Color(0.125f, 0.8f, 0.004f, 0.4f); // Greenish

        [Header("Structure Stats")]
        public int level = 1;
        public bool isMaxed = false;
        public StructureStats stats;


        private void OnEnable()
        {
            //Override this function for strucutres with multiple orientations.
            transform.rotation = Quaternion.Euler(Vector3.zero);
            _rangeOutline.color = _normalRangeColor;
        }

        /// <summary>
        /// Takes in the player stats and Updates the structure's stats, depending on its level and player buffs.
        /// </summary>
        public virtual void ApplyBuffs(PlayerStats playerStats)
        {
            //Calculates the percentaje from the current level and adds it
            stats.range = item.levels[level - 1].range;
            stats.range += item.levels[level - 1].range * playerStats.rangeIncrease;

            //Calculates the percentaje
            stats.damage = item.levels[level - 1].damage;
            stats.damage += item.levels[level - 1].damage * playerStats.damageIncrease;

            //Calculates the percentaje from the current level and subtracts it
            stats.attackCooldown = item.levels[level - 1].attackCooldown;
            stats.attackCooldown -= item.levels[level - 1].attackCooldown * playerStats.coolDownReduction;

            //Calculates the percentaje from the current level and adds it
            stats.areaSize = item.levels[level - 1].areaSize + playerStats.areaSizeIncrease;
            stats.areaSize += item.levels[level - 1].areaSize * playerStats.areaSizeIncrease;

            //Calculates the percentaje from the current level and adds it
            stats.projectileSpeed = item.levels[level - 1].projectileSpeed;
            stats.projectileSpeed += item.levels[level - 1].projectileSpeed * playerStats.projectileSpeedBoost;

            stats.duration = item.levels[level - 1].duration + playerStats.durationIncrease;
            stats.projectileAmnt = item.levels[level - 1].projectileAmnt + playerStats.ProjectileAmntIncrease;
            stats.passThroughAmnt = item.levels[level - 1].passThroughAmnt;
            stats.timeBetweenMultipleShots = item.levels[level - 1].timeBetweenMultipleShots;
            _rangeOutline.transform.localScale = Vector3.one * stats.range;
            _rangeOutline.color = _normalRangeColor;
        }

        public virtual void EnableStructure(bool enabled)
        {
            canAttack = enabled;
            _collider.enabled = enabled;
            _shadow.enabled = enabled;
            
            if (!enabled)
            {
                _outline.transform.localScale = Vector3.one * _margin;
                _rangeOutline.transform.localScale = Vector3.one * stats.range;
            }
        }

        public bool CheckIfPlaceable()
        {
            if (!StructureManager.Instance.CanPlace())
            {
                _outline.color = _notPlaceableColor;
                return false;
            }

            Collider2D[] hits = Physics2D.OverlapBoxAll(_outline.transform.position, Vector2.one * _margin, 0, _structureLayer | _boundsLayer);

            bool placeable = hits.Length <= 0;

            _outline.color = placeable ? _placeableColor : _notPlaceableColor;

            return placeable;
        }

        public virtual void ShowLevelUpStats(Structure selectedStructure)
        {
            if (isMaxed || selectedStructure && selectedStructure.isMaxed)
            {
                AssetsHolder.Instance.HUD.HoverStructureMax(this);
                return;
            }

            if (!selectedStructure)
            {
                AssetsHolder.Instance.HUD.HoverStructure(this, false);
                _rangeOutline.transform.localScale = Vector3.one * stats.range;
                _rangeOutline.color = _normalRangeColor;
                return;
            }

            if (selectedStructure.GetType() == GetType())
            {
                //if (selectedStructure.level == item.levels[level].neededLevel)
                //{

                int highestLevel = 0;
                //chooses the the structure with the highest level
                if (this.level >= selectedStructure.level)
                {
                    AssetsHolder.Instance.HUD.HoverStructure(this, true);
                    highestLevel = this.level;
                }
                else
                {
                    AssetsHolder.Instance.HUD.HoverStructure(selectedStructure, true);
                    highestLevel = selectedStructure.level;
                }

                //Shows the range that the structure will have next level, and changes its color if the next level has a range increase
                if (item.levels[highestLevel].range > item.levels[level - 1].range)
                    {
                        //Gets the range the structure would be the next level, taking into consideration current buffs
                        float nextRange = item.levels[highestLevel].range + Player.Instance.stats.rangeIncrease;
                        nextRange += item.levels[highestLevel].range * Player.Instance.stats.rangeIncrease;

                    _rangeOutline.transform.localScale = Vector3.one * nextRange;

                        _rangeOutline.color = _increasedRangeColor;
                }
                else
                {
                    _rangeOutline.transform.localScale = Vector3.one * stats.range;
                    _rangeOutline.color = _normalRangeColor;
                }

                return;
                //}
            }
        }

        /// <summary>
        /// Tries to upgrade with the selected structure.
        /// </summary>
        /// <param name="selectedStructure">Structure that will be used to upgrade.</param>
        /// <returns>True if the structure met the requirements, false if not.</returns>
        public virtual bool Upgrade(Structure selectedStructure)
        {
            if (isMaxed)
                return false;

            if (selectedStructure.isMaxed)
                return false;

            if (selectedStructure.GetType() == GetType())
            {
                //Used to check for a specific level, now the same structure at any level can be used
                //if (selectedStructure.level == item.levels[level - 1].neededLevel)
                //{
                int highestLevel;
                //chooses the the structure with the highest level
                if (this.level >= selectedStructure.level)
                {
                    highestLevel = this.level;
                }
                else
                {
                    highestLevel = selectedStructure.level;
                }

                level = highestLevel + 1;

                GameObject vfx = AssetsHolder.Instance.structureLevelUpVFX;
                vfx = Instantiate(vfx, new(transform.position.x, transform.position.y - 0.5f, transform.position.y -1), Quaternion.identity);
                vfx.transform.localScale = Vector3.one * _margin / 2;
                vfx.GetComponent<VisualFX>().PlayEffect();

                Debug.Log("UPGRADED to level: " + level);

                    if (level == item.levels.Count)
                        isMaxed = true;

                AudioPlayer.Instance.PlaySFX(StructureManager.Instance.structureUpgradeSound);
                    GameManager.Instance.structuresUpgraded++;
                    return true;
                //}
            }
            
            Debug.Log("Already at max level: " + level + "!");
            return false;
        }

        public void OutLine(bool outline)
        {
            _outline.enabled = outline;
            _rangeOutline.enabled = outline;
        }

        #region Orientation
        public virtual void ChangeOrientation(Orientation orientation)
        {
            _orientation = orientation;
            switch (orientation)
            {
                case Orientation.UP:
                    break;
                case Orientation.DOWN:
                    break;
                case Orientation.LEFT:
                    break;
                case Orientation.RIGHT:
                    break;
            }
            UpdateOrientation();
        }

        public virtual void ChangeOrientation()
        {
            switch (_orientation)
            {
                case Orientation.UP:
                    _orientation = Orientation.RIGHT;
                    break;
                case Orientation.RIGHT:
                    _orientation = Orientation.DOWN;
                    break;
                case Orientation.DOWN:
                    _orientation = Orientation.LEFT;
                    break;
                case Orientation.LEFT:
                    _orientation = Orientation.UP;
                    break;
            }

            UpdateOrientation();
        }

        public virtual void UpdateOrientation()
        {
            Debug.LogWarning("Orientation: " + _orientation + ". Changing orientation not implemented yet.");
        }

        #endregion

        #region Overridables
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

        #endregion

        //Draws a circle of the structure's attack range
        protected virtual void OnDrawGizmosSelected()
        {
            try
            {
                _outline.transform.localScale = Vector3.one * _margin;
                _rangeOutline.transform.localScale = Vector3.one * stats.range;
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, stats.range);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(_outline.transform.position, new Vector3(_margin, _margin, _margin));
            }
            catch
            {

            }
        }
    }

    [System.Serializable]
    public class StructureStats
    {
        public float range = 4;
        public float damage = 10f;
        public float attackCooldown = 1f;
        public float currentCooldown = 0f;
        public float areaSize = 1;
        public float projectileSpeed = 5;
        public float duration = 3f;
        public int projectileAmnt = 1;
        public float timeBetweenMultipleShots = 0.5f; //In case projectileAmnt is bigger than 1;
        public int passThroughAmnt = 0;
    }

    public enum Orientation
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
