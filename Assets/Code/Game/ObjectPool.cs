using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.Game
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        public int PoolLenght = 20;
        public bool Expandable = false;

        private List<GameObject> _pooledObjects;
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
            _pooledObjects = new List<GameObject>();
            for (int i = 0; i < PoolLenght; i++)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            if (Expandable)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
                PoolLenght++;
                return obj;
            }

            return null;
        }
    }
}
