using Project.Scripts.GameLogic.Dropdown;
using Project.Scripts.Module.Spawner;
using UnityEngine;

namespace Project.Scripts.Module.Effects
{
    public class CoinPickUpEffects: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _pickUpEffect;
        [SerializeField] private CoinSpawner _coinSpawner;
        
        private ParticleSystem _effect;
        
        private void Awake()
        {
            _coinSpawner.OnCoinSpawn += OnCoinSpawn;
        }

        private void Start()
        {
            _effect = Instantiate(_pickUpEffect, Vector3.zero, Quaternion.identity, transform);
        }
        
        private void OnDestroy()
        {
            _coinSpawner.OnCoinSpawn -= OnCoinSpawn;
        }

        private void OnCoinSpawn(Coin obj)
        {
            obj.OnPlayerHit += OnPlayerHit;
        }

        private void OnPlayerHit(Coin obj)
        {
            _effect.transform.position = obj.transform.position;
            _effect.Play();
            obj.OnPlayerHit -= OnPlayerHit;
        }
    }
}