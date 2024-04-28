using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
    public class CharacterSO : ScriptableObject
    {
        public Sprite Icon;
        public string idName = "name me";
        public string NameKey = "";
        public string DescriptionKey = "";
        public StructureItemSO startingItem;
        public GameObject prefab;
    }
}
