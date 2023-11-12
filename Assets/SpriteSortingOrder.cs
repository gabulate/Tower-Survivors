using UnityEngine;

public class SpriteSortingOrder : MonoBehaviour
{
    private void Update()
    {
        // Set the Z coordinate based on the Y coordinate
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
