using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerSurvivors.GUI
{
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private TextMeshPro _text;

        public void Display(int number)
        {
            _text.text = number.ToString();
            _animator.ResetTrigger("display");
            _animator.SetTrigger("display");
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
