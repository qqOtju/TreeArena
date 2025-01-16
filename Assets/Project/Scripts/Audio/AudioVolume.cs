using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Audio
{
    [Serializable]
    public class AudioVolume
    {
        [SerializeField] private AudioClip[] _buttonClickClips;
        [SerializeField] private AudioClip _musicClip;
        
        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string UIVolumeKey = "UIVolume";
        private const string SFXVolumeKey = "SFXVolume";

        private float _masterVolume;
        private float _musicVolume;
        private float _uiVolume;
        private float _sfxVolume;
        
        public float MasterVolume
        {
            get => _masterVolume; 
            set
            {
                _masterVolume = value;
                PlayerPrefs.SetFloat(MasterVolumeKey, _masterVolume);
                OnMasterVolumeChanged?.Invoke(value); 
            }
        }
        public float MusicVolume
        {
            get => _musicVolume; 
            set
            {
                _musicVolume = value;
                PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolume);
                OnMusicVolumeChanged?.Invoke(value);
            }
        }
        public float UIVolume
        {
            get => _uiVolume; 
            set
            {
                _uiVolume = value;
                PlayerPrefs.SetFloat(UIVolumeKey, _uiVolume);
                OnUIVolumeChanged?.Invoke(value);
            }
        }
        public float SFXVolume
        {
            get => _sfxVolume; 
            set
            {
                _sfxVolume = value;
                PlayerPrefs.SetFloat(SFXVolumeKey, _sfxVolume);
                OnSFXVolumeChanged?.Invoke(value);
            }
        }
        
        public AudioClip MusicClip => _musicClip;
        public AudioClip ButtonClickClip => _buttonClickClips[Random.Range(0, _buttonClickClips.Length)];
        
        public event Action<float> OnMasterVolumeChanged;
        public event Action<float> OnMusicVolumeChanged;
        public event Action<float> OnUIVolumeChanged;
        public event Action<float> OnSFXVolumeChanged;

        public void Init()
        {
            Debug.Log("<color=green>AudioVolume created</color>");
            MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            UIVolume = PlayerPrefs.GetFloat(UIVolumeKey, 0.5f);
            SFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);
        }
    }
}