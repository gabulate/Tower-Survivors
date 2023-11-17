using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    /// <summary>
    /// MonoBehaviour that automatically destroys the object after a certain amount of seconds to improve performance.
    /// </summary>
    public class AutoDestroy : MonoBehaviour
    {
        public float Seconds = 10f;
        public float CurrentSeconds;
        private Animator _animator;

        void Start()
        {
            CurrentSeconds = Seconds;
            TryGetComponent(out _animator);
        }


        void Update()
        {
            if(CurrentSeconds <= 0)
            {
                StartCoroutine(DestroyAnim());
            }
            else
            {
                CurrentSeconds -= Time.deltaTime;
            }
        }

        IEnumerator DestroyAnim()
        {
            if(_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}
