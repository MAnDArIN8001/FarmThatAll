using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.Audio;
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
        
        private Dictionary<SoundType, AudioMixerGroup> _audioMixerDictionary = new ();
        
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
            if (!setup.TryGetSoundData(soundType, out var soundData))
            {
                Debug.LogWarning($"Sound type {soundType} not found in config file!");
                
                return null;
            }
            
            if (soundData.Sound.Count == 0)
            {
                Debug.LogWarning($"Sound type {soundType} has 0 audio clips");

                return null;
            }
            
            var clip = soundData.Sound[Random.Range(0, soundData.Sound.Count)];

            return clip;
        }

        public AudioPlayer Play(SoundType soundType, string audioClip, bool isLooped = false)
        {
            var clip = GetSoundClip(soundType == SoundType.Sound ? _sfxSounds : _musicSounds, audioClip);

            if (clip == null)
                return null;

            var audioPlayer = _audioPlayersPool.Get(); 
            
            if (!_audioMixerDictionary.TryGetValue(soundType, out var mixer))
            {
                _mixersSetup.TryGetMixer(soundType, out mixer);
                
                _audioMixerDictionary.Add(soundType, mixer);
            }

            if (mixer != null)
            {
                audioPlayer.SetMixerGroup(mixer);
            }
            
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