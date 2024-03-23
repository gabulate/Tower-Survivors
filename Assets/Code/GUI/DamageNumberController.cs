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
        private List<DamageNumber> _pooledNumbers;

        public void ShowDamageNumber(int number, Vector3 position)
        {
            DamageNumber dn = GetPooledNumber();
            if (!dn)
                return;
            dn.transform.position = new Vector3(position.x, position.y, position.z - 4);
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
            _pooledNumbers = new List<DamageNumber>();
            for (int i = 0; i < PoolLenght; i++)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pooledNumbers.Add(obj.GetComponent<DamageNumber>());
            }
        }

        private DamageNumber GetPooledNumber()
        {
            for (int i = 0; i < _pooledNumbers.Count; i++)
            {
                if (!_pooledNumbers[i].gameObject.activeSelf)
                {
                    return _pooledNumbers[i];
                }
            }

            if (Expandable && _pooledNumbers.Count < maximumNumbers)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                DamageNumber dn = obj.GetComponent<DamageNumber>();
                _pooledNumbers.Add(dn);
                PoolLenght++;
                return dn;
            }

            return null;
        }
    }
}
