using System;
using Project.Scripts.Audio;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Tree = Project.Scripts.GameLogic.Tree;

namespace Project.Scripts.UI.Game
{
    public class UIGame: MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Button _showShopButton;
        [SerializeField] private UIWeaponUpgradeMenu _weaponUpgradeMenu;
        [SerializeField] private WavesController _wavesController;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _aoeButton;
        [SerializeField] private Button _singleButton;
        [SerializeField] private Canvas _pauseMenu;
        
        private AudioController _audioController;
        private CoinSystem _coinSystem;
        private Tree _tree;
        
        public event Action OnAoeAttack;
        public event Action OnSingleAttack; 

        [Inject]    
        private void Construct(Tree tree, CoinSystem coinSystem, AudioController audioController)
        {
            _tree = tree;
            _coinSystem = coinSystem;
            _audioController = audioController;
        }

        private void Awake()
        {
            _tree.OnHealthChange += OnHealthChange;
            _coinSystem.OnGoldChanged += OnGoldChanged;
            _wavesController.OnWaveEnd += OnWaveEnd;
            _wavesController.OnWaveStart += OnWaveStart;
            _showShopButton.onClick.AddListener(ShowShop);
            _pauseButton.onClick.AddListener(Pause);
            _aoeButton.onClick.AddListener(SetAoeAttack);
            _singleButton.onClick.AddListener(SetSingleAttack);
        }

        private void Start()
        {
            _audioController.PlayMusic();
            _showShopButton.gameObject.SetActive(false);
            _healthBar.value = 1;
            _healthText.text = $"{_tree.CurrentHealth}/{_tree.MaxHealth}";
            OnGoldChanged(_coinSystem.CurrentGold);
        }

        private void OnDestroy()
        {
            _tree.OnHealthChange -= OnHealthChange;
            _coinSystem.OnGoldChanged -= OnGoldChanged;
            _wavesController.OnWaveEnd -= OnWaveEnd;
            _wavesController.OnWaveStart -= OnWaveStart;
            _showShopButton.onClick.RemoveListener(ShowShop);
            _pauseButton.onClick.RemoveListener(Pause);
            _aoeButton.onClick.RemoveListener(SetAoeAttack);
            _singleButton.onClick.RemoveListener(SetSingleAttack);
        }

        private void Pause()
        {
            _audioController.PlayButtonClick();
            Time.timeScale = 0;
            _pauseMenu.enabled = true;
        }

        private void OnHealthChange(OnHealthChangeArgs<Tree> obj)
        {
            _healthBar.value = obj.Object.CurrentHealth / obj.Object.MaxHealth;
            _healthText.text = $"{obj.Object.CurrentHealth:F0}/{obj.Object.MaxHealth}";
        }

        private void OnGoldChanged(int obj)
        {
            _goldText.text = $"{obj} g";
        }

        private void OnWaveStart(int obj)
        {
            _showShopButton.gameObject.SetActive(false);
        }

        private void OnWaveEnd(int obj)
        {
            _showShopButton.gameObject.SetActive(true);
        }

        private void ShowShop()
        {
            _audioController.PlayButtonClick();
            _weaponUpgradeMenu.Open();
        }

        private void SetAoeAttack()
        {
            _audioController.PlayButtonClick();
            OnAoeAttack?.Invoke();
        }
        
        private void SetSingleAttack()
        {
            _audioController.PlayButtonClick();
            OnSingleAttack?.Invoke();
        }
    }
}