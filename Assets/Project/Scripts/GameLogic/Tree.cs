using Project.Scripts.Audio;
using Project.Scripts.Config;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.System;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic
{
    [SelectionBase]
    public class Tree: EntityBase<Tree>
    {
        [SerializeField] private WavesController _wavesController;
        [SerializeField] private AudioClip[] _damageSounds;
        
        private TreeUpgradeSystem _upgradeSystem;
        private AudioController _audioController;
        private bool _bonusHeal;
        private TreeStat _stat;
            
        [Inject]
        private void Construct(TreeUpgradeSystem upgradeSystem, AudioController audioController)
        {
            _upgradeSystem = upgradeSystem;
            _audioController = audioController;
        }

        private void Awake()
        {
            _upgradeSystem.OnTreeStatChanged += SetStat;
            _wavesController.OnWaveStart += OnWaveStart;
            _wavesController.OnWaveEnd += OnWaveEnd;
        }

        private void Start()
        {
            SetInitialHealth(_upgradeSystem.CurrentTreeStat.MaxHealth);
            _stat = _upgradeSystem.CurrentTreeStat;
        }

        private void Update()
        {
            if (CurrentHealth < MaxHealth && !_bonusHeal)
                Heal(_stat.Regen * Time.deltaTime);
            else if(CurrentHealth < MaxHealth && _bonusHeal)
                Heal(500 * Time.deltaTime);
        }

        private void OnDestroy()
        {
            _upgradeSystem.OnTreeStatChanged -= SetStat;
            _wavesController.OnWaveStart -= OnWaveStart;
            _wavesController.OnWaveEnd -= OnWaveEnd;
        }

        private void OnWaveEnd(int obj)
        {
            _bonusHeal = true;
        }

        private void OnWaveStart(int obj)
        {
            _bonusHeal = false;
        }

        private void SetStat(TreeStat obj)
        {
            if(obj.MaxHealth != _stat.MaxHealth)
                IncreaseMaxHealth(obj.MaxHealth - _stat.MaxHealth);
            _stat = obj;
            Debug.Log($"Tree stat changed: {_stat.MaxHealth} {_stat.Regen} {_stat.Armor} {_stat.Resistance}");
        }

        public override void TakeDamage(float dmg)
        {
            PlaySound();
            var chance = Random.Range(0, 100);
            if (chance < _stat.Resistance)
                dmg = 1;
            dmg -= _stat.Armor;
            if(dmg <= 0)
                dmg = 1;
            CurrentHealth -= dmg;
        }

        private void PlaySound()
        {
            var clip = _damageSounds[Random.Range(0, _damageSounds.Length)];
            _audioController.PlaySFX(clip);
        }
    }
}