using UnityEngine;
using TMPro;

namespace TowerSurvivors.GUI
{
    public class LanguageOption : MonoBehaviour
    {
        [SerializeReference]
        private string language = "English";
        public TextMeshProUGUI text;

        public void SetLanguage()
        {
            AppManager.Instance.SetLanguage(language);
        }

        public void Inistialise(string language)
        {
            this.language = language;
            text.text = language;
        }
    }
}
