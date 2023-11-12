using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.PlayerScripts
{
    public class MovementController : MonoBehaviour
    {
        public float Speed = 10;
        public GameObject charachter;

        [SerializeField]
        private Vector2 _input;

        void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void FixedUpdate()
        {
            Vector3 targetPosition = transform.position += new Vector3(_input.x * (Speed * Time.fixedDeltaTime),
                _input.y * (Speed * Time.fixedDeltaTime), 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, .5f);

            if (_input.x > 0)
            {
                charachter.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_input.x < 0)
            {
                charachter.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
