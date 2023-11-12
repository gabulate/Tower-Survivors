using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
