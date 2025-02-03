using System;
using Project.Scripts.Audio;
using Project.Scripts.Config;
using Project.Scripts.Module.System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(Canvas))]
    public class UIWeaponUpgradeMenu: MonoBehaviour
    {
        [Title("Stats")]
        [SerializeField] private TMP_Text _firstStatText;
        [SerializeField] private TMP_Text _secondStatText;
        [SerializeField] private TMP_Text _thirdStatText;
        [SerializeField] private TMP_Text _fourthStatText;
        [Title("Stats Icons")]
        [SerializeField] private Image _firstStatIcon;
        [SerializeField] private Image _secondStatIcon;
        [SerializeField] private Image _thirdStatIcon;
        [SerializeField] private Image _fourthStatIcon;
        [Title("Upgrades")]
        [SerializeField] private UIWeaponUpgrade _firstUpgrade;
        [SerializeField] private UIWeaponUpgrade _secondUpgrade;
        [SerializeField] private UIWeaponUpgrade _thirdUpgrade;
        [SerializeField] private UIWeaponUpgrade _fourthUpgrade;
        [Title("Icons")]
        [SerializeField] private Sprite _damageIcon;
        [SerializeField] private Sprite _attackSpeedIcon;
        [SerializeField] private Sprite _criticalChanceIcon;
        [SerializeField] private Sprite _criticalDamageIcon;
        [SerializeField] private Sprite _piercingIcon;
        [SerializeField] private Sprite _bulletCountIcon;
        [Title("Other")]
        [SerializeField] private UITreeUpgrade _treeUpgrade;
        [SerializeField] private Button _singleWeaponButton;
        [SerializeField] private Button _aoeWeaponButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _hideButton;
        [SerializeField] private TMP_Text _coinsText;

        private WeaponUpgradeSystem _weaponUpgradeSystem;
        private AudioController _audioController;
        private CoinSystem _coinSystem;
        private Canvas _canvas;
        
        public event Action OnClose;
        
        [Inject]
        private void Construct(WeaponUpgradeSystem weaponUpgradeSystem, 
            CoinSystem coinSystem, AudioController audioController)
        {
            _weaponUpgradeSystem = weaponUpgradeSystem;
            _audioController = audioController;
            _coinSystem = coinSystem;
        }
        
        private void Awake()
        {
            _treeUpgrade.OnClose += Open;
            _coinSystem.OnGoldChanged += UpdateCoins;
            _hideButton.onClick.AddListener(Hide);
            _closeButton.onClick.AddListener(Close);
            _singleWeaponButton.onClick.AddListener(OpenSingleWeaponMenu);
            _aoeWeaponButton.onClick.AddListener(OpenAoeWeaponMenu);
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            OpenAoeWeaponMenu();
            UpdateCoins(_coinSystem.CurrentGold);
        }
        
        private void OnDestroy()
        {
            _treeUpgrade.OnClose -= Open;
            _coinSystem.OnGoldChanged -= UpdateCoins;
            _hideButton.onClick.RemoveListener(Hide);
            _closeButton.onClick.RemoveListener(Close);
            _singleWeaponButton.onClick.RemoveListener(OpenSingleWeaponMenu);
            _aoeWeaponButton.onClick.RemoveListener(OpenAoeWeaponMenu);
            _firstUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _secondUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _thirdUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _fourthUpgrade.UpgradeButton.onClick.RemoveAllListeners();
        }

        private void UpdateCoins(int obj)
        {
            _coinsText.text = $"{obj} g";
        }

        private void OpenSingleWeaponMenu()
        {
            _audioController.PlayButtonClick();
            _weaponUpgradeSystem.OnAoeAttackStatChanged -= UpdateAoeWeaponStats;
            _weaponUpgradeSystem.OnSingleAttackStatChanged += UpdateSingleWeaponStats;
            UpdateSingleWeaponStats(_weaponUpgradeSystem.CurrentSingleAttackStat);
            SetSingleWeaponUpgrades();
        }
        
        private void OpenAoeWeaponMenu()
        {
            _audioController.PlayButtonClick();
            _weaponUpgradeSystem.OnSingleAttackStatChanged -= UpdateSingleWeaponStats;
            _weaponUpgradeSystem.OnAoeAttackStatChanged += UpdateAoeWeaponStats;
            UpdateAoeWeaponStats(_weaponUpgradeSystem.CurrentAoeAttackStat);
            SetAoeWeaponUpgrades();
        }

        private void UpdateSingleWeaponStats(SingleAttackStat singleAttackStat)
        {
            _firstStatText.text = $"{singleAttackStat.Damage:F0}";
            _secondStatText.text = $"{singleAttackStat.AttackSpeed:F2}";
            _thirdStatText.text = $"{singleAttackStat.CriticalChance:F0}";
            _fourthStatText.text = $"{singleAttackStat.CriticalDamage:F0}";
            _firstStatIcon.sprite = _damageIcon;
            _secondStatIcon.sprite = _attackSpeedIcon;
            _thirdStatIcon.sprite = _criticalChanceIcon;
            _fourthStatIcon.sprite = _criticalDamageIcon;
        }
        
        private void UpdateAoeWeaponStats(AoeAttackStat aoeAttackStat)
        {
            _firstStatText.text = $"{aoeAttackStat.Damage:F0}";
            _secondStatText.text = $"{aoeAttackStat.AttackSpeed:F2}";
            _thirdStatText.text = $"{aoeAttackStat.Piercing:F0}";
            _fourthStatText.text = $"{aoeAttackStat.BulletCount:F0}";
            _firstStatIcon.sprite = _damageIcon;
            _secondStatIcon.sprite = _attackSpeedIcon;
            _thirdStatIcon.sprite = _piercingIcon;
            _fourthStatIcon.sprite = _bulletCountIcon;
        }

        private void SetSingleWeaponUpgrades()
        {
            InitializeSingleWeaponUpgrades();
            _firstUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _secondUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _thirdUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _fourthUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _firstUpgrade.UpgradeButton.onClick.AddListener(UpgradeSingleDamage);
            _secondUpgrade.UpgradeButton.onClick.AddListener(UpgradeSingleAttackSpeed);
            _thirdUpgrade.UpgradeButton.onClick.AddListener(UpgradeSingleCriticalChance);
            _fourthUpgrade.UpgradeButton.onClick.AddListener(UpgradeSingleCriticalDamage);
        }

        private void SetAoeWeaponUpgrades()
        {
            InitializeAoeWeaponUpgrades();
            _firstUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _secondUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _thirdUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _fourthUpgrade.UpgradeButton.onClick.RemoveAllListeners();
            _firstUpgrade.UpgradeButton.onClick.AddListener(UpgradeAoeDamage);
            _secondUpgrade.UpgradeButton.onClick.AddListener(UpgradeAoeAttackSpeed);
            _thirdUpgrade.UpgradeButton.onClick.AddListener(UpgradeAoePiercing);
            _fourthUpgrade.UpgradeButton.onClick.AddListener(UpgradeAoeBulletCount);
        }
        
        private void InitializeSingleWeaponUpgrades()
        {
            var stat = _weaponUpgradeSystem.CurrentSingleAttackStat;
            var currentDamage = stat.Damage;
            var damageWithUpgrade = currentDamage + stat.BonusDamagePerLvl;
            var currentPrice = _weaponUpgradeSystem.CurrentSingleAttackPrice;
            _firstUpgrade.Initialize("Damage", 
            $"{currentDamage:F0}->{damageWithUpgrade:F0}", $"{currentPrice.DamagePrice}", _damageIcon, _weaponUpgradeSystem.SingleDamageLvl);
            var currentAttackSpeed = stat.AttackSpeed;
            var attackSpeedWithUpgrade = currentAttackSpeed + stat.BonusAttackSpeedPerLvl;
            _secondUpgrade.Initialize("Attack Speed",
            $"{currentAttackSpeed:F2}->{attackSpeedWithUpgrade:F2}", $"{currentPrice.AttackSpeedPrice}", _attackSpeedIcon, _weaponUpgradeSystem.SingleAttackSpeedLvl);
            var currentCriticalChance = stat.CriticalChance;
            var criticalChanceWithUpgrade = currentCriticalChance + stat.BonusCriticalChancePerLvl;
            _thirdUpgrade.Initialize("Critical Chance",
            $"{currentCriticalChance:F0}->{criticalChanceWithUpgrade:F0}", $"{currentPrice.CriticalChancePrice}", _criticalChanceIcon, _weaponUpgradeSystem.CriticalChanceLvl);
            var currentCriticalDamage = stat.CriticalDamage;
            var criticalDamageWithUpgrade = currentCriticalDamage + stat.BonusCriticalDamagePerLvl;
            _fourthUpgrade.Initialize("Critical Damage",
            $"{currentCriticalDamage:F0}->{criticalDamageWithUpgrade:F0}", $"{currentPrice.CriticalDamagePrice}", _criticalDamageIcon, _weaponUpgradeSystem.CriticalDamageLvl);
        }
        
        private void InitializeAoeWeaponUpgrades()
        {
            var stat = _weaponUpgradeSystem.CurrentAoeAttackStat;
            var currentDamage = stat.Damage;
            var damageWithUpgrade = currentDamage + stat.BonusDamagePerLvl;
            var currentPrice = _weaponUpgradeSystem.CurrentAoeAttackPrice;
            _firstUpgrade.Initialize("Damage", 
            $"{currentDamage:F0}->{damageWithUpgrade:F0}", $"{currentPrice.DamagePrice}", _damageIcon, _weaponUpgradeSystem.AoeDamageLvl);
            var currentAttackSpeed = stat.AttackSpeed;
            var attackSpeedWithUpgrade = currentAttackSpeed + stat.BonusAttackSpeedPerLvl;
            _secondUpgrade.Initialize("Fire Rate",
            $"{currentAttackSpeed:F2}->{attackSpeedWithUpgrade:F2}", $"{currentPrice.AttackSpeedPrice}", _attackSpeedIcon, _weaponUpgradeSystem.AoeAttackSpeedLvl);
            var currentPiercing = stat.Piercing;
            var piercingWithUpgrade = currentPiercing + stat.BonusPiercingPerLvl;
            _thirdUpgrade.Initialize("Piercing",
            $"{currentPiercing:F0}->{piercingWithUpgrade:F0}", $"{currentPrice.PiercingPrice}", _piercingIcon, _weaponUpgradeSystem.PiercingLvl);
            var currentBulletCount = stat.BulletCount;
            var bulletCountWithUpgrade = currentBulletCount + stat.BonusBulletCountPerLvl;
            _fourthUpgrade.Initialize("Bullet Count",
            $"{currentBulletCount:F0}->{bulletCountWithUpgrade:F0}", $"{currentPrice.BulletCountPrice}", _bulletCountIcon, _weaponUpgradeSystem.BulletCountLvl);
        }
        
        private void UpgradeSingleDamage()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentSingleAttackPrice.DamagePrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentSingleAttackPrice.DamagePrice;
            _weaponUpgradeSystem.UpgradeSingleDamage();
            InitializeSingleWeaponUpgrades();
        }
        
        private void UpgradeSingleAttackSpeed()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentSingleAttackPrice.AttackSpeedPrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentSingleAttackPrice.AttackSpeedPrice;
            _weaponUpgradeSystem.UpgradeSingleAttackSpeed();
            InitializeSingleWeaponUpgrades();
        }
        
        private void UpgradeSingleCriticalChance()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentSingleAttackPrice.CriticalChancePrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentSingleAttackPrice.CriticalChancePrice;
            _weaponUpgradeSystem.UpgradeCriticalChance();
            InitializeSingleWeaponUpgrades();
        }
        
        private void UpgradeSingleCriticalDamage()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentSingleAttackPrice.CriticalDamagePrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentSingleAttackPrice.CriticalDamagePrice;
            _weaponUpgradeSystem.UpgradeCriticalDamage();
            InitializeSingleWeaponUpgrades();
        }
        
        private void UpgradeAoeDamage()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentAoeAttackPrice.DamagePrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentAoeAttackPrice.DamagePrice;
            _weaponUpgradeSystem.UpgradeAoeDamage();
            InitializeAoeWeaponUpgrades();
        }
        
        private void UpgradeAoeAttackSpeed()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentAoeAttackPrice.AttackSpeedPrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentAoeAttackPrice.AttackSpeedPrice;
            _weaponUpgradeSystem.UpgradeAoeAttackSpeed();
            InitializeAoeWeaponUpgrades();
        }
        
        private void UpgradeAoePiercing()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentAoeAttackPrice.PiercingPrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentAoeAttackPrice.PiercingPrice;
            _weaponUpgradeSystem.UpgradePiercing();
            InitializeAoeWeaponUpgrades();
        }

        private void UpgradeAoeBulletCount()
        {
            _audioController.PlayButtonClick();
            if(_coinSystem.CurrentGold < _weaponUpgradeSystem.CurrentAoeAttackPrice.BulletCountPrice) return;
            _coinSystem.CurrentGold -= _weaponUpgradeSystem.CurrentAoeAttackPrice.BulletCountPrice;
            _weaponUpgradeSystem.UpgradeBulletCount();
            InitializeAoeWeaponUpgrades();
        }
        
        private void Close()
        {
            _audioController.PlayButtonClick();
            _canvas.enabled = false;
            OnClose?.Invoke();
        }

        private void Hide()
        {
            _audioController.PlayButtonClick();
            _canvas.enabled = false;
        }

        public void Open()
        {
            _audioController.PlayButtonClick();
            _canvas.enabled = true;
        }
    }
}