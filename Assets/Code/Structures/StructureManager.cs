using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.Structures
{
    public class StructureManager : MonoBehaviour
    {
        public static StructureManager Instance;

        public void PlaceStructure(Structure structure)
        {
            structure.transform.parent = transform;
            structure.canAttack = true;
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
