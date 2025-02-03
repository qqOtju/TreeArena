using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Config;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Factory.Enemy;
using Project.Scripts.Module.Spawner;
using Project.Scripts.UI.Game;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.Wave
{
    public class WavesController: MonoBehaviour
    {
        [SerializeField] private UIWeaponUpgradeMenu _uiWeaponUpgradeMenu;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private WaveConfig[] _wavesConfig;

        private List<EnemyFactory> _factories;
        private CoinFactory _coinFactory;
        private int _currentWaveIndex;
        private bool _allEnemiesSpawned;
        private bool _infinityWaves;
        
        public event Action<int> OnWaveStart;
        public event Action<int> OnWaveEnd;
        public event Action OnAllWavesEnd;
        
        [Inject]
        private void Construct(BasicEnemyFactory basicEnemyFactory, 
            FastEnemyFactory fastEnemyFactory, TankEnemyFactory tankEnemyFactory, CoinFactory coinFactory)
        {
            _factories = new List<EnemyFactory>
            {
                basicEnemyFactory,
                fastEnemyFactory,
                tankEnemyFactory
            };
            _coinFactory = coinFactory;
        }
        
        private void Awake()
        {
            _enemySpawner.OnAllEnemiesDead += OnAllEnemiesDead;
            _uiWeaponUpgradeMenu.OnClose += StartNextWave;
            OnWaveEnd += OnOnWaveEnd;
        }
        
        private void Start()
        {
            StartNextWave();
        }

        private void OnDestroy()
        {
            _enemySpawner.OnAllEnemiesDead -= OnAllEnemiesDead;
            OnWaveEnd -= OnOnWaveEnd;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                StartNextWave();
        }

        private void StartNextWave()
        {
            WaveConfig wave;
            if(_currentWaveIndex < _wavesConfig.Length)
                wave = _wavesConfig[_currentWaveIndex];
            else if(_infinityWaves)
                wave = _wavesConfig[Random.Range(0, _wavesConfig.Length)];
            else
                return;
            OnWaveStart?.Invoke(_currentWaveIndex);
            Debug.Log($"Wave {_currentWaveIndex} started");
            _currentWaveIndex++;
            StartWave(wave);
        }

        private void StartWave(WaveConfig wave)
        {
            _allEnemiesSpawned = false;
            foreach (var spawn in wave.Spawns)
            {
                StartCoroutine(StartWave(spawn));
            }
            _allEnemiesSpawned = true;
        }

        private IEnumerator StartWave(WaveContent wave)
        {
            yield return new WaitForSeconds(wave.WaveDelay);
            for (var i = 0; i < wave.EnemyCount; i++)
            {
                yield return new WaitForSeconds(wave.SpawnDelay);
                _enemySpawner.SpawnEnemy(wave.SpawnPoint, wave.EnemyType);
            }
        }//

        private void OnAllEnemiesDead()
        {
            if (!_allEnemiesSpawned) return;
            if(_currentWaveIndex >= _wavesConfig.Length && !_infinityWaves)
            {
                OnAllWavesEnd?.Invoke();
                Debug.Log("<color=green>All waves ended</color>");
            }
            else
            {
                OnWaveEnd?.Invoke(_currentWaveIndex - 1);
                Debug.Log($"Wave {_currentWaveIndex - 1} ended");
            }
        }
        
        private void OnOnWaveEnd(int obj)
        {
            foreach (var factory in _factories)
                factory.SetWaveIndex(obj);
            _coinFactory.SetWaveIndex(obj);
        }
        
        public void StartInfinityWaves()
        {
            _infinityWaves = true;
            OnWaveEnd?.Invoke(_currentWaveIndex - 1);
            Debug.Log($"Wave {_currentWaveIndex - 1} ended");
        }
    }
}