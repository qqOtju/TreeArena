using System;
using Project.Scripts.Config;
using Project.Scripts.GameLogic.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBase: EntityBase<EnemyBase>
    {
        [Title("Visuals")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [PreviewField(50, ObjectFieldAlignment.Left)]
        [SerializeField] private Sprite[] _animationSprites;
        [SerializeField] private Transform _visual;
        [SerializeField] private Color _color;
        [Title("Values")]
        [ShowInInspector] private const float AnimationTime = 0.25f;
    
        private const string TreeTag = "Tree";
        
        private BasicMovement _basicMovement;
        private Rigidbody2D _rb;
        private EnemyStat _enemyStat;
        private Vector2 _moveInput;
        private float _attackTimer;
        private float _animationTimer;
        private int _animationIndex;
        private Vector3 _targetPosition;
        
        public Color Color => _color;
        
        public event Action<EnemyBase, IHealth<Tree>> OnDealDamage;
        
        public void Initialize(EnemyStat enemyStat, Transform target)
        {
            _enemyStat = enemyStat;
            SetInitialHealth(_enemyStat.MaxHealth);
            Debug.Log($"Enemy initialized. Health: {_enemyStat.MaxHealth}, " +
                      $"Damage: {_enemyStat.Damage}, " +
                      $"MoveSpeed: {_enemyStat.MoveSpeed}, " +
                      $"AttackSpeed: {_enemyStat.AttackSpeed}, " +
                      $"AttackRange: {_enemyStat.AttackRange}");
            var range = 0.8f;
            _targetPosition = target.position + new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
        }
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _basicMovement = new BasicMovement(_rb);
            var range = 0.5f;
        }

        private void Update()
        {
            CalculateMoveInput();
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _enemyStat.AttackSpeed)
            {
                TryToAttackTree();
                _attackTimer = 0;
            }
            AnimationCycle();
        }
        
        private void FixedUpdate()
        {
            _basicMovement.Move(_moveInput, _enemyStat.MoveSpeed);
        }
        
        private void AnimationCycle()
        {
            var scale = _visual.localScale;
            if (_moveInput.x > 0)
                _visual.localScale = new Vector3(1, scale.y, scale.z);
            else if (_moveInput.x < 0)
                _visual.localScale = new Vector3(-1, scale.y, scale.z);
            _animationTimer += Time.deltaTime;
            if (_animationTimer >= AnimationTime)
            {
                _animationTimer = 0;
                _animationIndex++;
                if (_animationIndex >= _animationSprites.Length)
                    _animationIndex = 0;
                _spriteRenderer.sprite = _animationSprites[_animationIndex];
            }
        }
        
        private void CalculateMoveInput()
        {
            var hits = GetNearbyObjects(_enemyStat.AttackRange);
            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("Tree"))
                {
                    _moveInput = Vector2.zero;
                    return;
                }
            }
            _moveInput = (_targetPosition - transform.position).normalized;
        }

        private void TryToAttackTree()
        {
            var hits = GetNearbyObjects(_enemyStat.AttackRange);
            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag(TreeTag))
                    Attack(hit.transform.GetComponent<IHealth<Tree>>());
            }   
        }
        
        private RaycastHit2D[] GetNearbyObjects(float range)
        {
            return Physics2D.CircleCastAll(transform.position, range, Vector2.zero);
        }

        private void Attack(IHealth<Tree> health)
        {
            health.TakeDamage(_enemyStat.Damage);
            OnDealDamage?.Invoke(this, health);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _enemyStat.AttackRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_targetPosition, 0.1f);
        }
    }
}