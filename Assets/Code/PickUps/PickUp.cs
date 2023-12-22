using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Audio;
using UnityEngine;

namespace TowerSurvivors.PickUps
{
    public class PickUp : MonoBehaviour
    {
        protected static readonly LayerMask _playerLayer = 1 << 3;
        [SerializeField]
        protected SoundClip pickUpSound;

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            if (_playerLayer == (_playerLayer | (1 << collider.gameObject.layer)))
            {
                ExecPickUp();
               AudioPlayer.Instance.PlaySFX(pickUpSound);
            }
        }

        protected virtual void ExecPickUp()
        {
            Debug.Log("Picked Up!");
        }
    }
}
