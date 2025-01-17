using Project.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Menu.Options
{
    [RequireComponent(typeof(Canvas))]
    public class UIOptions: MonoBehaviour
    {
        [SerializeField] private UIVolume _masterVolume;
        [SerializeField] private UIVolume _musicVolume;
        [SerializeField] private UIVolume _uiVolume;
        [SerializeField] private UIVolume _sfxVolume;
        [SerializeField] private Button _closeButton;

        private AudioController _audioController;
        private AudioVolume _audioVolume;
        private Canvas _canvas;
        
        [Inject]
        private void Construct(AudioController audioController, AudioVolume audioVolume)
        {
            _audioController = audioController;
            _audioVolume = audioVolume;
        }

        private void Awake()
        {
            _masterVolume.OnVolumeChanged += SetMasterVolume;
            _musicVolume.OnVolumeChanged += SetMusicVolume;
            _uiVolume.OnVolumeChanged += SetUIVolume;
            _sfxVolume.OnVolumeChanged += SetSFXVolume;
            _closeButton.onClick.AddListener(Close);
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _masterVolume.Initialize("Master" ,_audioVolume.MasterVolume);
            _musicVolume.Initialize("Music", _audioVolume.MusicVolume);
            _uiVolume.Initialize("UI", _audioVolume.UIVolume);
            _sfxVolume.Initialize("SFX", _audioVolume.SFXVolume);
        }

        private void SetMasterVolume(float obj)
        {
            _audioVolume.MasterVolume = obj;
        }

        private void SetMusicVolume(float obj)
        {
            _audioVolume.MusicVolume = obj;
        }

        private void SetUIVolume(float obj)
        {
            _audioVolume.UIVolume = obj;
        }

        private void SetSFXVolume(float obj)
        {
            _audioVolume.SFXVolume = obj;
        }

        private void Close()
        {
            _audioController.PlayButtonClick();
            _canvas.enabled = false;
        }
    }
}