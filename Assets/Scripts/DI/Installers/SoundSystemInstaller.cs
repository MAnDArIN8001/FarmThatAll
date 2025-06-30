using Sounds;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DI
{
    public class SoundSystemInstaller : MonoInstaller
    {
        private SoundService _soundService;
        
        [SerializeField] private SoundDataSetup sfxDataSetup;
        [SerializeField] private SoundDataSetup musicDataSetup;
        
        [SerializeField] private AudioMixerSetup mixerSetup;
        
        [SerializeField] private AudioPlayer audioPlayer;
        
        [SerializeField] private int minPoolSize = 1;
        [SerializeField] private int maxPoolSize = 30;

        public override void InstallBindings()
        {
            _soundService = new SoundService(audioPlayer, sfxDataSetup, musicDataSetup,
                mixerSetup, transform, minPoolSize, maxPoolSize);
            
            Container.Bind<SoundService>().FromInstance(_soundService).AsSingle().NonLazy();
        }
        
        private void OnDestroy()
        {
            _soundService.Dispose();
        }
    }
}