using Project.Scripts.Config;
using Project.Scripts.GameLogic.Enemy;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory.Enemy
{
    public class TankEnemyFactory: EnemyFactory
    {
        public TankEnemyFactory(EnemyBase prefab, Transform container,
            EnemyConfig enemyConfig, DiContainer diContainer, Transform target)
            : base(prefab, container, enemyConfig, diContainer, target)
        {
        }
    }
}