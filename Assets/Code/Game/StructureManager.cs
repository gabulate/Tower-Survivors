using TowerSurvivors.Audio;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.ScriptableObjects;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSurvivors.Game
{
    public class StructureManager : MonoBehaviour
    {
        [SerializeField]
        private Color _outlineColor;
        [SerializeField]
        private Color _highlightColor;

        public static StructureManager Instance;
        public SoundClip placeSound;
        public SoundClip cantPlaceSound;
        public SoundClip structureUpgradeSound;

        public int initialMaximumStructures = 5;
        public int MaximumStructures = 5;
        public Transform placedStructres;
        public Transform hiddenStructures;
        public Structure[] structures;

        public UnityEvent<int, int> e_StAmntChanged;

        public void PlaceStructure(Structure structure)
        {
            Structure[] structures = GetStructures();

            if (structures.Length >= MaximumStructures)
                return;

            structure.transform.parent = placedStructres;
            structure.transform.position = new Vector3(structure.transform.position.x, structure.transform.position.y, structure.transform.position.y);
            structure.ApplyBuffs(Player.Instance.stats);
            structure.EnableStructure(true);
            structure.OutLine(false);

            AudioPlayer.Instance.PlaySFX(placeSound, structure.transform.position);

            e_StAmntChanged.Invoke(structures.Length + 1, MaximumStructures);
        }

        public void ReplaceStructure(Structure original, Structure replacement)
        {
            replacement.transform.parent = placedStructres;
            replacement.transform.position = original.transform.position;
            replacement.EnableStructure(true);

            original.transform.parent = hiddenStructures;
            original.EnableStructure(false);
            original.gameObject.SetActive(false);
            Destroy(original, 1);

            e_StAmntChanged.Invoke(structures.Length, MaximumStructures);
            replacement.ApplyBuffs(Player.Instance.stats);
            structures = placedStructres.GetComponentsInChildren<Structure>();
        }

        public Structure[] GetStructures()
        {
            if (placedStructres.childCount != structures.Length)
            {
                structures = placedStructres.GetComponentsInChildren<Structure>();
                return structures;
            }
            return structures;
        }

        public void HighLigthTypeOf(Structure structure)
        {
            foreach(Structure s in GetStructures())
            {
                if(s.GetType() == structure.GetType() && !s.isMaxed)
                {
                    SpriteRenderer[] sprites = s.GetComponentsInChildren<SpriteRenderer>();

                    foreach (SpriteRenderer sprite in sprites)
                    {
                        sprite.material.SetColor("_OutlineColor", _highlightColor);
                    }
                }
            }
        }


        public void UnhighlightAll()
        {
            //Yes, this is highly unefficient
            foreach(Structure s in GetStructures())
            {
                SpriteRenderer[] sprites = s.GetComponentsInChildren<SpriteRenderer>();

                foreach(SpriteRenderer sprite in sprites)
                {
                    sprite.material.SetColor("_OutlineColor", _outlineColor);
                }
            }
        }

        public void DisableAllStructures()
        {
            foreach(Structure s in GetStructures())
            {
                s.canAttack = false;
            }
        }

        public GameObject AddToInventory(StructureItemSO item, int index)
        {
            //Instantiates the structure into the scene
            GameObject go = Instantiate(item.prefab, hiddenStructures);
            Structure structure = go.GetComponent<Structure>();
            structure.stats.range += Player.Instance.stats.rangeIncrease;

            structure.EnableStructure(false);
            go.SetActive(false);

            return go;
        }

        public void PickUpStructure(Structure structure)
        {
            structure.transform.parent = hiddenStructures;
            structure.EnableStructure(false);
            structure.gameObject.SetActive(false);

            int structureQty = GetStructures().Length;
            e_StAmntChanged.Invoke(structureQty, MaximumStructures);
        }

        public bool CanPlace()
        {
            return GetStructures().Length < MaximumStructures;
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        public void IncreaseStructureLimit(int extraStructures)
        {
            MaximumStructures = initialMaximumStructures + extraStructures;
            e_StAmntChanged.Invoke(structures.Length, MaximumStructures);
        }
    }
}
