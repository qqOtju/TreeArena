using System;
using Project.Scripts.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPauseMenu: MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _returnToMenuButton;
        [SerializeField] private Button _exitButton;
        
        private AudioController _audioController;
        private Canvas _pauseMenu;

        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }
        
        private void Awake()
        {
            _pauseMenu = GetComponent<Canvas>();
            _resumeButton.onClick.AddListener(Resume);
            _returnToMenuButton.onClick.AddListener(ReturnToMenu);
            _exitButton.onClick.AddListener(Exit);
        }

        private void OnDestroy()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _returnToMenuButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

        private void Resume()
        {
            _audioController.PlayButtonClick();
            Time.timeScale = 1;
            _pauseMenu.enabled = false;
        }

        private void ReturnToMenu()
        {
            _audioController.PlayButtonClick();
            _audioController.StopMusic();
            Time.timeScale = 1;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void Exit()
        {
            _audioController.PlayButtonClick();
            _audioController.StopMusic();
            Time.timeScale = 1;
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}