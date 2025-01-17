using System;
using Project.Scripts.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Menu
{
    [RequireComponent(typeof(Canvas))]
    public class UIMainMenu: MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Canvas _options;

        private AudioController _audioController;
        
        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }

        private void Awake()
        {
            _startButton.onClick.AddListener(StartGame);
            _optionsButton.onClick.AddListener(ToggleOptions);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void StartGame()
        {
            _audioController.PlayButtonClick();
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        private void ToggleOptions()
        {
            _audioController.PlayButtonClick();
            _options.enabled = !_options.enabled;
        }

        private void ExitGame()
        {
            _audioController.PlayButtonClick();
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}