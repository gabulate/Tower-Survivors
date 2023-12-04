using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerSurvivors.ScriptableObjects;

namespace TowerSurvivors.Game
{
    public class AssetsHolder : MonoBehaviour
    {
        public static AssetsHolder Instance;
        public ItemListSO itemList;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
        }
    }
}
