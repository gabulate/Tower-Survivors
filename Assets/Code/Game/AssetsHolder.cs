using TowerSurvivors.GUI;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.VFX;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class AssetsHolder : MonoBehaviour
    {
        public static AssetsHolder Instance;
        public ItemListSO itemList;
        public GameHUD HUD;

        [Header("VFX Prefabs")]
        public GameObject structureLevelUpVFX;

        [Header("PickUp Prefabs")]
        public GameObject cookiePrefab;
        [Range(0,1)]
        public float cookieChance = 0.01f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
    }
}
