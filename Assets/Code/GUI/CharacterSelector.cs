using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.GUI
{
    public class CharacterSelector : MonoBehaviour
    {
        public static CharacterSO selected;

        public static void SelectCharacter(CharacterSO character)
        {
            selected = character;
        }

        public static bool IsUnlocked(CharacterSO character)
        {
            return character.idName.ToLower() switch
            {
                "engineer" => SaveSystem.csd.unEngineer,
                "prisoner" => SaveSystem.csd.unPrisoner,
                _ => false,
            };
        }
    }
}
