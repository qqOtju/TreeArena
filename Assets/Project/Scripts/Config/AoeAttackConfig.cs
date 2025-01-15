using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config
{
    [CreateAssetMenu(fileName = "AoeAttackConfig", menuName = "Config/AoeAttackConfig")]
    public class AoeAttackConfig: ScriptableObject
    {
        [SerializeField] private AoeAttackStat _stat;
        [SerializeField] private AoeAttackPrice _price;
        
        public AoeAttackStat Stat => _stat;
        public AoeAttackPrice Price => _price;
    }
    
    [Serializable]
    public struct AoeAttackStat
    {
        [Title("Stats")]
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private int _piercing;
        [SerializeField] private int _bulletCount;
        [Title("Rules")]
        [SerializeField] private float _bonusDamagePerLvl;
        [SerializeField] private float _bonusAttackSpeedPerLvl;
        [SerializeField] private int _bonusPiercingPerLvl;
        [SerializeField] private int _bonusBulletCountPerLvl;
        
        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public int Piercing => _piercing;
        public float BulletCount => _bulletCount;
        public float BonusDamagePerLvl => _bonusDamagePerLvl;
        public float BonusAttackSpeedPerLvl => _bonusAttackSpeedPerLvl;
        public int BonusPiercingPerLvl => _bonusPiercingPerLvl;
        public int BonusBulletCountPerLvl => _bonusBulletCountPerLvl;
        
        public void MultiplyAllByLvl(int lvl)
        {
            _damage += _bonusDamagePerLvl * lvl;
            _attackSpeed += _bonusAttackSpeedPerLvl * lvl;
            _piercing += _bonusPiercingPerLvl * lvl;
            _bulletCount += _bonusBulletCountPerLvl * lvl;
        }
        
        public void MultiplyDamageByLvl(int lvl)
        {
            _damage += _bonusDamagePerLvl * lvl;
        }
        
        public void MultiplyAttackSpeedByLvl(int lvl)
        {
            _attackSpeed += _bonusAttackSpeedPerLvl * lvl;
        }
        
        public void MultiplyPiercingByLvl(int lvl)
        {
            _piercing += _bonusPiercingPerLvl * lvl;
        }
        
        public void MultiplyBulletCountByLvl(int lvl)
        {
            _bulletCount += _bonusBulletCountPerLvl * lvl;
        }

        public static AoeAttackStat operator +(AoeAttackStat a, AoeAttackStat b)
        {
            return new AoeAttackStat
            {
                _damage = a._damage + b._damage,
                _attackSpeed = a._attackSpeed + b._attackSpeed,
                _piercing = a._piercing + b._piercing,
                _bulletCount = a._bulletCount + b._bulletCount
            };
        }
    }

    [Serializable]
    public struct AoeAttackPrice
    {
        [Header("Base")]
        [SerializeField] private int _damagePrice;
        [SerializeField] private int _attackSpeedPrice;
        [SerializeField] private int _piercingPrice;
        [SerializeField] private int _bulletCountPrice;
        [Header("Per Lvl")]
        [SerializeField] private int _damagePricePerLvl;
        [SerializeField] private int _attackSpeedPricePerLvl;
        [SerializeField] private int _piercingPricePerLvl;
        [SerializeField] private int _bulletCountPricePerLvl;
        
        public int DamagePrice => _damagePrice;
        public int AttackSpeedPrice => _attackSpeedPrice;
        public int PiercingPrice => _piercingPrice;
        public int BulletCountPrice => _bulletCountPrice;
        public int DamagePricePerLvl => _damagePricePerLvl;
        public int AttackSpeedPricePerLvl => _attackSpeedPricePerLvl;
        public int PiercingPricePerLvl => _piercingPricePerLvl;
        public int BulletCountPricePerLvl => _bulletCountPricePerLvl;
        
        public void MultiplyAllByLvl(int lvl)
        {
            _damagePrice += _damagePricePerLvl * lvl;
            _attackSpeedPrice += _attackSpeedPricePerLvl * lvl;
            _piercingPrice += _piercingPricePerLvl * lvl;
            _bulletCountPrice += _bulletCountPricePerLvl * lvl;
        }

        public void MultiplyDamagePriceByLvl(int lvl)
        {
            _damagePrice += _damagePricePerLvl * lvl;
        }
        
        public void MultiplyAttackSpeedPriceByLvl(int lvl)
        {
            _attackSpeedPrice += _attackSpeedPricePerLvl * lvl;
        }
        
        public void MultiplyPiercingPriceByLvl(int lvl)
        {
            _piercingPrice += _piercingPricePerLvl * lvl;
        }
        
        public void MultiplyBulletCountPriceByLvl(int lvl)
        {
            _bulletCountPrice += _bulletCountPricePerLvl * lvl;
        }
        
        public static AoeAttackPrice operator +(AoeAttackPrice a, AoeAttackPrice b)
        {
            return new AoeAttackPrice
            {
                _damagePrice = a._damagePrice + b._damagePrice,
                _attackSpeedPrice = a._attackSpeedPrice + b._attackSpeedPrice,
                _piercingPrice = a._piercingPrice + b._piercingPrice,
                _bulletCountPrice = a._bulletCountPrice + b._bulletCountPrice
            };
        }
    }
}