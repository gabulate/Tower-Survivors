using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.GUI
{
    public class DamageNumberController : MonoBehaviour
    {
        public static DamageNumberController Instance;
        [Header("Numbers Pool Config")]
        public int PoolLenght = 1;
        public bool Expandable = true;
        public int maximumNumbers = 20;

        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private List<DamageNumber> _pooledSounds;

        public void ShowDamageNumber(int number, Vector3 position)
        {
            DamageNumber dn = GetPooledNumber();
            if (!dn)
                return;
            dn.transform.position = new Vector3(position.x, position.y, position.z - 2);
            dn.gameObject.SetActive(true);
            dn.Display(number);
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            _pooledSounds = new List<DamageNumber>();
            for (int i = 0; i < PoolLenght; i++)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pooledSounds.Add(obj.GetComponent<DamageNumber>());
            }
        }

        private DamageNumber GetPooledNumber()
        {
            for (int i = 0; i < _pooledSounds.Count; i++)
            {
                if (!_pooledSounds[i].gameObject.activeSelf)
                {
                    return _pooledSounds[i];
                }
            }

            if (Expandable && _pooledSounds.Count < maximumNumbers)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                DamageNumber dn = obj.GetComponent<DamageNumber>();
                _pooledSounds.Add(dn);
                PoolLenght++;
                return dn;
            }

            return null;
        }
    }
}
