using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using UnityEngine;

namespace TowerSurvivors.Audio
{
    public class SoundObject : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;
        [SerializeField]
        private SoundClip clip;

        public void PlayFX(SoundClip soundClip)
        {
            source.spatialBlend = 0.8f;

            clip = soundClip;
            source.clip = soundClip.clip;
            source.volume = soundClip.volume * GameSettings.SFXVolume;

            VaryPitch();
            source.Play();
            StartCoroutine(DisableObject());
        }

        public void PlayFX(SoundClip soundClip, Vector3 position)
        {
            transform.position = position;
            source.spatialBlend = 0;

            clip = soundClip;
            source.clip = soundClip.clip;
            source.volume = soundClip.volume * GameSettings.SFXVolume;

            VaryPitch();
            source.Play();
            StartCoroutine(DisableObject());
        }

        private IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(source.clip.length + 1);
            gameObject.SetActive(false);
        }

        private void VaryPitch()
        {
            if (clip.varyPitch)
            {
                source.pitch = Random.Range(clip.minPitch, clip.maxPitch);
            }
            else
            {
                source.pitch = clip.pitch;
            }
        }

        private void OnPause(bool paused)
        {
            if (!paused)
            {
                source.volume = source.volume = clip.volume * GameSettings.SFXVolume;
            }
        }

        private void OnEnable()
        {
            if (GameManager.Instance)
                GameManager.Instance.e_Paused.AddListener(OnPause);
        }

        private void OnDisable()
        {
            if (GameManager.Instance)
                GameManager.Instance.e_Paused.RemoveListener(OnPause);
        }
    }
}
