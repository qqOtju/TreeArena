using System;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character
{
    public class BasicBullet: Bullet
    {
        private Vector3 _pastPosition;
        private bool _outOfRange;

        public override void Init(int piercing, Action<Bullet,
                IHealth<EnemyBase>> onHealthHit, Action<Bullet> onHit,
            Action<Bullet> moveForward)
        {
            base.Init(piercing, onHealthHit, onHit, moveForward);
            _pastPosition = transform.position;
            _outOfRange = false;
        }
        
        private void FixedUpdate()
        {
            if(_outOfRange) return;
            var currentPos = BulletTr.position;
            var distance = Vector3.Distance(currentPos, _pastPosition);
            CurrenDistance -= distance;
            _pastPosition = currentPos;
            if (CurrenDistance <= 0 && !_outOfRange)
                _outOfRange = true;
            Move?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(gameObject.activeSelf == false)
                return;
            if (other.gameObject.CompareTag("Enemy"))
            {
                CurrentPiercing--;
                OnHealthHit?.Invoke(this, other.gameObject.GetComponent<IHealth<EnemyBase>>());
            }
            else if(other.gameObject.CompareTag("Wall"))
                OnHit?.Invoke(this);
        }
    }
}