using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Game;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.Projectiles;
using TowerSurvivors.VFX;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class Pawn : Structure
    {
        [SerializeField]
        private GameObject _queenPrefab;
        [SerializeField]
        private Vector2 _direction = Vector2.right;
        [SerializeField]
        private Transform _arrow;
        [SerializeField]
        private SpriteRenderer _mouseUI;

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
            stats.currentCooldown = stats.attackCooldown + stats.projectileAmnt* stats.timeBetweenMultipleShots;
        }

        protected override IEnumerator SpawnProjectile(float delay)
        {
            yield return new WaitForSeconds(delay);
            //Instantiates the projectile////////////////////////////////////////////////
            GameObject e = Instantiate(prefab, _firePoint.position, _firePoint.rotation);
            //Calculates the direction of the target
            //Sets the corresponding attributes to the projectile
            e.GetComponent<BasicProjectile>().SetAttributes(stats, _direction);

            //Play firing animation and firing sound
            _animator.SetTrigger("fire");
            AudioPlayer.Instance.PlaySFX(firingSound, transform.position);
        }

        public override bool Upgrade(Structure selectedStructure)
        {
            bool ret = base.Upgrade(selectedStructure);

            //If it reached the maximum level, transform into a queen
            if (isMaxed)
            {
                Structure queen = Instantiate(_queenPrefab, transform.position, Quaternion.identity).GetComponent<Structure>();

                StructureManager.Instance.ReplaceStructure(this, queen);

                if (!SaveSystem.csd.unQueen)
                {
                    SaveSystem.csd.unQueen = true;
                    SaveSystem.Save();
                }
                return true;
            }
           return ret;
        }

        public override void UpdateOrientation()
        {
            switch (_orientation)
            {
                case Orientation.UP:
                    _direction = Vector2.up;
                    _arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    break;
                case Orientation.LEFT:
                    _direction = Vector2.left;
                    _arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    break;
                case Orientation.DOWN:
                    _direction = Vector2.down;
                    _arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                    break;
                case Orientation.RIGHT:
                    _direction = Vector2.right;
                    _arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
            }
        }

        //Makes sure the range is always 1
        public override void ApplyBuffs(PlayerStats playerStats)
        {
            base.ApplyBuffs(playerStats);
            stats.range = 1;
        }

        //Modified method for toggling UI elements
        public override void EnableStructure(bool enabled)
        {
            base.EnableStructure(enabled);
            _mouseUI.enabled = !enabled;
        }
    }
}
