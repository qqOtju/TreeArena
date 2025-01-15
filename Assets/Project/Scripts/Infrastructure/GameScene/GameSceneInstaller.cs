using Project.Scripts.Config;
using Project.Scripts.GameLogic.Character;
using Project.Scripts.GameLogic.Dropdown;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Factory.Enemy;
using Project.Scripts.Module.System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Tree = Project.Scripts.GameLogic.Tree;

namespace Project.Scripts.Infrastructure.GameScene
{
    public class GameSceneInstaller: MonoInstaller
    {
        [SerializeField] private Transform _enemyTarget;
        [Title("Basic Enemy")]
        [AssetsOnly]
        [SerializeField] private EnemyBase _basicEnemyPrefab;
        [SerializeField] private EnemyConfig _basicEnemyConfig;
        [SerializeField] private Transform _basicEnemyContainer;
        [Title("Fast Enemy")]
        [AssetsOnly]
        [SerializeField] private EnemyBase _fastEnemyPrefab;
        [SerializeField] private EnemyConfig _fastEnemyConfig;
        [SerializeField] private Transform _fastEnemyContainer;
        [Title("Tank Enemy")]
        [AssetsOnly]
        [SerializeField] private EnemyBase _tankEnemyPrefab;
        [SerializeField] private EnemyConfig _tankEnemyConfig;
        [SerializeField] private Transform _tankEnemyContainer;
        [Title("Tree")]
        [SerializeField] private Tree _tree;
        [SerializeField] private TreeConfig _treeConfig;
        [Title("Coin")]
        [AssetsOnly]
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private CoinConfig _coinConfig;
        [SerializeField] private Transform _coinContainer;
        [Title("Bullet")]
        [AssetsOnly]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletContainer;
        [Title("Weapon")]
        [SerializeField] private SingleAttackConfig _singleAttackConfig;
        [SerializeField] private AoeAttackConfig _aoeAttackConfig;

        private const int InitialGold = 100;

        public override void InstallBindings()
        {
            BindTreeUpgradeSystem();
            BindWeaponUpgradeSystem();
            BindDiContainer();
            BindFactories();
            BindTree();
            BindCoinFactory();
            BindCoinSystem();
        }

        private void BindWeaponUpgradeSystem()
        {
            var weaponUpgradeSystem = new WeaponUpgradeSystem(_singleAttackConfig, _aoeAttackConfig);
            Container.Bind<WeaponUpgradeSystem>().FromInstance(weaponUpgradeSystem).AsSingle();
        }

        private void BindTreeUpgradeSystem()
        {
            var treeUpgradeSystem = new TreeUpgradeSystem(_treeConfig);
            Container.Bind<TreeUpgradeSystem>().FromInstance(treeUpgradeSystem).AsSingle();
        }

        private void BindDiContainer()
        {
            Container.Bind<DiContainer>().AsSingle();
        }

        private void BindTree()
        {
            Container.Inject(_tree);
            Container.Bind<Tree>().FromInstance(_tree).AsSingle();
        }

        private void BindCoinFactory()
        {
            var coinFactory = new CoinFactory(_coinPrefab, _coinContainer, _coinConfig, Container);
            Container.Bind<CoinFactory>().FromInstance(coinFactory).AsSingle();
        }

        private void BindCoinSystem()
        {
            Container.Bind<CoinSystem>().AsSingle().WithArguments(InitialGold);
            BindBulletFactory();
        }

        private void BindBulletFactory()
        {
            var bulletFactory = new BulletFactory(_bulletPrefab, _bulletContainer, Container);
            Container.Bind<BulletFactory>().FromInstance(bulletFactory).AsSingle();
        }

        private void BindFactories()
        {
            var fastEnemyFactory = new FastEnemyFactory(_fastEnemyPrefab, _fastEnemyContainer, _fastEnemyConfig, Container, _enemyTarget);
            Container.Bind<FastEnemyFactory>().FromInstance(fastEnemyFactory).AsSingle();
            var tankEnemyFactory = new TankEnemyFactory(_tankEnemyPrefab, _tankEnemyContainer, _tankEnemyConfig, Container, _enemyTarget);
            Container.Bind<TankEnemyFactory>().FromInstance(tankEnemyFactory).AsSingle();
            var basicEnemyFactory = new BasicEnemyFactory(_basicEnemyPrefab, _basicEnemyContainer, _basicEnemyConfig, Container, _enemyTarget);
            Container.Bind<BasicEnemyFactory>().FromInstance(basicEnemyFactory).AsSingle();
        }
    }
}