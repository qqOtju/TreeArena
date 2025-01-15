using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.GameLogic.Wave
{
    [Serializable]
    public struct WaveContent
    {
        [EnumToggleButtons]
        [SerializeField] private SpawnPoint _spawnPoint;
        [EnumToggleButtons]
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private int _enemyCount;
        [Tooltip("Time between enemy spawns")]
        [SerializeField] private float _spawnDelay;
        [Tooltip("Time before spawn after wave start")]
        [SerializeField] private float _waveDelay;
        
        public SpawnPoint SpawnPoint => _spawnPoint;
        public EnemyType EnemyType => _enemyType;
        public int EnemyCount => _enemyCount;
        public float SpawnDelay => _spawnDelay;
        public float WaveDelay => _waveDelay;
    }
}