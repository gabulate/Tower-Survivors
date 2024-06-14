using System.Collections;
using TowerSurvivors.Game;
using UnityEngine;

namespace TowerSurvivors.Audio
{
    public class SoundObject : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;
        public SoundClip clip;
        [SerializeField]
        private bool musicMode = false;
        private bool repeating = false;

        /// <summary>
        /// Plays a sound to be heard everywhere.
        /// </summary>
        /// <param name="soundClip">The soundClip to be played.</param>
        public void PlayFX(SoundClip soundClip)
        {
            source.spatialBlend = 0.0f;

            clip = soundClip;
            PlayFX();
        }

        /// <summary>
        /// Plays a sound with 3D spatial blend.
        /// </summary>
        /// <param name="soundClip">The soundClip to be played.</param>
        /// <param name="position">The position from where the sound will be heard.</param>
        public void PlayFX(SoundClip soundClip, Vector3 position)
        {
            transform.position = position;
            source.spatialBlend = 0.8f;

            clip = soundClip;
            PlayFX();
        }

        public void FadeIn(float seconds)
        {
            StartCoroutine(FadeInCoroutine(seconds));
        }

        private IEnumerator FadeInCoroutine(float seconds)
        {
            SetMusicVolume(0);
            float elapsedTime = 0f;

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                SetMusicVolume((elapsedTime / seconds));
                yield return null;
            }
            SetMusicVolume(1);
        }

        private void PlayFX()
        {
            source.clip = clip.clip;
            source.volume = clip.volume * GameSettings.SFXVolume;

            VaryPitch();
            source.Play();
            StartCoroutine(DisableObject());
        }

        /// <summary>
        /// Plays a music track and loops it.
        /// </summary>
        /// <param name="musicTrack">the music track to be played.</param>
        public void PlayMusic(SoundClip musicTrack)
        {
            musicMode = true;
            source.spatialBlend = 0;

            clip = musicTrack;
            source.clip = musicTrack.clip;
            source.volume = musicTrack.volume * GameSettings.MusicVolume;
            source.loop = true;
            VaryPitch();
            source.Play();

            if(GameManager.Instance)
                GameManager.Instance.e_Paused.AddListener(OnPause);
        }

        public void Pause()
        {
            source.Pause();
        }

        public void UnPause()
        {
            source.UnPause();
        }

        /// <summary>
        /// Stops the sound or music being played.
        /// </summary>
        public void Stop()
        {
            source.Stop();
            repeating = false;
        }

        public IEnumerator PlayRepeating(SoundClip soundClip, float timeBetween)
        {
            source.spatialBlend = 0.0f;

            clip = soundClip;

            WaitForSeconds waitSeconds = new WaitForSeconds(timeBetween);

            while (repeating)
            {
                PlayFX();
                yield return waitSeconds;
            }
        }

        public IEnumerator PlayRepeating(SoundClip soundClip, float timeBetween, Transform transform)
        {
            source.spatialBlend = 0.8f;

            clip = soundClip;
            repeating = true;

            WaitForSeconds waitSeconds = new WaitForSeconds(timeBetween);

            while (repeating)
            {
                transform.position = transform.position;
                PlayFX();
                yield return waitSeconds;
            }
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
            if (paused) Pause();
            else UnPause();

            if (musicMode)
            {
                source.volume = clip.volume * GameSettings.MusicVolume;
            }
            else
            {
                source.volume = clip.volume * GameSettings.SFXVolume;
            }
        }

        /// <summary>
        /// Changes the volume of the sound object by a percentaje of the volume set in the settings.
        /// </summary>
        /// <param name="percentaje">0 for completely silent and 1 for the volume set by the user.</param>
        public void SetMusicVolume(float percentaje)
        {
            source.volume = clip.volume * percentaje * GameSettings.MusicVolume;
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
