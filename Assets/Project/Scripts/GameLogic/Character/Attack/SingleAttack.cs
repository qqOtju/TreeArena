using System;
using System.Collections.Generic;
using Project.Scripts.Config;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public class SingleAttack: WispAttack
    {
        private const float BaseBulletSpeed = 5f;
        private const int BasePiercing = 1;
        
        private readonly Dictionary<Bullet, Transform> _bullets = new ();
        private readonly Func<IHealth<EnemyBase>> _getCurrentTarget;
        
        private SingleAttackStat _singleAttackStat;
        private float _rotationVelocity;
        
        public SingleAttack(BulletFactory bulletFactory, Transform muzzle, 
           SingleAttackStat singleAttackStat, Func<IHealth<EnemyBase>> getCurrentTarget): 
            base(bulletFactory, muzzle)
        {
            _getCurrentTarget = getCurrentTarget;
            _singleAttackStat = singleAttackStat;
        }
        
        protected override void Execute()
        {
            _bulletFactory.SetActions(OnHealthHit, OnWallHit, BulletMoveForward, 
                _muzzle, Piercing);
            var bullet = _bulletFactory.Create();
            if (!_bullets.ContainsKey(bullet))
            {
                var enemyTransform = _getCurrentTarget()?.Object.transform;
                _bullets.Add(bullet, enemyTransform);
            }
            else
            {
                var enemyTransform = _getCurrentTarget()?.Object.transform;
                _bullets[bullet] = enemyTransform;
            }
            bullet.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, _muzzle.localRotation.eulerAngles.z));
        }

        protected override void OnHealthHit(Bullet bullet, IHealth<EnemyBase> health)
        {
            var random = UnityEngine.Random.Range(0, 100);
            if (random <= _singleAttackStat.CriticalChance)
            {
                health.TakeDamage(Damage * _singleAttackStat.CriticalDamage/100);
            }
            else
            {
                health.TakeDamage(Damage);
            }
            if(bullet.CurrentPiercing <= 0)
                _bulletFactory.Release(bullet);
        }

        protected override void BulletMoveForward(Bullet bullet)
        {
            base.BulletMoveForward(bullet);
            if(_bullets.ContainsKey(bullet) && _bullets[bullet] != null)
            {
                var enemyTransform = _bullets[bullet].position; 
                var direction = enemyTransform - bullet.transform.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var rotation = Mathf.SmoothDampAngle(bullet.transform.rotation.eulerAngles.z, angle,
                    ref _rotationVelocity,
                    0.1f);
                bullet.transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
        }

        protected override float GetDamage() => _singleAttackStat.Damage;
        protected override float GetAttackSpeed() => _singleAttackStat.AttackSpeed;
        protected override int GetPiercing() => BasePiercing;
        protected override float GetBulletSpeed() => BaseBulletSpeed;
        
        public void SetStat(SingleAttackStat singleAttackStat)
        {
            _singleAttackStat = singleAttackStat;
        }
    }
}