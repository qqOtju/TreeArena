using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPauseMenu: MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _returnToMenuButton;
        [SerializeField] private Button _exitButton;
        
        private CanvasGroup _canvasGroup;
        private Canvas _pauseMenu;
        
        private void Awake()
        {
            _pauseMenu = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _resumeButton.onClick.AddListener(Resume);
            _returnToMenuButton.onClick.AddListener(ReturnToMenu);
            _exitButton.onClick.AddListener(Exit);
        }

        private void Resume()
        {
            Time.timeScale = 1;
            _pauseMenu.enabled = false;
        }

        private void ReturnToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void Exit()
        {
            Time.timeScale = 1;
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}