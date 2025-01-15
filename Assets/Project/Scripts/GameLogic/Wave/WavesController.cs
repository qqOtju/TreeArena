using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Config;
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
        private int _currentWaveIndex;
        private bool _allEnemiesSpawned;
        
        public event Action<int> OnWaveStart;
        public event Action<int> OnWaveEnd;
        
        [Inject]
        private void Construct(BasicEnemyFactory basicEnemyFactory, 
            FastEnemyFactory fastEnemyFactory, TankEnemyFactory tankEnemyFactory)
        {
            _factories = new List<EnemyFactory>
            {
                basicEnemyFactory,
                fastEnemyFactory,
                tankEnemyFactory
            };
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
            else
                wave = _wavesConfig[Random.Range(0, _wavesConfig.Length)];
            OnWaveStart?.Invoke(_currentWaveIndex);
            Debug.Log($"Wave {_currentWaveIndex} started");
            _currentWaveIndex++;
            StartCoroutine(StartWave(wave));
        }

        private IEnumerator StartWave(WaveConfig wave)
        {
            _allEnemiesSpawned = false;
            foreach (var spawn in wave.Spawns)
            {
                yield return new WaitForSeconds(spawn.WaveDelay);
                for (var i = 0; i < spawn.EnemyCount; i++)
                {
                    yield return new WaitForSeconds(spawn.SpawnDelay);
                    _enemySpawner.SpawnEnemy(spawn.SpawnPoint, spawn.EnemyType);
                }
            }
            _allEnemiesSpawned = true;
        }

        private void OnAllEnemiesDead()
        {
            if (_allEnemiesSpawned)
            {
                OnWaveEnd?.Invoke(_currentWaveIndex - 1);
                Debug.Log($"Wave {_currentWaveIndex - 1} ended");
            }
        }
        
        private void OnOnWaveEnd(int obj)
        {
            foreach (var factory in _factories)
                factory.SetWaveIndex(obj);
        }
    }
}