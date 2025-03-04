using Project.Scripts.Config;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public class AOEAttack: WispAttack
    {
        private const float BaseBulletSpeed = 5f;
        
        private AoeAttackStat _aoeAttackStat;
        
        public AOEAttack(BulletFactory bulletFactory, Transform muzzle, 
            AoeAttackStat aoeAttackStat): 
            base(bulletFactory, muzzle)
        {
            _aoeAttackStat = aoeAttackStat;
        }
        
        protected override void Execute()
        {
            _bulletFactory.SetActions(OnHealthHit, OnWallHit, BulletMoveForward, 
                _muzzle, _aoeAttackStat.Piercing);
            var angle = 7f;
            var bulletCount = _aoeAttackStat.BulletCount;
            var startAngle = _muzzle.localRotation.eulerAngles.z - angle * bulletCount / 2;
            for (int i = 0; i < bulletCount; i++)
            {
                var bullet = _bulletFactory.Create();
                bullet.transform.rotation = Quaternion.Euler(
                    new Vector3(0,0, startAngle + i * angle));
            }
            RaiseOnAttack();
        }
        
        protected override float GetDamage() => _aoeAttackStat.Damage;

        protected override float GetAttackSpeed() => _aoeAttackStat.AttackSpeed;

        protected override int GetPiercing() => _aoeAttackStat.Piercing;
        protected override float GetBulletSpeed() => BaseBulletSpeed;
        
        public void SetStat(AoeAttackStat aoeAttackStat)
        {
            _aoeAttackStat = aoeAttackStat;
        } 
    }
}