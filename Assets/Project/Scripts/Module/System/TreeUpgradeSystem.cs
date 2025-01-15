using System;
using Project.Scripts.Config;

namespace Project.Scripts.Module.System
{
    public class TreeUpgradeSystem
    {
        private readonly TreeConfig _config;

        private int _maxHealthLvl;
        private int _regenLvl;
        private int _armorLvl;
        private int _resistanceLvl;

        public TreeStat CurrentTreeStat { get; private set; }

        public event Action<TreeStat> OnTreeStatChanged; 

        public TreeUpgradeSystem(TreeConfig config)
        {
            _config = config;
            SetStats();
        }

        private void SetStats()
        {
            var treeStat = _config.TreeStat;
            treeStat.MultiplyMaxHealthByLvl(_maxHealthLvl);
            treeStat.MultiplyRegenByLvl(_regenLvl);
            treeStat.MultiplyArmorByLvl(_armorLvl);
            treeStat.MultiplyResistanceByLvl(_resistanceLvl);
            CurrentTreeStat = treeStat;
            OnTreeStatChanged?.Invoke(treeStat);
        }
        
        public void UpgradeMaxHealth()
        {
            _maxHealthLvl++;
            SetStats();
        }
        
        public void UpgradeRegen()
        {
            _regenLvl++;
            SetStats();
        }
        
        public void UpgradeArmor()
        {
            _armorLvl++;
            SetStats();
        }
        
        public void UpgradeResistance()
        {
            _resistanceLvl++;
            SetStats();
        }
    }
}