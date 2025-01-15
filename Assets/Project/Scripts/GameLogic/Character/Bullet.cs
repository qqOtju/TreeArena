using System;
using Project.Scripts.Config;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class Bullet: MonoBehaviour
    {
        protected const float BaseDistance = 50f;
        
        protected Transform BulletTr;
        protected Action<Bullet> Move;
        protected Action<Bullet, IHealth<EnemyBase>> OnHealthHit;
        protected Action<Bullet> OnHit;

        public int CurrentPiercing { get; protected set; }
        public float CurrenDistance { get; protected set; }
        public Rigidbody2D Rb { get; private set; }

        protected virtual void Awake()
        {
            BulletTr = transform;
        }
        
        protected virtual void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        public virtual void Init(int piercing, Action<Bullet, 
                IHealth<EnemyBase>> onHealthHit, Action<Bullet> onHit,
            Action<Bullet> moveForward)
        {
            OnHealthHit = onHealthHit;
            OnHit = onHit;
            Move = moveForward;
            CurrentPiercing = piercing;
            CurrenDistance = BaseDistance;
        }
    }
}