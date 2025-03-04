using System;
using Project.Scripts.Module.System;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic
{
    public class DebugController: MonoBehaviour
    {
        private TreeUpgradeSystem _treeUpgradeSystem;
        private int _count;

        [Inject]
        private void Construct(TreeUpgradeSystem treeUpgradeSystem)
        {
            _treeUpgradeSystem = treeUpgradeSystem;
        }

        private void Start()
        {
            _count = PlayerPrefs.GetInt("ScreenshotsCount", 0); 
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
            if(Input.GetKeyDown(KeyCode.S))
                Screenshot();
        }
#endif
        
        private void Screenshot()
        {
            _count++;
            ScreenCapture.CaptureScreenshot($"screenshot{_count}.png");
            PlayerPrefs.SetInt("ScreenshotsCount", _count);
            Debug.Log("A screenshot was taken!");
        }
    }
}