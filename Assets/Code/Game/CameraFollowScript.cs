using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class CameraFollowScript : MonoBehaviour
    {
        void LateUpdate()
        {
            //Follows the player
            //Super high level complicated stuff really
            transform.position = new Vector3(
                Player.Instance.transform.position.x,
                Player.Instance.transform.position.y,
                Player.Instance.transform.position.z - 10);
        }
    }
}
