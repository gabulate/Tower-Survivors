using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class InventoryItem : MonoBehaviour
    {
        public ItemSO item;
        public Image icon;
        private Material _mat;

        public void InitialiseItem(ItemSO newItem)
        {
            item = newItem;
            icon.sprite = newItem.icon;
        }

        public void HighLight()
        {
            if(_mat == null)
            {
                _mat = Instantiate(icon.material);
                icon.material = _mat;
            }
            _mat.SetFloat("_ShowOutline_ON", 1.0f);
        }

        public void UnHighLight()
        {
            if (_mat == null)
            {
                _mat = Instantiate(icon.material);
                icon.material = _mat;
            }
            _mat.SetFloat("_ShowOutline_ON", 0.0f);
        }
    }
}
