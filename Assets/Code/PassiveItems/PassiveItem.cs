using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    /// <summary>
    /// Parent class for Passive Items.
    /// </summary>
    public class PassiveItem : MonoBehaviour
    {
        public int level = 1;
        public bool isMaxed = false;
        public PassiveItemSO item;

        public virtual void ApplyEffect()
        {
            Destroy(gameObject, 1f);
        }

        public virtual void RemoveEffect()
        {
        }
    }
}
