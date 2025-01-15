using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config
{
    [CreateAssetMenu(fileName = "TreeConfig", menuName = "Config/TreeConfig")]
    public class TreeConfig: ScriptableObject
    {
        [SerializeField] private TreeStat _treeStat;
        
        public TreeStat TreeStat => _treeStat;
    }

    [Serializable]
    public struct TreeStat
    {
        [Title("Values")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _regen;
        [SerializeField] private int _armor;
        [SerializeField] private float _resistance;
        [Title("Rules")]
        [SerializeField] private int _bonusMaxHealthPerLvl;
        [SerializeField] private float _bonusRegenPerLvl;
        [SerializeField] private int _bonusArmorPerLvl;
        [SerializeField] private float _bonusResistancePerLvl;
        
        public int MaxHealth => _maxHealth;
        public float Regen => _regen;
        public int Armor => _armor;
        public float Resistance => _resistance;
        
        public int BonusMaxHealthPerLvl => _bonusMaxHealthPerLvl;
        public float BonusRegenPerLvl => _bonusRegenPerLvl;
        public int BonusArmorPerLvl => _bonusArmorPerLvl;
        public float BonusResistancePerLvl => _bonusResistancePerLvl;
        
        public void MultiplyAllByLvl(int lvl)
        {
            _maxHealth += _bonusMaxHealthPerLvl * lvl;
            _regen += _bonusRegenPerLvl * lvl;
            _armor += _bonusArmorPerLvl * lvl;
            _resistance += _bonusResistancePerLvl * lvl;
        }
        
        public void MultiplyMaxHealthByLvl(int lvl)
        {
            _maxHealth += _bonusMaxHealthPerLvl * lvl;
        }
        
        public void MultiplyRegenByLvl(int lvl)
        {
            _regen += _bonusRegenPerLvl * lvl;
        }
        
        public void MultiplyArmorByLvl(int lvl)
        {
            _armor += _bonusArmorPerLvl * lvl;
        }
        
        public void MultiplyResistanceByLvl(int lvl)
        {
            _resistance += _bonusResistancePerLvl * lvl;
        }
        
        public static TreeStat operator +(TreeStat a, TreeStat b)
        {
            return new TreeStat
            {
                _maxHealth = a._maxHealth + b._maxHealth,
                _regen = a._regen + b._regen,
                _armor = a._armor + b._armor,
                _resistance = a._resistance + b._resistance
            };
        }
        
        public static TreeStat operator -(TreeStat a, TreeStat b)
        {
            return new TreeStat
            {
                _maxHealth = a._maxHealth - b._maxHealth,
                _regen = a._regen - b._regen,
                _armor = a._armor - b._armor,
                _resistance = a._resistance - b._resistance
            };
        }
        
        public static TreeStat operator *(TreeStat a, int b)
        {
            return new TreeStat
            {
                _maxHealth = a._maxHealth * b,
                _regen = a._regen * b,
                _armor = a._armor * b,
                _resistance = a._resistance * b
            };
        }
    }
}