using System.Collections;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class CameraFollowScript : MonoBehaviour
    {
        public static CameraFollowScript Instance;
        private bool _followPlayer = true;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        void LateUpdate()
        {
            if (!_followPlayer)
                return;

            //Follows the player
            //Super high level complicated stuff really
            transform.position = new Vector3(
                Player.Instance.transform.position.x,
                Player.Instance.transform.position.y,
                Player.Instance.transform.position.y - 10);
        }

        public void EndGameCamera(Vector3 target)
        {
            target = new Vector3(target.x, target.y, target.y -10);
            _followPlayer = false;
            StartCoroutine(EndGameCoroutine(target));
        }

        public IEnumerator EndGameCoroutine(Vector3 target)
        {
            StartCoroutine(Zoom());
            Vector3 startPosition = transform.position;
            float duration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            // Ensure the object reaches the exact target position
            transform.position = target;
        }

        private IEnumerator Zoom()
        {
            Camera c = GetComponent<Camera>();
            yield return new WaitForSeconds(0.3f);

            float startSize = c.orthographicSize;
            float targetSize = 2;
            float duration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                c.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; // Wait for the next frame
            }
        }
    }
}
