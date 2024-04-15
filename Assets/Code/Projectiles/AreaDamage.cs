using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class AreaDamage : MonoBehaviour
    {
        protected static readonly LayerMask _enemyLayer = 1 << 6;

        [SerializeField]
        private Animator _animator;

        private float _timeLeft = 1;
        private float _areaSize = 1;
        private float _damage = 10;
        private float _cooldown = 1;
        private float _currentCooldown = 1;

        private bool _active = false;

        public void SetAttributes(float duration, float areaSize, float damage, float cooldown)
        {
            _timeLeft = duration;

            _areaSize = areaSize;
            transform.localScale = areaSize * Vector3.one;

            _damage = damage;
            _cooldown = cooldown;
            _currentCooldown = cooldown;
            _active = true;

        }

        void FixedUpdate()
        {
            if (!_active)
                return;

            _currentCooldown -= Time.fixedDeltaTime;

            if(_currentCooldown <= 0)
            {
                DealDamage();
                _currentCooldown = _cooldown;
            }

            if (_timeLeft <= 0)
            {
                _active = false;
                DestroyAnim();
            }
            else
            {
                _timeLeft -= Time.fixedDeltaTime;
            }
        }

        private void DestroyAnim()
        {
            _animator.SetTrigger("destroy");
            Destroy(gameObject, 0.7f);
        }

        private void DealDamage()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _areaSize / 2, _enemyLayer);

            foreach (Collider2D col in hits)
            {
                col.GetComponent<Enemy>().TakeDamage(_damage);
            }
        }
    }
}
