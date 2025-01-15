using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
    public class EnemyConfig: ScriptableObject
    {
        [SerializeField] private EnemyStat _enemyStat;
        
        public EnemyStat EnemyStat => _enemyStat;
    }
    
    [Serializable]
    public struct EnemyStat
    {
        [Title("Values")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _attackRange;
        [Title("Rules")]
        [SerializeField] private int _maxHealthBonusPerWave;
        [SerializeField] private float _moveSpeedBonusPerWave;
        [SerializeField] private float _damageBonusPerWave;
        [SerializeField] private float _attackSpeedBonusPerWave;
        [SerializeField] private float _attackRangeBonusPerWave;

        public int MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public float AttackRange => _attackRange;
        
        public void MultiplyByWave(int wave)
        {
            _maxHealth += _maxHealthBonusPerWave * wave;
            _moveSpeed += _moveSpeedBonusPerWave * wave;
            _damage += _damageBonusPerWave * wave;
            _attackSpeed += _attackSpeedBonusPerWave * wave;
            _attackRange += _attackRangeBonusPerWave * wave;
        }
        
        public static EnemyStat operator +(EnemyStat a, EnemyStat b)
        {
            return new EnemyStat
            {
                _maxHealth = a._maxHealth + b._maxHealth,
                _moveSpeed = a._moveSpeed + b._moveSpeed,
                _damage = a._damage + b._damage,
                _attackSpeed = a._attackSpeed + b._attackSpeed,
                _attackRange = a._attackRange + b._attackRange
            };
        }
    }
}