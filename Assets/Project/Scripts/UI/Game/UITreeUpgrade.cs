using System;
using Project.Scripts.Audio;
using Project.Scripts.Config;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(Canvas))]
    public class UITreeUpgrade: MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private TMP_Text _maxHealthText;
        [SerializeField] private TMP_Text _regenText;
        [SerializeField] private TMP_Text _armorText;
        [SerializeField] private TMP_Text _resistanceText;
        [Header("Upgrade Values")]
        [SerializeField] private TMP_Text _maxHealthUpgradeValue;
        [SerializeField] private TMP_Text _regenUpgradeValue;
        [SerializeField] private TMP_Text _armorUpgradeValue;
        [SerializeField] private TMP_Text _resistanceUpgradeValue;
        [Header("Buttons")]
        [SerializeField] private Button _maxHealthButton;
        [SerializeField] private Button _regenButton;
        [SerializeField] private Button _armorButton;
        [SerializeField] private Button _resistanceButton;
        [Header("Other")]
        [SerializeField] private WavesController _wavesController;

        private TreeUpgradeSystem _treeUpgradeSystem;
        private AudioController _audioController;
        private Canvas _canvas;
        
        public event Action OnClose; 

        [Inject]
        private void Construct(TreeUpgradeSystem treeUpgradeSystem, AudioController audioController)
        {
            _treeUpgradeSystem = treeUpgradeSystem;
            _audioController = audioController;
        }
        
        private void Awake()
        {
            _treeUpgradeSystem.OnTreeStatChanged += UpdateText;
            _wavesController.OnWaveEnd += Open;
            _maxHealthButton.onClick.AddListener(UpgradeMaxHealth);
            _regenButton.onClick.AddListener(UpgradeRegen);
            _armorButton.onClick.AddListener(UpgradeArmor);
            _resistanceButton.onClick.AddListener(UpgradeResistance);
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            UpdateText(_treeUpgradeSystem.CurrentTreeStat);
        }
        
        private void OnDestroy()
        {
            _treeUpgradeSystem.OnTreeStatChanged -= UpdateText;
        }

        private void UpdateText(TreeStat obj)
        {
            _maxHealthText.text = $"{obj.MaxHealth}";
            _regenText.text = $"{obj.Regen}";
            _armorText.text = $"{obj.Armor}";
            _resistanceText.text = $"{obj.Resistance}";  
            _maxHealthUpgradeValue.text = $"Health +{obj.BonusMaxHealthPerLvl}";
            _regenUpgradeValue.text = $"Regen +{obj.BonusRegenPerLvl}";
            _armorUpgradeValue.text = $"Armor +{obj.BonusArmorPerLvl}";
            _resistanceUpgradeValue.text = $"Resist +{obj.BonusResistancePerLvl}";
        }
        
        private void Close()
        {
            _canvas.enabled = false;
            OnClose?.Invoke();
        }

        private void Open(int obj)
        {
            _canvas.enabled = true;
        }

        private void UpgradeMaxHealth()
        {
            _audioController.PlayButtonClick();
            _treeUpgradeSystem.UpgradeMaxHealth();
            Close();
        }

        private void UpgradeRegen()
        {
            _audioController.PlayButtonClick();
            _treeUpgradeSystem.UpgradeRegen();
            Close();
        }

        private void UpgradeArmor()
        {
            _audioController.PlayButtonClick();
            _treeUpgradeSystem.UpgradeArmor();
            Close();    
        }

        private void UpgradeResistance()
        {
            _audioController.PlayButtonClick();
            _treeUpgradeSystem.UpgradeResistance();
            Close();
        }
    }
}