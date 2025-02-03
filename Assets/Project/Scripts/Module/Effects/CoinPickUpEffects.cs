using Project.Scripts.Audio;
using Project.Scripts.GameLogic.Dropdown;
using Project.Scripts.Module.Spawner;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Effects
{
    public class CoinPickUpEffects: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _pickUpEffect;
        [SerializeField] private AudioClip[] _pickUpSounds;
        [SerializeField] private CoinSpawner _coinSpawner;

        private AudioController _audioController;
        private ParticleSystem _effect;

        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }
        
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
            _effect.Clear();
            Destroy(_effect.gameObject);
        }

        private void OnCoinSpawn(Coin obj)
        {
            obj.OnPlayerHit += OnPlayerHit;
        }

        private void OnPlayerHit(Coin obj)
        {
            _effect.transform.position = obj.transform.position;
            _effect.Play();
            var randomClip = _pickUpSounds[Random.Range(0, _pickUpSounds.Length)];
            _audioController.PlaySFX(randomClip);
            obj.OnPlayerHit -= OnPlayerHit;
        }
    }
}