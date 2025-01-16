using Project.Scripts.Audio;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Project
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private AudioController _audioController;
        [SerializeField] private AudioVolume _audioVolume;
        
        public override void InstallBindings()
        {
            BindAudioVolume();
            BindAudioController();
        }

        private void BindAudioVolume()
        {
            Container.Bind<AudioVolume>().FromInstance(_audioVolume).AsSingle();
            _audioVolume.Init();
        }

        private void BindAudioController()
        {
            var audioController = Container.InstantiatePrefab(_audioController).GetComponent<AudioController>();
            Container.Bind<AudioController>().FromInstance(audioController).AsSingle();
        }
    }
}