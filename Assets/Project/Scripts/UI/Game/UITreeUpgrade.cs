using System;
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
        private Canvas _canvas;
        
        public event Action OnClose; 

        [Inject]
        private void Construct(TreeUpgradeSystem treeUpgradeSystem)
        {
            _treeUpgradeSystem = treeUpgradeSystem;
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
            _maxHealthUpgradeValue.text = $"+{obj.BonusMaxHealthPerLvl} Max Health";
            _regenUpgradeValue.text = $"+{obj.BonusRegenPerLvl} Regen";
            _armorUpgradeValue.text = $"+{obj.BonusArmorPerLvl} Armor";
            _resistanceUpgradeValue.text = $"{obj.BonusResistancePerLvl} Resistance";
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
            _treeUpgradeSystem.UpgradeMaxHealth();
            Close();
        }

        private void UpgradeRegen()
        {
            _treeUpgradeSystem.UpgradeRegen();
            Close();
        }

        private void UpgradeArmor()
        {
            _treeUpgradeSystem.UpgradeArmor();
            Close();    
        }

        private void UpgradeResistance()
        {
            _treeUpgradeSystem.UpgradeResistance();
            Close();
        }
    }
}