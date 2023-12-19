using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.GUI;

namespace TowerSurvivors.Game
{
    public class AssetsHolder : MonoBehaviour
    {
        public static AssetsHolder Instance;
        public ItemListSO itemList;
        public GameHUD HUD;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
        }
    }
}
