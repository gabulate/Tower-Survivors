using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class StructureManager : MonoBehaviour
    {
        public static StructureManager Instance;

        public Transform placedStrucutres;
        public Transform hiddenStructures;

        public void PlaceStructure(Structure structure)
        {
            structure.transform.parent = placedStrucutres;
            structure.transform.position = new Vector3(structure.transform.position.x, structure.transform.position.y, structure.transform.position.y);
            structure.EnableStructure(true);
            structure.OutLine(false);
        }

        public Structure[] GetStructures()
        {
            if(placedStrucutres.childCount > 0)
            {
                return GetComponentsInChildren<Structure>();
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
