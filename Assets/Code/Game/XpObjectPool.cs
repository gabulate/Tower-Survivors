using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerSurvivors.PickUps;
using TowerSurvivors.PlayerScripts;
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

        public void SpawnXp(int xp, Vector3 position)
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
                int randomIndex = UnityEngine.Random.Range(0, activeXpPickUps.Count);
                randomXP = activeXpPickUps[randomIndex];
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(randomXP.transform.position, groupRange, _xpLayer);

            int totalXp = randomXP.Xp;
            randomXP.gameObject.SetActive(false);

            for (int i = 0; i < hits.Length; i++)
            {
                totalXp += hits[i].GetComponent<XpPickUp>().Xp;
                hits[i].gameObject.SetActive(false);
                enabledXpObjects--;
            }

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
    }
}
