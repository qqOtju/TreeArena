using System;
using Project.Scripts.GameLogic.Dropdown;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.Module.Factory;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.Module.Spawner
{
    public class CoinSpawner: MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        
        private const float RandomSpawnRange = 0.5f;

        private CoinFactory _coinFactory;
        
        public event Action<Coin> OnCoinSpawn;

        [Inject]
        private void Construct(CoinFactory coinFactory)
        {
            _coinFactory = coinFactory;
        }
        
        private void Awake()
        {
            _enemySpawner.OnEnemyDeath += OnEnemyDeath;
        }

        private void OnDestroy()
        {
            _enemySpawner.OnEnemyDeath -= OnEnemyDeath;
        }

        private void OnEnemyDeath(EnemyBase obj)
        {
            var coin = _coinFactory.Create();
            coin.transform.position = GetRandomSpawnPos(obj.transform.position, RandomSpawnRange);
            OnCoinSpawn?.Invoke(coin);
        }

        private Vector3 GetRandomSpawnPos(Vector3 position, float range)
        {
            var randomPos = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
            return position + randomPos;
        }
    }
}