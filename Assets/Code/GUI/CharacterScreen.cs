using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.Audio;
using TowerSurvivors.Localisation;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class CharacterScreen : MonoBehaviour
    {
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI characterDsc;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI playText;
        public TextMeshProUGUI coinsText;
        public Image coinImage;

        public SoundClip errorSound;

        public static UnityEvent e_UnlockedCharacter = new UnityEvent();

        public void ShowCharacterDeets(CharacterSO character)
        {
            if (CharacterSelector.IsUnlocked(character))
            {
                characterName.text = Language.Get(character.NameKey);
                characterDsc.text = Language.Get(character.DescriptionKey);
                itemName.text = Language.Get(character.startingItem.itemNameKey);
                playText.text = Language.Get("PLAY");
                playText.color = Color.white;
                coinImage.enabled = false;
            }
            else
            {
                characterName.text = "???";
                characterDsc.text = "???";
                itemName.text = "???";
                playText.text = character.price.ToString();
                coinImage.enabled = true;

                if(SaveSystem.csd.coins < character.price)
                    playText.color = Color.red;
                else
                    playText.color = Color.white;
            }
        }

        public void PlayOrBuy()
        {
            if (CharacterSelector.IsUnlocked(CharacterSelector.selected))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                if (CharacterSelector.Unlock(CharacterSelector.selected))
                {
                    ShowCharacterDeets(CharacterSelector.selected);
                    coinsText.text = SaveSystem.csd.coins.ToString();
                    e_UnlockedCharacter.Invoke();
                }
                else
                {
                    AudioPlayer.Instance.PlaySFX(errorSound);
                }
            }
        }

        private void OnEnable()
        {
            CharacterOption.OnSelected.AddListener(ShowCharacterDeets);
            coinsText.text = SaveSystem.csd.coins.ToString();
        }

        private void OnDisable()
        {
            CharacterOption.OnSelected.RemoveListener(ShowCharacterDeets);
        }
    }
}
