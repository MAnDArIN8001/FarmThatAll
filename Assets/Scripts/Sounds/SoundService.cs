using System;
using System.Linq;
using ModestTree;
using UnityEngine;
using Utiles;
using Utiles.Pool;
using Random = UnityEngine.Random;

namespace Sounds
{
    public class SoundService : IDisposable
    {
        private readonly SoundDataSetup _musicSounds;
        private readonly SoundDataSetup _sfxSounds;
        
        private readonly AudioMixerSetup _mixersSetup;
        
        private readonly AbstractPool<AudioPlayer> _audioPlayersPool;
        
        public SoundService(AudioPlayer soundPlayer, SoundDataSetup sfxSounds, SoundDataSetup musicSounds, 
            AudioMixerSetup mixerSetupSetup, Transform parent ,int minPoolSize, int maxPoolSize)
        {
            _sfxSounds = sfxSounds;
            _musicSounds = musicSounds;
            
            _mixersSetup = mixerSetupSetup;
            
            _audioPlayersPool = new AbstractPool<AudioPlayer>(soundPlayer, parent, minPoolSize, maxPoolSize);
        }

        private AudioClip GetSoundClip(SoundDataSetup setup, string soundType)
        {
            var clips = setup.SoundDataList.First(x => x.Type == soundType)?.Sound;
            
            if (clips.Count == 0)
            {
                Debug.LogError($"Sound type {soundType} not found in config file!");

                return null;
            }
            
            var clip = clips[Random.Range(0, clips.Count)];

            return clip;
        }

        public AudioPlayer Play(SoundType soundType, string audioClip, bool isLooped = false)
        {
            var clip = GetSoundClip(soundType == SoundType.Sound ? _sfxSounds : _musicSounds, audioClip);

            if (clip == null)
                return null;

            var audioPlayer = _audioPlayersPool.Get();
            
            audioPlayer.Play(clip, isLooped);
            audioPlayer.OnReleased += ReleaseAudioPlayer;
            
            return audioPlayer;
        }

        public AudioPlayer Play(SoundType soundType, string audioClip, Transform parent, float radius, bool isLooped = false)
        {
            var clip = GetSoundClip(soundType == SoundType.Sound ? _sfxSounds : _musicSounds, audioClip);

            if (clip == null)
                return null;
            
            var audioPlayer = _audioPlayersPool.Get();

            if (_mixersSetup.TryGetMixer(soundType, out var mixer))
            {
                audioPlayer.SetMixerGroup(mixer);
            }
            
            audioPlayer.Play(clip, parent, radius, isLooped);
            audioPlayer.OnReleased += ReleaseAudioPlayer;
            
            return audioPlayer;
        }

        private void ReleaseAudioPlayer(AudioPlayer audioPlayer)
        {
            audioPlayer.OnReleased -= ReleaseAudioPlayer;
            
            _audioPlayersPool.Release(audioPlayer);
        }

        public void Dispose()
        {
            _audioPlayersPool.Dispose();
        }
    }
}