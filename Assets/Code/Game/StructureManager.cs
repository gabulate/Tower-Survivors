using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Structures;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class StructureManager : MonoBehaviour
    {
        public static StructureManager Instance;

        public void PlaceStructure(Structure structure)
        {
            structure.transform.parent = transform;
            structure.transform.position = new Vector3(structure.transform.position.x, structure.transform.position.y, structure.transform.position.y);
            structure.EnableStructure(true);
        }

        public Structure[] GetStructures()
        {
            return GetComponentsInChildren<Structure>();
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
