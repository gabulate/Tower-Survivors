using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class BarbedWire : Structure
    {
        [SerializeField]
        private List<Enemy> _enemiesInRange = new List<Enemy>();
        [SerializeField]
        private GameObject _wire;
        

        protected override void FixedUpdate()
        {
            if(stats.currentCooldown > 0)
            {
                stats.currentCooldown -= Time.fixedDeltaTime;
            }

            if(stats.currentCooldown <= 0)
            {
                Attack();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_enemyLayer == (_enemyLayer | (1 << collision.gameObject.layer)))
            {
                Enemy e = collision.GetComponent<Enemy>();
                _enemiesInRange.Add(e);
                e.speed -= 20;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_enemyLayer == (_enemyLayer | (1 << collision.gameObject.layer)))
            {
                Enemy e = collision.GetComponent<Enemy>();
                _enemiesInRange.Remove(e);
                e.speed += 20;
            }
        }


        protected override void Attack()
        {
            if (_enemiesInRange.Count <= 0)
                return;

            //Damages instantly all enemies in range
            for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
            {
                if (!_enemiesInRange[i] || !_enemiesInRange[i].isAlive)
                {
                    _enemiesInRange.RemoveAt(i);
                    continue;
                }
                _enemiesInRange[i].TakeDamage(stats.damage);
            }

            //Reset cooldown
            stats.currentCooldown = stats.attackCooldown;
        }

        protected override IEnumerator SpawnProjectile(float delay)
        {
            yield return new WaitForSeconds(delay);
        }

        public override void ApplyBuffs(PlayerStats playerStats)
        {
            base.ApplyBuffs(playerStats);

            transform.localScale = new(stats.areaSize, stats.areaSize, stats.areaSize);
            _margin = 6-level *0.5f;
            _outline.transform.localScale = Vector3.one * _margin;
        }


        public override void UpdateOrientation()
        {
            switch (_orientation)
            {
                case Orientation.UP:
                case Orientation.DOWN:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    break;
                case Orientation.LEFT:
                case Orientation.RIGHT:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    break;
            }
        }
    }
}
