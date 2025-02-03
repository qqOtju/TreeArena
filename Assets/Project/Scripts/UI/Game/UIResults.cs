using System;
using Project.Scripts.Audio;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.GameLogic.Wave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Tree = Project.Scripts.GameLogic.Tree;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(Canvas))]
    public class UIResults: MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Button _button;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private WavesController _wavesController;
        [SerializeField] private Tree _tree;

        private AudioController _audioController;
        private Canvas _canvas;
        private bool _lose;

        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }

        private void Awake()
        {
            _tree.OnHealthChange += OnTreeHealthChange;
            _wavesController.OnAllWavesEnd += OnAllWavesEnd;
            _mainMenuButton.onClick.AddListener(MainMenu);
            _exitButton.onClick.AddListener(Exit);
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
        }

        private void OnDestroy()
        {
            _tree.OnHealthChange -= OnTreeHealthChange;
            _wavesController.OnAllWavesEnd -= OnAllWavesEnd;
            _mainMenuButton.onClick.RemoveListener(MainMenu);
            _exitButton.onClick.RemoveListener(Exit);
            _button.onClick.RemoveAllListeners();
        }

        private void MainMenu()
        {
            Time.timeScale = 1;
            _audioController.PlayButtonClick();
            _audioController.StopMusic();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void Exit()
        {
            _audioController.PlayButtonClick();
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnTreeHealthChange(OnHealthChangeArgs<Tree> obj)
        {
            if (obj.Value <= 0 && !_lose)
                Lose();
        }

        private void Lose()
        {
            _canvas.enabled = true;
            _lose = true;
            Time.timeScale = 0;
            _resultText.text = "You lose";
            _buttonText.text = "Restart";
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            _audioController.PlayButtonClick();
            Time.timeScale = 1;
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        private void OnAllWavesEnd()
        {
            _canvas.enabled = true;
            _resultText.text = "You win";
            _buttonText.text = "Continue";
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(Continue);
        }

        private void Continue()
        {
            _audioController.PlayButtonClick();
            Time.timeScale = 1;
            _canvas.enabled = false;
            _wavesController.StartInfinityWaves();
        }
    }
}