using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Project.Scripts.Audio
{
    public class AudioController: MonoBehaviour
    {
        
        [Header("Audio Sources")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource _audioSourceUI;
        [SerializeField] private AudioSource _audioSourceSFX;
    
        private AudioVolume _audioData;
        
        [Inject]
        public void Construct(AudioVolume audioData)
        {
            _audioData = audioData;
            SetMasterVolume(_audioData.MasterVolume);
            SetMusicVolume(_audioData.MusicVolume);
            SetUIVolume(_audioData.UIVolume);
            SetSFXVolume(_audioData.SFXVolume);
            _audioData.OnMasterVolumeChanged += SetMasterVolume;
            _audioData.OnMusicVolumeChanged += SetMusicVolume;
            _audioData.OnUIVolumeChanged += SetUIVolume;
            _audioData.OnSFXVolumeChanged += SetSFXVolume;
        }

        private void SetMasterVolume(float obj) => 
            _audioMixer.SetFloat("Master", Mathf.Log10(obj) * 20);

        private void SetMusicVolume(float obj) =>
            _audioSourceMusic.volume = obj;

        private void SetUIVolume(float obj) => 
            _audioSourceUI.volume = obj;

        private void SetSFXVolume(float obj) => 
            _audioSourceSFX.volume = _audioData.SFXVolume;

        private void Awake()
        {
            var go = gameObject;
            go.transform.parent = null;
            DontDestroyOnLoad(go);
        }

        public void PlayButtonClick() =>
            _audioSourceUI.PlayOneShot(_audioData.ButtonClickClip);

        public void PlayUISound(AudioClip clip) =>
            _audioSourceUI.PlayOneShot(clip);
        
        public void PlayMusic()
        { 
            _audioSourceMusic.Stop();
            _audioSourceMusic.clip = _audioData.MusicClip;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
        
        public void PlayMusic(AudioClip clip)
        {
            _audioSourceMusic.Stop();
            _audioSourceMusic.clip = clip;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
        
        public void StopMusic() =>
            _audioSourceMusic.Stop();
        
        public void PlaySFX(AudioClip clip) =>
            _audioSourceSFX.PlayOneShot(clip);       
    }
}