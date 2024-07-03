using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using TowerSurvivors.Game;
using TowerSurvivors.GUI;
using TowerSurvivors.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.Enemies
{
    public class Boss : Enemy
    {
        public UnityEvent e_die = new UnityEvent();

        public IEnumerator DieCoroutine()
        {
            CameraFollowScript.Instance.EndGameCamera(transform.position);
            e_die.Invoke();
            yield return new WaitForSeconds(1);
            Destroy(gameObject, 5);
        }

        protected override void DestroyAnim()
        {
            _collider.enabled = false;
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            StartCoroutine(DieCoroutine());
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            //The boss constantly gets more speed but it is reduced when taking damage
            speed += 4 * Time.fixedDeltaTime;
        }

        protected override void Move()
        {
            if (!isAlive || isStunned)
            {
                _rb.velocity = Vector2.zero;
                return;
            }


            targetPosition.x = Player.Instance.transform.position.x;
            targetPosition.y = Player.Instance.transform.position.y;

            //Calculate the direction to move towards
            Vector2 moveDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

            if(Vector2.Distance(transform.position, targetPosition) > 3)
            {
                //if the distance from the player is big, move twice as fast
                _rb.velocity = new Vector2(moveDirection.x * speed * 2 * Time.fixedDeltaTime, moveDirection.y * speed * 2 * Time.fixedDeltaTime);
            }
            else
            {
                //Apply force to the Rigidbody
                _rb.velocity = new Vector2(moveDirection.x * speed * 2 * Time.fixedDeltaTime, moveDirection.y * speed * Time.fixedDeltaTime);
            }

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

        public override void TakeDamage(float damage, bool countIfKilled = true)
        {
            //countIfKilled is only false when called by the KillAllEnemies() method by the enemy spawner
            //This check is here so that coroutine doesn't kill this boss
            if (isInvincible || !countIfKilled) 
            {
                return;
            }

            HP -= damage;
            
            if(speed > 40)
            {
                speed -= damage * 0.15f;
            }

            try
            {
                AudioPlayer.Instance.PlaySFX(hurtSound, transform.position);
                DamageNumberController.Instance.ShowDamageNumber((int)damage, transform.position);
            }
            catch { }
            //Debug.Log("Damage taken: " + damage);

            StartCoroutine(Invincivility(invulnerableTime));

            if (HP <= 0)
            {
                Die(countIfKilled);
            }
            else
                StartCoroutine(Stun(stunTime));
        }
    }
}
