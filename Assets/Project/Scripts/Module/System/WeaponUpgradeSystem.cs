using System;
using Project.Scripts.Config;

namespace Project.Scripts.Module.System
{
    public class WeaponUpgradeSystem
    {
        private readonly SingleAttackConfig _singleAttackConfig;
        private readonly AoeAttackConfig _aoeAttackConfig;

        public int SingleDamageLvl { get; private set; }
        public int SingleAttackSpeedLvl { get; private set; }
        public int CriticalChanceLvl { get; private set; }
        public int CriticalDamageLvl { get; private set; }

        public int AoeDamageLvl { get; private set; }
        public int AoeAttackSpeedLvl { get; private set; }
        public int PiercingLvl { get; private set; }
        public int BulletCountLvl { get; private set; }

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
            singleAttackStat.MultiplyDamageByLvl(SingleDamageLvl);
            singleAttackStat.MultiplyAttackSpeedByLvl(SingleAttackSpeedLvl);
            singleAttackStat.MultiplyCriticalChanceByLvl(CriticalChanceLvl);
            singleAttackStat.MultiplyCriticalDamageByLvl(CriticalDamageLvl);
            CurrentSingleAttackStat = singleAttackStat;
            OnSingleAttackStatChanged?.Invoke(singleAttackStat);
        }

        private void SetAoeStats()
        {
            var aoeAttackStat = _aoeAttackConfig.Stat;
            aoeAttackStat.MultiplyDamageByLvl(AoeDamageLvl);
            aoeAttackStat.MultiplyAttackSpeedByLvl(AoeAttackSpeedLvl);
            aoeAttackStat.MultiplyPiercingByLvl(PiercingLvl);
            aoeAttackStat.MultiplyBulletCountByLvl(BulletCountLvl);
            CurrentAoeAttackStat = aoeAttackStat;
            OnAoeAttackStatChanged?.Invoke(aoeAttackStat);
        }
        
        private SingleAttackPrice GetSingleAttackPrice()
        {
            var singleAttackPrice = _singleAttackConfig.Price;
            singleAttackPrice.MultiplyDamagePriceByLvl(SingleDamageLvl);
            singleAttackPrice.MultiplyAttackSpeedPriceByLvl(SingleAttackSpeedLvl);
            singleAttackPrice.MultiplyCriticalChancePriceByLvl(CriticalChanceLvl);
            singleAttackPrice.MultiplyCriticalDamagePriceByLvl(CriticalDamageLvl);
            return singleAttackPrice;
        }
        
        private AoeAttackPrice GetAoeAttackPrice()
        {
            var aoeAttackPrice = _aoeAttackConfig.Price;
            aoeAttackPrice.MultiplyDamagePriceByLvl(AoeDamageLvl);
            aoeAttackPrice.MultiplyAttackSpeedPriceByLvl(AoeAttackSpeedLvl);
            aoeAttackPrice.MultiplyPiercingPriceByLvl(PiercingLvl);
            aoeAttackPrice.MultiplyBulletCountPriceByLvl(BulletCountLvl);
            return aoeAttackPrice;
        }
        
        public void UpgradeSingleDamage()
        {
            SingleDamageLvl++;
            SetSingleStats();
        }
        
        public void UpgradeSingleAttackSpeed()
        {
            SingleAttackSpeedLvl++;
            SetSingleStats();
        }
        
        public void UpgradeCriticalChance()
        {
            CriticalChanceLvl++;
            SetSingleStats();
        }
        
        public void UpgradeCriticalDamage()
        {
            CriticalDamageLvl++;
            SetSingleStats();
        }
        
        public void UpgradeAoeDamage()
        {
            AoeDamageLvl++;
            SetAoeStats();
        }
        
        public void UpgradeAoeAttackSpeed()
        {
            AoeAttackSpeedLvl++;
            SetAoeStats();
        }
        
        public void UpgradePiercing()
        {
            PiercingLvl++;
            SetAoeStats();
        }
        
        public void UpgradeBulletCount()
        {
            BulletCountLvl++;
            SetAoeStats();
        }
    }
}