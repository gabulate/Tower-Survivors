using UnityEngine;
using TowerSurvivors.Localisation;

namespace TowerSurvivors
{
    public static class GameSettings
    {
        public static float MusicVolume { get; private set; } = 0.5f;
        public static float SFXVolume { get; private set; } = 0.5f;

        public static int fps;
        public static int QualityIndex;
        public static bool FullScreen;
        public static bool vSync = true;
        public static Vector2 resolution;

        public static void LoadSettings()
        {
            MusicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

            SetFPS(PlayerPrefs.GetInt("fps", 60));
            SetFullScreen(PlayerPrefs.GetInt("fullScreen", 1));
            SetVsync(PlayerPrefs.GetInt("vSync", 1));

            SetResolution(PlayerPrefs.GetInt("resX", Screen.currentResolution.width),
                PlayerPrefs.GetInt("resY", Screen.currentResolution.height));
        }

        public static void SetLanguage(string language)
        {
            PlayerPrefs.SetString("language", language);
        }

        public static void SetFPS(float fps)
        {
            Application.targetFrameRate = (int)fps;
            PlayerPrefs.SetInt("fps", (int)fps);
        }

        public static void SetVsync(bool enable)
        {
            if (enable)
            {
                QualitySettings.vSyncCount = 1;
                PlayerPrefs.SetInt("vSync", 1);
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                PlayerPrefs.SetInt("vSync", 0);
            }
        }

        public static void SetVsync(int oneOrZero)
        {
            QualitySettings.vSyncCount = oneOrZero;
            PlayerPrefs.SetInt("vSync", oneOrZero);

            if (oneOrZero == 0) SetFPS(PlayerPrefs.GetInt("fps", 60));
        }

        public static void SetQuality(int i)
        {
            QualitySettings.SetQualityLevel(i);
            PlayerPrefs.SetInt("quality", i);
            SetVsync(PlayerPrefs.GetInt("vSync", 1));
        }

        public static void SetFullScreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
            Debug.Log((fullscreen ? "F" : "Not f") + "ullscreen!"); //Magia increíble del gabo de 2022
            PlayerPrefs.SetInt("fullScreen", fullscreen ? 1 : 0);
        }

        public static void SetFullScreen(int OneOrZero)
        {
            Screen.fullScreen = OneOrZero == 1;
            PlayerPrefs.SetInt("fullScreen", OneOrZero);
        }

        public static void SetResolution(int x, int y)
        {

            Screen.SetResolution(x, y, Screen.fullScreen);
            PlayerPrefs.SetInt("resX", x);
            PlayerPrefs.SetInt("resY", y);
        }

        /// <summary>
        /// Sets volume for the music.
        /// </summary>
        /// <param name="volume">Volume to be set, range from 0 to 1.</param>
        public static void SetMusicVolume(float volume)
        {
            if (volume > 1)
            {
                MusicVolume = 1;
            }
            else
            {
                MusicVolume = volume;
            }
            PlayerPrefs.SetFloat("musicVolume", MusicVolume);
        }

        /// <summary>
        /// Sets volume for the sound effects.
        /// </summary>
        /// <param name="volume">Volume to be set, range from 0 to 1.</param>
        public static void SetSFXVolume(float volume)
        {
            if (volume > 1)
            {
                SFXVolume = 1;
                return;
            }
            else
            {
                SFXVolume = volume;
            }
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        }
    }
}