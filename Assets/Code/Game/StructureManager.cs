using TowerSurvivors.Audio;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.Game
{
    public class StructureManager : MonoBehaviour
    {
        public static StructureManager Instance;
        public SoundClip placeSound;

        public int initialMaximumStructures = 5;
        public int MaximumStructures = 5;
        public Transform placedStructres;
        public Transform hiddenStructures;
        public Structure[] structures;

        public UnityEvent<int, int> e_StAmntChanged;

        public void PlaceStructure(Structure structure)
        {
            Structure[] structures = GetStructures();

            if (structures.Length >= MaximumStructures)
                return;

            structure.transform.parent = placedStructres;
            structure.transform.position = new Vector3(structure.transform.position.x, structure.transform.position.y, structure.transform.position.y);
            structure.EnableStructure(true);
            structure.OutLine(false);

            AudioPlayer.Instance.PlaySFX(placeSound, structure.transform.position);

            e_StAmntChanged.Invoke(structures.Length + 1, MaximumStructures);
        }

        public Structure[] GetStructures()
        {
            if (placedStructres.childCount != structures.Length)
            {
                structures = placedStructres.GetComponentsInChildren<Structure>();
                return structures;
            }
            return structures;
        }

        public GameObject AddToInventory(StructureItemSO item, int index)
        {
            //Instantiates the structure into the scene
            GameObject go = Instantiate(item.prefab, hiddenStructures);
            Structure structure = go.GetComponent<Structure>();
            structure.stats.range += Player.Instance.stats.rangeIncrease;

            structure.EnableStructure(false);
            go.SetActive(false);

            return go;
        }

        public void PickUpStructure(Structure structure)
        {
            structure.transform.parent = hiddenStructures;
            structure.EnableStructure(false);
            structure.gameObject.SetActive(false);

            int structureQty = GetStructures().Length;
            e_StAmntChanged.Invoke(structureQty, MaximumStructures);
        }

        public bool CanPlace()
        {
            return GetStructures().Length < MaximumStructures;
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        public void IncreaseStructureLimit(int extraStructures)
        {
            MaximumStructures = initialMaximumStructures + extraStructures;
            e_StAmntChanged.Invoke(structures.Length, MaximumStructures);
        }
    }
}
