using System.Collections;
using TowerSurvivors.Enemies;
using TowerSurvivors.Structures;
using UnityEngine;

namespace TowerSurvivors.Projectiles
{
    public class SniperBullet : BasicProjectile
    {
        private float _range;
        public Vector2 startPoint;
        private Vector2 endPoint = new Vector2(5f, 5f);
        public void SetAttributes(StructureStats stats, Vector3 direction)
        {
            base.SetAttributes(stats, direction);
            startPoint = transform.position;
            endPoint = direction;
            _range = stats.range;
            StartCoroutine(Snipe());
        }

        //shows the bullet's trail.
        public IEnumerator Snipe()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, endPoint - startPoint, Vector2.Distance(startPoint, endPoint), _enemyLayer);

            float i = 0;
            while (i < 1)
            {
                yield return new WaitForSeconds(0.01f);
                transform.position = Vector3.Lerp(startPoint, endPoint, i);
                i += 0.15f;
            }
            transform.position = endPoint;


            //Deals damage to all enemies hit, no matter its passthrough
            foreach (RaycastHit2D hit in hits)
            {
                if (_enemyLayer == (_enemyLayer | (1 << hit.collider.gameObject.layer)))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
            }


            DestroyAnim();
        }

        private void OnDrawGizmos()
        {
            // Set the color for the gizmo
            Gizmos.color = Color.red;

            // Draw the ray in the Scene view
            Gizmos.DrawLine(startPoint, endPoint);
        }

        protected override void FixedUpdate()
        {
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
        }

        protected override void DestroyAnim()
        {
            _enough = true;
            _collider.enabled = false;
            if (_animator != null)
            {
                _animator.SetTrigger("destroy");
            }
            Destroy(gameObject, 5);
        }
    }
}
