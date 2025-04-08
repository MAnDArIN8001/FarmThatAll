using System;
using UnityEngine;
using Utiles;
using Utiles.Pool;

namespace Sounds
{
    public class SoundService : IDisposable
    {
        private SoundDataConfig _musicSounds;
        private SoundDataConfig _sfxSounds;
        
        private AbstractPool<AudioPlayer> _soundPlayersPool;
        
        public SoundService(AudioPlayer audioPlayer, SoundDataConfig sfxSounds, SoundDataConfig musicSounds,
            Transform parent ,int minPoolSize, int maxPoolSize)
        {
            _sfxSounds = sfxSounds;
            _musicSounds = musicSounds;
            
            _soundPlayersPool = new AbstractPool<AudioPlayer>(audioPlayer, parent, minPoolSize, maxPoolSize);
        }

        public void Play2DSfx(SoundType soundType, float volume)
        {
            var sfxClip = _sfxSounds.SoundDataList.Find(x => x.Type == soundType).Sound;

            if (!sfxClip)
            {
                Debug.LogError($"Sound type {soundType} not found in config file!");
            }
            
            Play2DSound(sfxClip, volume);
        }
        
        public void Play2DMusic(SoundType soundType, float volume)
        {
            var musicClip = _musicSounds.SoundDataList.Find(x => x.Type == soundType).Sound;

            if (!musicClip)
            {
                Debug.LogError($"Sound type {soundType} not found in config file!");
            }
            
            Play2DSound(musicClip, volume);
        }
        
        private void Play2DSound(AudioClip clip, float volume)
        {
            var sound2DPlayer = _soundPlayersPool.Get();
            
            sound2DPlayer.PlayEffect(clip, volume, false);
                
            sound2DPlayer.OnReleased += ReleaseAudioPlayer;
        }

        private void Play3DSound(Transform parent, AudioClip clip,
            float radius = 10f, float volume = 1f)
        {
            var sound3DPlayer = _soundPlayersPool.Get();

            sound3DPlayer.PlayEffect(clip, parent, volume, radius, false);

            sound3DPlayer.OnReleased += ReleaseAudioPlayer;
        }

        private AudioPlayer Play2DSoundLooped(AudioClip clip, float volume)
        {
            var sound2DPlayer = _soundPlayersPool.Get();
            
            sound2DPlayer.PlayEffect(clip, volume, true);
                
            sound2DPlayer.OnReleased += ReleaseAudioPlayer;

            return sound2DPlayer;
        }
        
        private AudioPlayer Play3DSoundLooped(Transform parent, AudioClip clip,
            float radius = 10f, float volume = 1f)
        {
            var sound3DPlayer = _soundPlayersPool.Get();
            
            sound3DPlayer.PlayEffect(clip, parent, volume, radius, true);

            sound3DPlayer.OnReleased += ReleaseAudioPlayer;
            
            return sound3DPlayer;
        }

        private void ReleaseAudioPlayer(AudioPlayer audioPlayer)
        {
            audioPlayer.OnReleased -= ReleaseAudioPlayer;
            
            _soundPlayersPool.Release(audioPlayer);
        }

        public void Dispose()
        {
            _soundPlayersPool.Dispose();
        }
    }
}