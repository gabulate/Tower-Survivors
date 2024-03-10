using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.VFX
{
    public class VisualFX : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private SpriteRenderer _sprite;
        public void PlayEffect()
        {
            _animator.SetTrigger("play");
            Destroy(gameObject, 2);
        }

        public void ChangeColor(Color color)
        {
            _sprite.color = color;
        }
    }
}
