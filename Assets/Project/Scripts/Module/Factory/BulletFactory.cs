using System;
using Project.Scripts.Config;
using Project.Scripts.DesignPattern.Factory;
using Project.Scripts.GameLogic.Character;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory
{
    public class BulletFactory: ObjectFactory<Bullet>
    {
        private const int BasePoolSize = 10;
        
        private readonly DiContainer _diContainer;
        
        private Action<Bullet, IHealth<EnemyBase>> _onHealthHit;
        private Action<Bullet> _onHit;
        private Action<Bullet> _moveForward;
        private int _piercing;
        private Transform _muzzle;

        public BulletFactory(Bullet prefab, Transform container, 
            DiContainer diContainer) : base(prefab, container)
        {
            Pool.Initialize(BasePoolSize);
            _diContainer = diContainer;
        }

        public void SetActions(Action<Bullet, IHealth<EnemyBase>> onHealthHit, 
            Action<Bullet> onWallHit, Action<Bullet> moveForward, Transform muzzle, int piercing)
        {
            _onHealthHit = onHealthHit;
            _onHit = onWallHit;
            _moveForward = moveForward;
            _muzzle = muzzle;
            _piercing = piercing;
        }

        public override Bullet Create()
        {
            var bullet = Pool.Get();
            bullet.transform.position = _muzzle.position;
            bullet.Init(_piercing, _onHealthHit, _onHit, _moveForward);
            _diContainer.Inject(bullet);
            return bullet;
        }

        public override void Release(Bullet obj)
        {
            Pool.Release(obj);
        }
    }
}