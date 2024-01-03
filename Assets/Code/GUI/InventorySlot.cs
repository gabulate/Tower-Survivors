using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class InventorySlot : MonoBehaviour
    {
        public Image icon;

        public Color normalColor;
        public Color selectedColor;
        public void HighLight()
        {
            icon.color = selectedColor;
        }

        public void UnHighLight()
        {
            icon.color = normalColor;
        }
    }
}
