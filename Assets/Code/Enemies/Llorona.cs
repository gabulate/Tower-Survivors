using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.Enemies
{
    public class Llorona : Enemy
    {
        private bool _teleporting = false;
        [SerializeField]
        private float _tpDistance = 10f;
        [SerializeField]
        private float _tpDuration = 1f;
        [SerializeField]
        private float _tpCooldown = 10f;
        [SerializeField]
        private float _currentTpCooldown = 10f;
        [SerializeField]
        private float subtractTpC = 0;
        [SerializeField]
        private SoundClip _tpSound;
        [SerializeField]
        private SoundClip _deathSound;

        [SerializeField]
        private SpriteRenderer _shadow;

        protected override void FixedUpdate()
        {
            //currentCooldown is always counting down to 0, when it reaches 0, the enemy is allowed to attack
            currentCooldown = currentCooldown <= 0 ? 0 : currentCooldown - Time.fixedDeltaTime;

            subtractTpC = Vector2.Distance(Player.Instance.transform.position, transform.position)
                * Time.fixedDeltaTime / 10;
            _currentTpCooldown = _currentTpCooldown <= 0 ? 0 : _currentTpCooldown - subtractTpC;

            _timeAlive += Time.fixedDeltaTime;

            if (_currentTpCooldown <= 0)
                Teleport();

            Move();
        }

        protected override void Move()
        {
            if (!isAlive || _teleporting)
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            targetPosition.x = Player.Instance.transform.position.x;
            targetPosition.y = Player.Instance.transform.position.y;

            //Calculate the direction to move towards
            Vector2 moveDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

            //Apply force to the Rigidbody
            _rb.velocity = new Vector2(moveDirection.x * speed * Time.fixedDeltaTime, moveDirection.y * speed * Time.fixedDeltaTime);

            //Flip the sprite based on the movement direction
            if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private void Teleport()
        {
            if (!isAlive)
                return;

            _currentTpCooldown = _tpCooldown;
            Vector2 direction = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
            Vector3 tpPosition = new Vector3(
                transform.position.x + direction.x * _tpDistance,
                transform.position.y + direction.y * _tpDistance,
                0);

            StartCoroutine(TPCoroutine(_tpDuration, tpPosition));
        }

        private IEnumerator TPCoroutine(float duration, Vector3 position)
        {
            _teleporting = true;
            _collider.enabled = false;
            _shadow.enabled = false;
            _animator.SetTrigger("tpInit");

            AudioPlayer.Instance.PlaySFX(_tpSound, transform.position);

            yield return new WaitForSeconds(duration);
            transform.position = position;

            _animator.SetTrigger("tpFinish");
            yield return new WaitForSeconds(0.5f);
            _teleporting = false;
            _collider.enabled = true;
            _shadow.enabled = true;
        }

        protected override void AttackPlayer()
        {
            if (_teleporting)
                return;


            Player.Health.TakeDamage(damage);
            _currentTpCooldown += 1;

            //Resets the cooldown
            currentCooldown = attackCooldown;
        }

        public override void Die(bool addToKillCount)
        {
            base.Die(addToKillCount);
            AudioPlayer.Instance.PlaySFX(_deathSound, transform.position);
        }
    }
}
