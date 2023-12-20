using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerSurvivors.PassiveItems;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace TowerSurvivors.GUI
{
    public class InventoryItem : MonoBehaviour
    {
        public GameObject itemInstance;
        public ItemSO item;
        public Image icon;
        private Material _mat;
        public TextMeshProUGUI level;

        public void InitialiseItem(ItemSO newItem, GameObject instance)
        {
            item = newItem;
            icon.sprite = newItem.icon;
            itemInstance = instance;
            int l;

            if (newItem.GetType() == typeof(StructureItemSO))
                l = instance.GetComponent<Structure>().level;
            else
                l = instance.GetComponent<PassiveItem>().level;

            if(l > 1)
            {
                level.enabled = true;
                level.text = l.ToString();
            }
            else
            {
                level.enabled = false;
            }
        }

        internal void UpdateInfo()
        {
            icon.sprite = item.icon;
            int l;

            if (item.GetType() == typeof(StructureItemSO))
                l = itemInstance.GetComponent<Structure>().level;
            else
                l = itemInstance.GetComponent<PassiveItem>().level;

            if (l > 1)
            {
                level.enabled = true;
                level.text = l.ToString();
            }
            else
            {
                level.enabled = false;
            }
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
