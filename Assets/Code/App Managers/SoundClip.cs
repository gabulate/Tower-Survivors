using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TowerSurvivors.Audio
{
    [System.Serializable]
    public class SoundClip
    {
        public AudioClip clip;
        [Range(0, 1)]
        public float volume = 0.5f;
        [Range(-3, 3)]
        public float pitch = 1;
        
        public bool varyPitch;
        [Range(-3, 3)]
        public float minPitch = -1;
        [Range(-3, 3)]
        public float maxPitch = 2;

        public SoundClip(float minPitch, float maxPitch)
        {
            varyPitch = true;
            this.minPitch = minPitch;
            this.maxPitch = maxPitch;
        }
    }
}
