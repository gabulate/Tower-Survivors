using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerSurvivors.Localisation
{
    public class AutoTranslateChildren : MonoBehaviour
    {
        public bool translateOnStart = false;
        private bool _translated = false;

        public void Translate()
        {
            if (_translated)
            {
                Debug.LogWarning("Tried to translate twice!");
                return;
            }

            foreach (TextMeshProUGUI t in GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                t.text = Language.Get(t.text);
            }
            _translated = true;
        }

        private void Start()
        {
            if (translateOnStart && !_translated)
            {
                Translate();
            }
        }
    }
}
