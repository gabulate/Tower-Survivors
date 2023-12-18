using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using UnityEngine;

namespace TowerSurvivors.PassiveItems
{
    /// <summary>
    /// Parent class for Passive Items.
    /// </summary>
    public class PassiveItem : MonoBehaviour
    {
        public int level;
        public PassiveItemSO item;

        public virtual void ApplyEffect()
        {
            Player.Instance.ApplyBuffs();
        }

        public virtual void RemoveEffect()
        {
            Player.Instance.ApplyBuffs();
        }
    }
}
