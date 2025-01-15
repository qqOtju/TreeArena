using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config
{
    [CreateAssetMenu(fileName = "SingleAttackConfig", menuName = "Config/SingleAttackConfig")]
    public class SingleAttackConfig: ScriptableObject
    {
        [SerializeField] private SingleAttackStat _stat;
        [SerializeField] private SingleAttackPrice _price;
        
        public SingleAttackStat Stat => _stat;
        public SingleAttackPrice Price => _price;
    }

    [Serializable]
    public struct SingleAttackStat
    {
        [Title("Values")]
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _criticalDamage;
        [Title("Rules")]
        [SerializeField] private float _bonusDamagePerLvl;
        [SerializeField] private float _bonusAttackSpeedPerLvl;
        [SerializeField] private float _bonusCriticalChancePerLvl;
        [SerializeField] private float _bonusCriticalDamagePerLvl;

        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public float CriticalChance => _criticalChance;
        public float CriticalDamage => _criticalDamage;
        public float BonusDamagePerLvl => _bonusDamagePerLvl;
        public float BonusAttackSpeedPerLvl => _bonusAttackSpeedPerLvl;
        public float BonusCriticalChancePerLvl => _bonusCriticalChancePerLvl;
        public float BonusCriticalDamagePerLvl => _bonusCriticalDamagePerLvl;
        
        public void MultiplyAllByLvl(int lvl)
        {
            _attackSpeed += _bonusAttackSpeedPerLvl * lvl;
            _damage += _bonusDamagePerLvl * lvl;
            _criticalChance += _bonusCriticalChancePerLvl * lvl;
            _criticalDamage += _bonusCriticalDamagePerLvl * lvl;
        }
        
        public void MultiplyAttackSpeedByLvl(int lvl)
        {
            _attackSpeed += _bonusAttackSpeedPerLvl * lvl;
        }
        
        public void MultiplyDamageByLvl(int lvl)
        {
            _damage += _bonusDamagePerLvl * lvl;
        }
        
        public void MultiplyCriticalChanceByLvl(int lvl)
        {
            _criticalChance += _bonusCriticalChancePerLvl * lvl;
        }
        
        public void MultiplyCriticalDamageByLvl(int lvl)
        {
            _criticalDamage += _bonusCriticalDamagePerLvl * lvl;
        }

        public static SingleAttackStat operator +(SingleAttackStat a, SingleAttackStat b)
        {
            return new SingleAttackStat
            {
                _attackSpeed = a._attackSpeed + b._attackSpeed,
                _damage = a._damage + b._damage,
                _criticalChance = a._criticalChance + b._criticalChance,
                _criticalDamage = a._criticalDamage + b._criticalDamage
            };
        }
    }

    [Serializable]
    public struct SingleAttackPrice
    {
        [Header("Base")]
        [SerializeField] private int _damagePrice;
        [SerializeField] private int _attackSpeedPrice;
        [SerializeField] private int _criticalChancePrice;
        [SerializeField] private int _criticalDamagePrice;
        [Header("Per Lvl")]
        [SerializeField] private int _damagePricePerLvl;
        [SerializeField] private int _attackSpeedPricePerLvl;
        [SerializeField] private int _criticalChancePricePerLvl;
        [SerializeField] private int _criticalDamagePricePerLvl;
        
        public int DamagePrice => _damagePrice;
        public int AttackSpeedPrice => _attackSpeedPrice;
        public int CriticalChancePrice => _criticalChancePrice;
        public int CriticalDamagePrice => _criticalDamagePrice;
        public int DamagePricePerLvl => _damagePricePerLvl;
        public int AttackSpeedPricePerLvl => _attackSpeedPricePerLvl;
        public int CriticalChancePricePerLvl => _criticalChancePricePerLvl;
        public int CriticalDamagePricePerLvl => _criticalDamagePricePerLvl;
        
        public void MultiplyAllByLvl(int lvl)
        {
            _damagePrice += _damagePricePerLvl * lvl;
            _attackSpeedPrice += _attackSpeedPricePerLvl * lvl;
            _criticalChancePrice += _criticalChancePricePerLvl * lvl;
            _criticalDamagePrice += _criticalDamagePricePerLvl * lvl;
        }
        
        public void MultiplyDamagePriceByLvl(int lvl)
        {
            _damagePrice += _damagePricePerLvl * lvl;
        }
        
        public void MultiplyAttackSpeedPriceByLvl(int lvl)
        {
            _attackSpeedPrice += _attackSpeedPricePerLvl * lvl;
        }
        
        public void MultiplyCriticalChancePriceByLvl(int lvl)
        {
            _criticalChancePrice += _criticalChancePricePerLvl * lvl;
        }
        
        public void MultiplyCriticalDamagePriceByLvl(int lvl)
        {
            _criticalDamagePrice += _criticalDamagePricePerLvl * lvl;
        }
        
        public static SingleAttackPrice operator +(SingleAttackPrice a, SingleAttackPrice b)
        {
            return new SingleAttackPrice
            {
                _damagePrice = a._damagePrice + b._damagePrice,
                _attackSpeedPrice = a._attackSpeedPrice + b._attackSpeedPrice,
                _criticalChancePrice = a._criticalChancePrice + b._criticalChancePrice,
                _criticalDamagePrice = a._criticalDamagePrice + b._criticalDamagePrice
            };
        }
    }
}