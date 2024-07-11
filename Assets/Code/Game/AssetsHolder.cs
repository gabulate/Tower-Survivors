using TowerSurvivors.GUI;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.VFX;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class AssetsHolder : MonoBehaviour
    {
        public static AssetsHolder Instance;
        public DebugInfoText debugInfo;
        public ItemListSO itemListSO;
        public GameHUD HUD;

        [Header("Debugging Objects")]
        public InventoryDebugger inventoryDebugger;

        [Header("VFX Prefabs")]
        public GameObject structureLevelUpVFX;
        public GameObject xpGroupVFX;

        [Header("PickUp Prefabs")]
        public GameObject cookiePrefab;
        [Range(0,1)]
        public float cookieChance = 0.01f;
        public GameObject coinPrefab;
        [Range(0, 1)]
        public float coinChance = 0.02f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
    }
}
