using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that gives static acces player scripts, stats and components.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public static PlayerHealth Health { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
            Health = GetComponent<PlayerHealth>();
        }

    }
}
