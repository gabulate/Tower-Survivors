using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public static AudioPlayer Instance;
        public int PoolLenght = 10;
        public bool Expandable = false;
        public int maximumSounds = 20;

        private List<SoundObject> _pooledSounds;
        [SerializeField]
        private GameObject _prefab;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        public void PlaySFX(SoundClip soundClip)
        {
            SoundObject so = GetPooledSound();
            so.gameObject.SetActive(true);
            so.PlayFX(soundClip);
        }

        public void PlaySFX(SoundClip soundClip, Vector3 position)
        {
            SoundObject so = GetPooledSound();
            so.gameObject.SetActive(true);
            so.PlayFX(soundClip, position);
        }

        private void Start()
        {
            _pooledSounds = new List<SoundObject>();
            for (int i = 0; i < PoolLenght; i++)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pooledSounds.Add(obj.GetComponent<SoundObject>());
            }
        }

        private SoundObject GetPooledSound()
        {
            for (int i = 0; i < _pooledSounds.Count; i++)
            {
                if (!_pooledSounds[i].gameObject.activeInHierarchy)
                {
                    return _pooledSounds[i];
                }
            }

            if (Expandable && _pooledSounds.Count < maximumSounds)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                SoundObject so = obj.GetComponent<SoundObject>();
                _pooledSounds.Add(so);
                PoolLenght++;
                return so;
            }

            return null;
        }
    }
}
