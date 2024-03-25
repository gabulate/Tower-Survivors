    using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Enemies;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class Dart : BasicProjectile
    {
        protected override void DestroyAnim()
        {
            _enough = true;
            _collider.enabled = false;
            speed = 0;
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            Destroy(gameObject, 1);
        }
    }
}
