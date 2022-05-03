using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Control;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Santuary.Harry.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] Slider masterVolumeSlider, sfxVolumeSlider, musicVolumeSlider;

        void Start()
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        }

        public void ChangeMasterVolumeSlider()
        {
            float sliderValue = masterVolumeSlider.value;
            audioMixer.SetFloat("masterVolume", Mathf.Log10(sliderValue)*20);
            PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        }
        public void ChangeSFXVolumeSlider()
        {
            float sliderValue = sfxVolumeSlider.value;
            audioMixer.SetFloat("soundVolume", Mathf.Log10(sliderValue)*20);
            PlayerPrefs.SetFloat("SoundVolume", sliderValue);
        }
        public void ChangeMusicVolumeSlider()
        {
            float sliderValue = musicVolumeSlider.value;
            audioMixer.SetFloat("musicVolume", Mathf.Log10(sliderValue)*20);
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
    }
}
