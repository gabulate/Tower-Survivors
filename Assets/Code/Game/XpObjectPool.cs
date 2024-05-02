using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.PickUps;
using TowerSurvivors.PlayerScripts;
using TowerSurvivors.VFX;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class XpObjectPool : MonoBehaviour
    {
        protected static readonly LayerMask _xpLayer = 1 << 11;


        public static XpObjectPool Instance;
        public int PoolLenght = 20;
        public bool Expandable = false;
        public int maximumLenght = 30;
        public int enabledXpObjects = 0;
        public int groupAfter = 50; // The amount of xp orbs that need to exist before they start grouping for performance
        public float groupRange = 3;

        private List<XpPickUp> _pooledObjects;
        [SerializeField]
        private GameObject _prefab;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            _pooledObjects = new List<XpPickUp>();
            for (int i = 0; i < PoolLenght; i++)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pooledObjects.Add(obj.GetComponent<XpPickUp>());
            }
        }

        public void SpawnXp(uint xp, Vector3 position)
        {
            if(enabledXpObjects > groupAfter)
            {
                Group();
            }


            XpPickUp xpP = GetPooledObject();

            xpP.Xp = xp;
            xpP.transform.position = position;
            xpP.gameObject.SetActive(true);
            enabledXpObjects++;
        }

        private void Group()
        {
            XpPickUp randomXP = null;
            List<XpPickUp> activeXpPickUps = _pooledObjects.Where(x => x.gameObject.activeSelf).ToList();

            if (activeXpPickUps.Count > 0)
            {
                //get a random active xp object to use its position
                int randomIndex = UnityEngine.Random.Range(0, activeXpPickUps.Count);
                randomXP = activeXpPickUps[randomIndex];
            }

            uint totalXp = randomXP.Xp;
            //Disable the random xp to avoid hitting it and duplicating xp
            randomXP.gameObject.SetActive(false);
            enabledXpObjects--;

            Collider2D[] hits = Physics2D.OverlapCircleAll(randomXP.transform.position, groupRange, _xpLayer);
            //If didnt get any other xp in range, just return
            if(hits.Length == 0)
            {
                randomXP.gameObject.SetActive(true);
                enabledXpObjects++;
                return;
            }

            //If there are xp objects in range
            //Sum the total xp from all the objects hit and disable them
            for (int i = 0; i < hits.Length; i++)
            {
                totalXp += hits[i].GetComponent<XpPickUp>().Xp;
                hits[i].gameObject.SetActive(false);
                enabledXpObjects--;
            }

            //Play visual effect
            GameObject vfxObject = AssetsHolder.Instance.xpGroupVFX;
            vfxObject = Instantiate(vfxObject, new(randomXP.transform.position.x, randomXP.transform.position.y, randomXP.transform.position.y - 1), Quaternion.identity);
            VisualFX vfx = vfxObject.GetComponent<VisualFX>();
            vfx.ChangeColor(GetXpColor(totalXp));
            vfx.PlayEffect();

            //Spawn a new xp object with the total xp
            SpawnXp(totalXp, randomXP.transform.position);
        }

        public XpPickUp GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            if (Expandable && _pooledObjects.Count < maximumLenght)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                XpPickUp xpP = obj.GetComponent<XpPickUp>();
                _pooledObjects.Add(xpP);
                PoolLenght++;
                return xpP;
            }

            return null;
        }

        private Color GetXpColor(float xp)
        {
            if (xp == 1)
                return new Color(1.0f, 0.12156862745098039f, 0.20392156862745098f); // Red
            else if (xp <= 4)
                return new Color(0.2549019607843137f, 0.6941176470588235f, 0.984313725490196f); // Blue
            else if (xp <= 9)
                return new Color(1.0f, 0.807843137254902f, 0.0f); // Yellow
            else if (xp <= 19)
                return new Color(0.24705882352941178f, 0.9137254901960784f, 0.5529411764705883f); // BlueGreenish
            else if (xp <= 49)
                return new Color(1.0f, 0.596078431372549f, 0.0f); // Orange
            else
                return new Color(0.6705882352941176f, 0.0f, 1.0f); // Purple
        }
    }
}
