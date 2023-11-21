using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that controls the player's input and Movement.
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        public float Speed = 10;

        [SerializeField]
        private bool _canMove = true;
        [SerializeField]
        private Vector2 _input;

        void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void FixedUpdate()
        {
            Move();   
        }

        private void Move()
        {
            if (!_canMove)
            {
                Player.PlayerAnimator.SetFloat("speed", 0);
                return;
            }

            //Normalize the input vector to ensure consistent speed in all directions.
            Vector2 normalizedInput = _input.normalized;

            //Calculate the target position based on the normalized input.
            Vector3 targetPosition = transform.position + new Vector3(normalizedInput.x * (Speed * Time.fixedDeltaTime),
                normalizedInput.y * (Speed * Time.fixedDeltaTime), 0);

            //Move towards the target position using Lerp.
            transform.position = targetPosition;

            //Animator and sprite orientation////////////////////////////////////
            Player.PlayerAnimator.SetFloat("speed", Mathf.Abs(_input.magnitude));
            if (_input.x > 0)
            {
                Player.Sprite.flipX = false;
            }
            else if (_input.x < 0)
            {
                Player.Sprite.flipX = true;
            }
        }

        public void EnableMovement(bool enabled)
        {
            _canMove = enabled;
        }
    }
}
