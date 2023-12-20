using System.Collections;
using System.Collections.Generic;
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

        public int MaximumStructures = 3;
        public Transform placedStructres;
        public Transform hiddenStructures;

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

            e_StAmntChanged.Invoke(structures.Length +1, MaximumStructures);
        }

        public Structure[] GetStructures()
        {
            if(placedStructres.childCount > 0)
            {
                return placedStructres.GetComponentsInChildren<Structure>();
            }
            return new Structure[0];
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
    }
}
