using System;
using Project.Scripts.GameLogic.Entity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Project.Scripts.GameLogic
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        
        private ColorAdjustments _colorAdjustments;
        private Tree _tree;

        [Inject]
        private void Construct(Tree tree)
        {
            _tree = tree;
        }

        private void Awake()
        {
            _tree.OnHealthChange += OnHealthChange;
        }

        private void Start()
        {
            _volume.profile.TryGet(out ColorAdjustments colorAdjustments);
            _colorAdjustments = colorAdjustments;
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        private void OnHealthChange(OnHealthChangeArgs<Tree> obj)
        {
            _colorAdjustments.saturation.Override(Mathf.Lerp(-30, 0, obj.Object.CurrentHealth / obj.Object.MaxHealth));
        }
    }
}