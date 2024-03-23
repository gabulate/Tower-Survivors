using UnityEngine;

namespace TowerSurvivors.Audio
{
    /// <summary>
    /// Custom Homemade Audio Reference
    /// </summary>
    [CreateAssetMenu(fileName = "AudioClip", menuName = "SoundClip")]
    public class SoundClip : ScriptableObject
    {
        public AudioClip clip;
        [Range(0, 1)]
        public float volume = 1f;
        [Range(-3, 3)]
        public float pitch = 1;

        public bool varyPitch;
        [Range(-3, 3)]
        public float minPitch = -1;
        [Range(-3, 3)]
        public float maxPitch = 2;
        public bool loop = false;

        public SoundClip()
        {
            volume = 1;
            pitch = 1;
            varyPitch = false;
        }

        //public SoundClip(float minPitch, float maxPitch)
        //{
        //    varyPitch = true;
        //    this.minPitch = minPitch;
        //    this.maxPitch = maxPitch;
        //}
    }
}
