using Project.Scripts.Module.System;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic
{
    public class DebugController: MonoBehaviour
    {
        private TreeUpgradeSystem _treeUpgradeSystem;

        [Inject]
        private void Construct(TreeUpgradeSystem treeUpgradeSystem)
        {
            _treeUpgradeSystem = treeUpgradeSystem;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
                _treeUpgradeSystem.UpgradeMaxHealth();
            if (Input.GetKeyDown(KeyCode.F))
                _treeUpgradeSystem.UpgradeRegen();
            if (Input.GetKeyDown(KeyCode.G))
                _treeUpgradeSystem.UpgradeArmor();
            if (Input.GetKeyDown(KeyCode.H))
                _treeUpgradeSystem.UpgradeResistance();
        }
#endif
    }
}