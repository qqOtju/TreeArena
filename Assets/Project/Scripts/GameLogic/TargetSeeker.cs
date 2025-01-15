using System.Linq;
using Project.Scripts.GameLogic.Character;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Entity;
using Project.Scripts.Module.Spawner;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class TargetSeeker: MonoBehaviour
    {
        [SerializeField] private Wisp _wisp;
        [SerializeField] private EnemySpawner _enemySpawner;
        
        private IHealth<EnemyBase> _currentTarget;
        private Transform _muzzlePoint;
        private int _updateIteration;
        
        private void Awake()
        {
            _enemySpawner.OnEnemyDeath += OnEnemyDeath;
        }

        private void OnDestroy()
        {
            _enemySpawner.OnEnemyDeath -= OnEnemyDeath;
        }

        private void OnEnemyDeath(EnemyBase obj)
        {
            if (obj == _currentTarget) SetTarget();
        }

        private void Update()
        {
            if (_updateIteration == 10)
            {
                _updateIteration = 0;
                SetTarget();
            }
            _updateIteration++;
        }

        private void SetTarget()
        {
            var list = _enemySpawner.GetActiveEnemies().ToArray();
            var target = FindTarget(list);
            if (target == _currentTarget) return;
            _currentTarget = target;
            _wisp.SetTarget(_currentTarget);
            Debug.Log($"New target: {_currentTarget?.Object.name}");
        }
        
        private IHealth<EnemyBase> FindTarget(IHealth<EnemyBase>[] targets)
        {
            if (targets.Length <= 0) return null;
            var playerPos = _wisp.gameObject.transform.position;
            var distance = Vector2.Distance(targets[0].Object.transform.position, playerPos);
            var enemy = targets[0];
            foreach (var target in targets)
            {
                var localDis = Vector2.Distance(target.Object.transform.position, playerPos);
                if (localDis < distance)
                {
                    distance = localDis;
                    enemy = target;
                }
            }
            return enemy;
        }
    }
}