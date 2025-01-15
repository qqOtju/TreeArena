using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public abstract class WispAttack
    {
        protected readonly BulletFactory _bulletFactory;
        protected readonly Transform _muzzle;
        
        private float _time;
        
        protected float Damage => GetDamage();
        protected float AttackSpeed => GetAttackSpeed();
        protected int Piercing => GetPiercing();
        protected float BulletSpeed => GetBulletSpeed();
        
        public WispAttack(BulletFactory bulletFactory, Transform muzzle)
        {
            _bulletFactory = bulletFactory;
            _muzzle = muzzle;
        }

        public virtual void Update()
        {
            _time += Time.deltaTime;
            if (_time >= AttackSpeed)
            {
                _time = 0;
                Execute();
            }
        }
        
        protected virtual void Execute()
        {
            _bulletFactory.SetActions(OnHealthHit, OnWallHit, BulletMoveForward, 
                _muzzle, Piercing);
            var bullet = _bulletFactory.Create();
            bullet.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, _muzzle.localRotation.eulerAngles.z));
        }

        protected virtual void OnHealthHit(Bullet bullet, IHealth<EnemyBase> health)
        {
            health.TakeDamage(Damage);
            if(bullet.CurrentPiercing <= 0)
                _bulletFactory.Release(bullet);
        }
        
        protected virtual void OnWallHit(Bullet bullet)
        {
            _bulletFactory.Release(bullet);
        }
        
        protected virtual void BulletMoveForward(Bullet bullet)
        {
            if (bullet.CurrenDistance <= 0)
            {
                _bulletFactory.Release(bullet);
                return;
            }
            var tr = bullet.gameObject.transform;
            var rb = bullet.Rb;
            var moveForce = tr.position + tr.right / 5;
            rb.MovePosition(moveForce);
        }
        
        protected abstract float GetDamage();
        protected abstract float GetAttackSpeed();
        protected abstract int GetPiercing();
        protected abstract float GetBulletSpeed();
    }
}