using System;
using UnityEngine;
using Utiles;
using Utiles.Pool;

namespace Sounds
{
    public class SoundService : IDisposable
    {
        private readonly SoundDataSetup _musicSounds;
        private readonly SoundDataSetup _sfxSounds;
        
        private readonly AbstractPool<AudioPlayer> _soundPlayersPool;
        
        public SoundService(AudioPlayer audioPlayer, SoundDataSetup sfxSounds, SoundDataSetup musicSounds,
            Transform parent ,int minPoolSize, int maxPoolSize)
        {
            _sfxSounds = sfxSounds;
            _musicSounds = musicSounds;
            
            _soundPlayersPool = new AbstractPool<AudioPlayer>(audioPlayer, parent, minPoolSize, maxPoolSize);
        }

        public void Play2DSfx(SoundType soundType, float volume) => 
            Play2DSound(GetSoundClip(_sfxSounds, soundType), volume);

        public void Play2DMusic(SoundType soundType, float volume) => 
            Play2DSound(GetSoundClip(_musicSounds, soundType), volume);

        public AudioPlayer Play2DSfxLooped(SoundType soundType, float volume) => 
            Play2DSoundLooped(GetSoundClip(_sfxSounds, soundType), volume);

        public AudioPlayer Play2DMusicLooped(SoundType soundType, float volume) => 
            Play2DSoundLooped(GetSoundClip(_musicSounds, soundType), volume);

        public void Play3DSfx(SoundType soundType, Transform soundSource, float radius, float volume) => 
            Play3DSound(GetSoundClip(_sfxSounds, soundType), soundSource, radius, volume);

        public void Play3DMusic(SoundType soundType, Transform soundSource, float radius, float volume) => 
            Play3DSound(GetSoundClip(_musicSounds, soundType), soundSource, radius, volume);

        public AudioPlayer Play3DSfxLooped(SoundType soundType, Transform soundSource, float radius, float volume) => 
            Play3DSoundLooped(GetSoundClip(_sfxSounds, soundType), soundSource, radius, volume);

        public AudioPlayer Play3DMusicLooped(SoundType soundType, Transform soundSource, float radius, float volume) => 
            Play3DSoundLooped(GetSoundClip(_musicSounds, soundType), soundSource, radius, volume);

        private AudioClip GetSoundClip(SoundDataSetup setup, SoundType soundType)
        {
            var clip = setup.SoundDataList.Find(x => x.Type == soundType)?.Sound;
            
            if (clip == null)
            {
                Debug.LogError($"Sound type {soundType} not found in config file!");

                return null;
            }

            return clip;
        }
        
        private void Play2DSound(AudioClip clip, float volume)
        {
            if (clip == null) return;
            
            var sound2DPlayer = _soundPlayersPool.Get();
            
            sound2DPlayer.Play(clip, volume, false);
                
            sound2DPlayer.OnReleased += ReleaseAudioPlayer;
        }

        private void Play3DSound(AudioClip clip, Transform parent,
            float radius = 10f, float volume = 1f)
        {
            if (clip == null) return;
            
            var sound3DPlayer = _soundPlayersPool.Get();

            sound3DPlayer.Play(clip, parent, volume, radius, false);

            sound3DPlayer.OnReleased += ReleaseAudioPlayer;
        }

        private AudioPlayer Play2DSoundLooped(AudioClip clip, float volume)
        {
            if (clip == null) return null;
            
            var sound2DPlayer = _soundPlayersPool.Get();
            
            sound2DPlayer.Play(clip, volume, true);
                
            sound2DPlayer.OnReleased += ReleaseAudioPlayer;

            return sound2DPlayer;
        }
        
        private AudioPlayer Play3DSoundLooped(AudioClip clip, Transform parent,
            float radius = 10f, float volume = 1f)
        {
            if (clip == null) return null;
            
            var sound3DPlayer = _soundPlayersPool.Get();
            
            sound3DPlayer.Play(clip, parent, volume, radius, true);

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