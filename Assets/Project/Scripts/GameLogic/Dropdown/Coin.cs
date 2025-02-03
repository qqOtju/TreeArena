using System;
using Project.Scripts.Config;
using Project.Scripts.Module.System;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic.Dropdown
{
    [SelectionBase]
    public class Coin: MonoBehaviour
    {
        private const int MaxHits = 50;
        
        private readonly RaycastHit2D[] _hitsBuffer = new RaycastHit2D[MaxHits];
        
        private CoinSystem _coinSystem;
        private Transform _transform;
        private float _pickupRange;
        private int _value;
        
        public event Action<Coin> OnPlayerHit;

        [Inject]
        private void Construct(CoinSystem coinSystem)
        {
            _coinSystem = coinSystem;
        }
        
        public void Initialize(CoinStat config, int wave)
        {
            _pickupRange = config.PickupRange;
            config.MultiplyByWave(wave);
            _value = config.Value;
        }
        
        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            var size = Physics2D.CircleCastNonAlloc(_transform.position,
                _pickupRange, Vector2.zero, _hitsBuffer);
            for (var i = 0; i < size; i++)
            {
                if (!_hitsBuffer[i].collider.CompareTag("Player")) continue;
                PlayerHit();
                return;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _pickupRange);
        }
        
        private void PlayerHit()
        {
            OnPlayerHit?.Invoke(this);
            _coinSystem.CurrentGold += _value;
            Debug.Log("Player hit coin");
        }
    }
}