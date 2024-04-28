using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class CharacterOption : MonoBehaviour
    {
        public CharacterSO character;
        public Image backgorund; 
        public Image sprite;
        public Image structureSprite;

        public static UnityEvent Unselect = new UnityEvent();
        public static UnityEvent<CharacterSO> OnSelected = new UnityEvent<CharacterSO>();
        public void SelectThisCharacter()
        {
            Unselect.Invoke();
            CharacterSelector.SelectCharacter(character);
            backgorund.color = new Color(0.255f, 0.153f, 0.243f);
            OnSelected.Invoke(character);
        }

        private void Start()
        {
            if(CharacterSelector.selected && CharacterSelector.selected.idName == character.idName)
            {
                backgorund.color = new Color(0.255f, 0.153f, 0.243f);
            }

            sprite.sprite = character.Icon;
            structureSprite.sprite = character.startingItem.icon;

            if (!CharacterSelector.IsUnlocked(character))
            {
                sprite.color = Color.black;
                structureSprite.color = Color.black;
            }
        }

        private void ChangeColorBackToNormal()
        {
            backgorund.color = new Color(0.427f, 0.455f, 0.518f);
        }

        private void OnEnable()
        {
            Unselect.AddListener(this.ChangeColorBackToNormal);
        }

        private void OnDisable()
        {
            Unselect.RemoveListener(this.ChangeColorBackToNormal);
        }
    }
}
