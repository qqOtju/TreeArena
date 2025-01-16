using Project.Scripts.Audio;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.Module.Spawner;
using UnityEngine;
using Zenject;
using Tree = Project.Scripts.GameLogic.Tree;

namespace Project.Scripts.Module.Effects
{
    public class EnemyDamageEffect: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _damageEffect;
        [SerializeField] private ParticleSystem _dealDamageEffect;
        [SerializeField] private AudioClip[] _damageSounds;
        [SerializeField] private EnemySpawner _enemySpawner;

        private ParticleSystem _effect;
        private ParticleSystem _dealEffect;
        private AudioController _audioController;

        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }
        
        private void Awake()
        {
            _enemySpawner.OnEnemySpawn += OnEnemySpawn;
            _enemySpawner.OnEnemyDeath += OnEnemyDeath;
        }

        private void Start()
        {
            _effect = Instantiate(_damageEffect, Vector3.zero, Quaternion.identity, transform);
            _dealEffect = Instantiate(_dealDamageEffect, Vector3.zero, Quaternion.identity, transform);
        }

        private void OnDestroy()
        {
            _enemySpawner.OnEnemySpawn -= OnEnemySpawn;
            _enemySpawner.OnEnemyDeath -= OnEnemyDeath;
        }

        private void OnEnemySpawn(EnemyBase obj)
        {
            obj.OnHealthChange += OnHealthChange;
            obj.OnDealDamage += OnDealDamage;
        }

        private void OnEnemyDeath(EnemyBase obj)
        {
            obj.OnHealthChange -= OnHealthChange;
            obj.OnDealDamage -= OnDealDamage;
        }

        private void OnHealthChange(OnHealthChangeArgs<EnemyBase> obj)
        {
            if (obj.Type == HeathChangeType.Heal) return;
            _effect.transform.position = obj.Object.transform.position;
            var effectMain = _effect.main;
            effectMain.startColor = obj.Object.Color;
            var randomClip = _damageSounds[Random.Range(0, _damageSounds.Length)];
            _audioController.PlaySFX(randomClip);
            _effect.Play();
        }

        private void OnDealDamage(EnemyBase enemy, IHealth<Tree> tree)
        {
            var range = 0.5f;
            var offset = new Vector3(0, -3f);
            _dealEffect.transform.position = tree.Object.transform.position + offset +
                                             new Vector3(Random.Range(-range, range), Random.Range(range, range), 0);
            var effectMain = _dealEffect.main;
            effectMain.startColor = enemy.Color;
            _dealEffect.Play();
        }
    }
}