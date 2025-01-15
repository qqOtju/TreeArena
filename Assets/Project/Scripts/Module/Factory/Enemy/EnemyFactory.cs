using Project.Scripts.Config;
using Project.Scripts.DesignPattern.Factory;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory.Enemy
{    
    public abstract class EnemyFactory: ObjectFactory<EnemyBase>
    {
        private const int BasePoolSize = 10;
     
        private readonly EnemyConfig _enemyConfig;
        private readonly DiContainer _diContainer;
        private readonly Transform _target;
        
        private int _currentWaveIndex;
        
        public EnemyFactory(EnemyBase prefab, Transform container, 
            EnemyConfig enemyConfig, DiContainer diContainer, Transform target) : base(prefab, container)
        {
            Pool.Initialize(BasePoolSize);
            _enemyConfig = enemyConfig;
            _diContainer = diContainer;
            _target = target;
        }

        private void OnHealthChange(OnHealthChangeArgs<EnemyBase> obj)
        {
            if(obj.Type == HeathChangeType.Death)
                Release(obj.Object);
        }

        public override EnemyBase Create()
        {
            var enemy = Pool.Get();
            var stat = _enemyConfig.EnemyStat;
            stat.MultiplyByWave(_currentWaveIndex);
            enemy.Initialize(stat, _target);
            _diContainer.Inject(enemy);
            enemy.OnHealthChange += OnHealthChange;
            return enemy;
        }

        public override void Release(EnemyBase obj)
        {
            obj.OnHealthChange -= OnHealthChange;
            Pool.Release(obj);
        }
        
        public void SetWaveIndex(int waveIndex)
        {
            _currentWaveIndex = waveIndex;
        }
    }
}