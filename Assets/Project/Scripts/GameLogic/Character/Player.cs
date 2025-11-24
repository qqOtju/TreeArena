using Project.Scripts.UI.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player: MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _animationSprites;
        [SerializeField] private Transform _view;
        [SerializeField] private UIJoystick _joystick;
        [Title("Values")]
        [ShowInInspector] private const float DefaultSpeed = 4f;
        [ShowInInspector] private const float AnimationTime = 0.25f;
        
        private int _maxHealth;
        private float _currentHealth;
        private BasicMovement _basicMovement;
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private float _animationTimer;
        private int _animationIndex;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _basicMovement = new BasicMovement(_rb);
        }

        private void Update()
        {
#if UNITY_ANDROID ||  UNITY_IOS
            _moveInput = _joystick.GetInput();
#else 
            _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif
            AnimationCycle();
        }

        private void FixedUpdate()
        {
            _basicMovement.Move(_moveInput, DefaultSpeed);
        }

        private void AnimationCycle()
        {
            var scale = _view.localScale;
            if (_moveInput.x > 0)
                _view.localScale = new Vector3(1, scale.y, scale.z);
            else if (_moveInput.x < 0)
                _view.localScale = new Vector3(-1, scale.y, scale.z);
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
    }
}