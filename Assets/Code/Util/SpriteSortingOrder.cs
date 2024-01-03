using UnityEngine;

namespace TowerSurvivors.Util
{
    /// <summary>
    /// Monobehaviour that makes objects further up in the world look like they're behind.
    /// </summary>
    public class SpriteSortingOrder : MonoBehaviour
    {
        private void Update()
        {
            // Set the Z coordinate based on the Y coordinate
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }
    }
}
