using System;
using Project.Scripts.Config;

namespace Project.Scripts.Module.System
{
    public class WeaponUpgradeSystem
    {
        private readonly SingleAttackConfig _singleAttackConfig;
        private readonly AoeAttackConfig _aoeAttackConfig;
        
        private int _singleDamageLvl;
        private int _singleAttackSpeedLvl;
        private int _criticalChanceLvl;
        private int _criticalDamageLvl;
        
        private int _aoeDamageLvl;
        private int _aoeAttackSpeedLvl;
        private int _piercingLvl;
        private int _bulletCountLvl;
        
        public SingleAttackStat CurrentSingleAttackStat { get; private set; }
        public SingleAttackPrice CurrentSingleAttackPrice => GetSingleAttackPrice();
        public AoeAttackStat CurrentAoeAttackStat { get; private set; }
        public AoeAttackPrice CurrentAoeAttackPrice => GetAoeAttackPrice();

        public event Action<SingleAttackStat> OnSingleAttackStatChanged;
        public event Action<AoeAttackStat> OnAoeAttackStatChanged;
        
        public WeaponUpgradeSystem(SingleAttackConfig singleAttackConfig, AoeAttackConfig aoeAttackConfig)
        {
            _singleAttackConfig = singleAttackConfig;
            _aoeAttackConfig = aoeAttackConfig;
            SetAllStats();
        }

        private void SetAllStats()
        {
            SetSingleStats();
            SetAoeStats();
        }

        private void SetSingleStats()
        {
            var singleAttackStat = _singleAttackConfig.Stat;
            singleAttackStat.MultiplyDamageByLvl(_singleDamageLvl);
            singleAttackStat.MultiplyAttackSpeedByLvl(_singleAttackSpeedLvl);
            singleAttackStat.MultiplyCriticalChanceByLvl(_criticalChanceLvl);
            singleAttackStat.MultiplyCriticalDamageByLvl(_criticalDamageLvl);
            CurrentSingleAttackStat = singleAttackStat;
            OnSingleAttackStatChanged?.Invoke(singleAttackStat);
        }

        private void SetAoeStats()
        {
            var aoeAttackStat = _aoeAttackConfig.Stat;
            aoeAttackStat.MultiplyDamageByLvl(_aoeDamageLvl);
            aoeAttackStat.MultiplyAttackSpeedByLvl(_aoeAttackSpeedLvl);
            aoeAttackStat.MultiplyPiercingByLvl(_piercingLvl);
            aoeAttackStat.MultiplyBulletCountByLvl(_bulletCountLvl);
            CurrentAoeAttackStat = aoeAttackStat;
            OnAoeAttackStatChanged?.Invoke(aoeAttackStat);
        }
        
        private SingleAttackPrice GetSingleAttackPrice()
        {
            var singleAttackPrice = _singleAttackConfig.Price;
            singleAttackPrice.MultiplyDamagePriceByLvl(_singleDamageLvl);
            singleAttackPrice.MultiplyAttackSpeedPriceByLvl(_singleAttackSpeedLvl);
            singleAttackPrice.MultiplyCriticalChancePriceByLvl(_criticalChanceLvl);
            singleAttackPrice.MultiplyCriticalDamagePriceByLvl(_criticalDamageLvl);
            return singleAttackPrice;
        }
        
        private AoeAttackPrice GetAoeAttackPrice()
        {
            var aoeAttackPrice = _aoeAttackConfig.Price;
            aoeAttackPrice.MultiplyDamagePriceByLvl(_aoeDamageLvl);
            aoeAttackPrice.MultiplyAttackSpeedPriceByLvl(_aoeAttackSpeedLvl);
            aoeAttackPrice.MultiplyPiercingPriceByLvl(_piercingLvl);
            aoeAttackPrice.MultiplyBulletCountPriceByLvl(_bulletCountLvl);
            return aoeAttackPrice;
        }
        
        public void UpgradeSingleDamage()
        {
            _singleDamageLvl++;
            SetSingleStats();
        }
        
        public void UpgradeSingleAttackSpeed()
        {
            _singleAttackSpeedLvl++;
            SetSingleStats();
        }
        
        public void UpgradeCriticalChance()
        {
            _criticalChanceLvl++;
            SetSingleStats();
        }
        
        public void UpgradeCriticalDamage()
        {
            _criticalDamageLvl++;
            SetSingleStats();
        }
        
        public void UpgradeAoeDamage()
        {
            _aoeDamageLvl++;
            SetAoeStats();
        }
        
        public void UpgradeAoeAttackSpeed()
        {
            _aoeAttackSpeedLvl++;
            SetAoeStats();
        }
        
        public void UpgradePiercing()
        {
            _piercingLvl++;
            SetAoeStats();
        }
        
        public void UpgradeBulletCount()
        {
            _bulletCountLvl++;
            SetAoeStats();
        }
    }
}