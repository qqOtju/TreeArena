using System;
using System.Collections.Generic;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.Factory.Enemy;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Spawner
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private Collider2D _leftSpawnArea;
        [SerializeField] private Collider2D _leftBottomSpawnArea;
        [SerializeField] private Collider2D _bottomSpawnArea;
        [SerializeField] private Collider2D _rightBottomSpawnArea;
        [SerializeField] private Collider2D _rightSpawnArea;

        private readonly List<EnemyBase> _activeEnemies = new();
        
        private BasicEnemyFactory _basicEnemyFactory;
        private FastEnemyFactory _fastEnemyFactory;
        private TankEnemyFactory _tankEnemyFactory;
        
        public event Action<EnemyBase> OnEnemySpawn;
        public event Action<EnemyBase> OnEnemyDeath; 
        public event Action OnAllEnemiesDead;

        [Inject]
        private void Construct(BasicEnemyFactory basicEnemyFactory, 
            FastEnemyFactory fastEnemyFactory, TankEnemyFactory tankEnemyFactory)
        {
            _basicEnemyFactory = basicEnemyFactory;
            _fastEnemyFactory = fastEnemyFactory;
            _tankEnemyFactory = tankEnemyFactory;
        }

        private void OnHealthChange(OnHealthChangeArgs<EnemyBase> obj)
        {
            if (obj.Type != HeathChangeType.Death) return;
            obj.Object.OnHealthChange -= OnHealthChange;
            OnEnemyDeath?.Invoke(obj.Object);
            RemoveEnemy(obj.Object);
        }
        
        private void RemoveEnemy(EnemyBase enemy)
        {
            if (!_activeEnemies.Contains(enemy)) return;
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count == 0)
                OnAllEnemiesDead?.Invoke();
            Debug.Log($"Enemy removed. Active enemies count: {_activeEnemies.Count}");
        }

        private Vector2 GetSpawnPosition(Collider2D spawnArea)
        {
            var spawnPosition = new Vector2(
                UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y));
            return spawnPosition;
        }
        
        private void SpawnEnemy(Collider2D spawnArea, EnemyFactory enemyFactory)
        {
            var enemy = enemyFactory.Create();
            enemy.transform.position = GetSpawnPosition(spawnArea);
            enemy.OnHealthChange += OnHealthChange;
            _activeEnemies.Add(enemy);
            OnEnemySpawn?.Invoke(enemy);
            Debug.Log($"Enemy spawned. Active enemies count: {_activeEnemies.Count}");
        }

        public void SpawnEnemy(SpawnPoint spawnPoint, EnemyType enemyType)
        {
            var area = spawnPoint switch
            {
                SpawnPoint.Left => _leftSpawnArea,
                SpawnPoint.BottomLeft => _leftBottomSpawnArea,
                SpawnPoint.Bottom => _bottomSpawnArea,
                SpawnPoint.BottomRight => _rightBottomSpawnArea,
                SpawnPoint.Right => _rightSpawnArea,
                _ => throw new ArgumentOutOfRangeException(nameof(spawnPoint), spawnPoint, null)
            };
            switch (enemyType)
            {
                case EnemyType.Basic:
                    SpawnEnemy(area, _basicEnemyFactory);
                    break;
                case EnemyType.Fast:
                    SpawnEnemy(area, _fastEnemyFactory);
                    break;
                case EnemyType.Tank:
                    SpawnEnemy(area, _tankEnemyFactory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null);
            }
        }
        
        public IReadOnlyList<EnemyBase> GetActiveEnemies()
        {
            return _activeEnemies.AsReadOnly();
        }
    }
}