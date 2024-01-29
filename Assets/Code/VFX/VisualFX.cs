using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.VFX
{
    public class VisualFX : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public void PlayEffect()
        {
            _animator.SetTrigger("play");
            Destroy(gameObject, 2);
        }
    }
}
