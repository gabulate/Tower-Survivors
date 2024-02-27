using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerSurvivors.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public static AudioPlayer Instance;
        [Header("Sounds Pool Config")]
        public int PoolLenght = 10;
        public bool Expandable = false;
        public int maximumSounds = 20;
        [SerializeField]
        private GameObject _prefab;
        private List<SoundObject> _pooledSounds;

        [Header("Game Music Config")]
        public SoundObject musicObject;
        public SoundClip musicTrack;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        public void PlayMusic()
        {
            musicObject.PlayMusic(musicTrack);
        }

        public void PlayMusic(SoundClip musicTrack)
        {
            StopMusic();
            this.musicTrack = musicTrack;
            musicObject.PlayMusic(musicTrack);
            musicObject.FadeIn(7);
        }

        public void StopMusic()
        {
            musicObject.Stop();
        }

        public void PauseMusic(bool pause)
        {
            if (pause)
                musicObject.Pause();
            else
                musicObject.UnPause();
        }

        public void LowerVolume(bool pause)
        {
            if (pause)
                musicObject.SetMusicVolume(0.25f);
            else
                musicObject.SetMusicVolume(1);
        }

        public void PlaySFX(SoundClip soundClip)
        {
            if (!soundClip)
                return;

            SoundObject so = GetPooledSound();
            if (!so)
                return;

            so.gameObject.SetActive(true);
            so.PlayFX(soundClip);
        }

        public void PlaySFX(SoundClip soundClip, Vector3 position)
        {
            if (!soundClip)
                return;

            SoundObject so = GetPooledSound();
            if (!so)
                return;

            so.gameObject.SetActive(true);
            so.PlayFX(soundClip, position);

        }

        /// <summary>
        /// Plays a repeating sound following the position of a transform.
        /// </summary>
        /// <param name="soundClip">soundClip to be played.</param>
        /// <param name="timeBetween">Time between each play.</param>
        public void PlayRepeating(SoundClip soundClip, float timeBetween)
        {
            SoundObject so = GetPooledSound();
            if (!so)
                return;

            so.gameObject.SetActive(true);
            StartCoroutine(so.PlayRepeating(soundClip, timeBetween));
        }

        /// <summary>
        /// Plays a repeating sound following the position of a transform.
        /// </summary>
        /// <param name="soundClip">soundClip to be played.</param>
        /// <param name="timeBetween">Time between each play.</param>
        /// <param name="transform">Transform from where the sound is gonna be played.</param>
        public void PlayRepeating(SoundClip soundClip, float timeBetween, Transform transform)
        {
            SoundObject so = GetPooledSound();
            so.gameObject.SetActive(true);
            StartCoroutine(so.PlayRepeating(soundClip, timeBetween, transform));
        }

        public void StopRepeating(SoundClip soundClip)
        {
            SoundObject so = _pooledSounds.Where(s => s.gameObject.activeInHierarchy && s.clip == soundClip).FirstOrDefault();
            so.Stop();
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
