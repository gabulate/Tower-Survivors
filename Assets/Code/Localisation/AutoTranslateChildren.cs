using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerSurvivors.Localisation
{
    public class AutoTranslateChildren : MonoBehaviour
    {
        public void Translate()
        {
            foreach(TextMeshProUGUI t in GetComponentsInChildren<TextMeshProUGUI>())
            {
                t.text = Language.Get(t.text);
            }
        }

    }
}
