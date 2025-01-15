using Project.Scripts.Config;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic.Character
{
    public class Wisp: MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private Player _player;
        [SerializeField] private float _avoidanceDistance;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _muzzle;
        [Title("Visuals")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [PreviewField(50, ObjectFieldAlignment.Left)]
        [SerializeField] private Sprite[] _animationSprites;
        [SerializeField] private Transform _visual;
        [Title("References")]
        [SerializeField] private WavesController _wavesController;
        [Title("Values")]
        [ShowInInspector] private const float AnimationTime = 0.4f;
        
        private const float AdditionalDistance = 0.5f;
        private const float RotationSpeed = 2f;
        
        private WeaponUpgradeSystem _weaponUpgradeSystem;
        private IHealth<EnemyBase> _currentTarget;
        private float _muzzleRotationVelocity;
        private BulletFactory _bulletFactory;
        private float _rotationVelocity;
        private Transform _transform;
        private SingleAttack _singleAttack;
        private AOEAttack _aoeAttack;
        private WispAttack _currentAttack;
        private bool _isAttacking;
        private Rigidbody2D _rb;
        private float _time;
        private Vector2 _moveDirection;
        private float _animationTimer;
        private int _animationIndex;
        private bool _animationDirection = true;

        [Inject]
        private void Construct(BulletFactory bulletFactory, 
            WeaponUpgradeSystem weaponUpgradeSystem)
        {
            _bulletFactory = bulletFactory;    
            _weaponUpgradeSystem = weaponUpgradeSystem;
        }

        private void Awake()
        {
            _weaponUpgradeSystem.OnSingleAttackStatChanged += OnSingleAttackStatChanged;
            _weaponUpgradeSystem.OnAoeAttackStatChanged += OnAoeAttackStatChanged;
            _wavesController.OnWaveStart += OnWaveStart;
            _wavesController.OnWaveEnd += OnWaveEnd;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _transform = transform;
            _singleAttack = new SingleAttack(_bulletFactory, _muzzle, 
                _weaponUpgradeSystem.CurrentSingleAttackStat, () => _currentTarget);
            _aoeAttack = new AOEAttack(_bulletFactory, _muzzle, 
                _weaponUpgradeSystem.CurrentAoeAttackStat);
            _currentAttack = _singleAttack;
        }

        private void FixedUpdate()
        {
            Rotate();
            Move();
            RotateMuzzle();
        }
        
        private void Update()
        {
            if (_isAttacking)
                _currentAttack.Update();
            if (_visual != null)
                _visual.rotation = Quaternion.identity;
            if(Input.GetKeyDown(KeyCode.Z))
                _currentAttack = _singleAttack;
            else if(Input.GetKeyDown(KeyCode.X))
                _currentAttack = _aoeAttack;
            if(_moveDirection.x > 0)
                _visual.localScale = new Vector3(1, 1, 1);
            else if(_moveDirection.x < 0)
                _visual.localScale = new Vector3(-1, 1, 1);
            AnimationCycle();
        }

        private void OnWaveStart(int obj)
        {
            _isAttacking = true;
        }

        private void OnWaveEnd(int obj)
        {
            _isAttacking = false;
        }

        private void OnSingleAttackStatChanged(SingleAttackStat obj)
        {
            _singleAttack.SetStat(obj);
        }
        
        private void OnAoeAttackStatChanged(AoeAttackStat obj)
        {
            _aoeAttack.SetStat(obj);
        }
        
        private void Move()
        {
            var startPosition = transform.position;
            var endPosition = _player.gameObject.transform.position;
            var distance = Vector3.Distance(startPosition, endPosition);
            if (!(distance > _avoidanceDistance + AdditionalDistance)) return;
            var moveForce = _transform.position + (_transform.right / 50 * _moveSpeed);
            _moveDirection = (Vector2)moveForce - _rb.position;
            _rb.MovePosition(moveForce);
        }

        private void Rotate()
        {
            var direction = (Vector2)_player.transform.position - _rb.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var currentAngle = _rb.rotation;
            _rb.rotation = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed * Time.fixedDeltaTime);
        }

        private void RotateMuzzle()
        {
            if (_currentTarget == null) return;
            var direction = _currentTarget.Object.transform.position - _muzzle.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _muzzle.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }

        private void AnimationCycle()
        {
            _animationTimer += Time.deltaTime;
            if (_animationTimer >= AnimationTime)
            {
                _animationTimer = 0;
                if (_animationDirection)
                    _animationIndex++;
                else
                    _animationIndex--;
                Debug.Log($"Animation index: {_animationIndex}");
                if (_animationIndex >= _animationSprites.Length - 1)
                    _animationDirection = false;
                else if (_animationIndex <= 0)
                    _animationDirection = true;
                _spriteRenderer.sprite = _animationSprites[_animationIndex];
            }
        }
        
        public void SetTarget(IHealth<EnemyBase> target)
        {
            _currentTarget = target;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _avoidanceDistance);
        }
    }
}