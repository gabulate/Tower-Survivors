using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.Localisation;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.GUI
{
    public class CharacterScreen : MonoBehaviour
    {
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI characterDsc;
        public TextMeshProUGUI itemName;

        public void ShowCharacterDeets(CharacterSO character)
        {
            if (CharacterSelector.IsUnlocked(character))
            {
                characterName.text = Language.Get(character.NameKey);
                characterDsc.text = Language.Get(character.DescriptionKey);
                itemName.text = Language.Get(character.startingItem.itemNameKey);
            }
            else
            {
                characterName.text = "???";
                characterDsc.text = "???";
                itemName.text = "???";
            }
        }

        private void OnEnable()
        {
            CharacterOption.OnSelected.AddListener(ShowCharacterDeets);
        }

        private void OnDisable()
        {
            CharacterOption.OnSelected.RemoveListener(ShowCharacterDeets);
        }
    }
}
