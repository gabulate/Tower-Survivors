using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.PickUps
{
    public class PickUp : MonoBehaviour
    {
        protected static readonly LayerMask _playerLayer = 1 << 3;

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            if (_playerLayer == (_playerLayer | (1 << collider.gameObject.layer)))
                ExecPickUp();
        }

        protected virtual void ExecPickUp()
        {
            Debug.Log("Picked Up!");
        }
    }
}
