using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Utiles.Pool;

namespace Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IPoolable
    {
        private AudioSource _audioSource;
        
        private Coroutine _waitCoroutine;

        private YieldInstruction _waitDuration;
        
        public event Action<AudioPlayer> OnReleased;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void OnDisable()
        {
            transform.position = Vector3.zero;
        }

        public void SetMixerGroup(AudioMixerGroup mixerGroup)
        {
            _audioSource.outputAudioMixerGroup = mixerGroup;
        }
        
        public void Play(AudioClip clip, Transform parent, float radius, bool isLooped)
        {
            _audioSource.transform.SetParent(parent);
            _audioSource.transform.localPosition = Vector3.zero;
            
            _audioSource.spatialize = true;
            _audioSource.spatialBlend = 1;
            
            _audioSource.clip = clip;
            _audioSource.loop = isLooped;
            
            _audioSource.maxDistance = radius;
            
            _audioSource.Play();
            
            if (!isLooped)
            {
                _waitCoroutine = StartCoroutine(WaitingCoroutine(clip.length));
            }
        }
        
        public void Play(AudioClip clip, bool isLooped)
        {
            _audioSource.transform.SetParent(null);
            _audioSource.transform.localPosition = Vector3.zero;
            
            _audioSource.spatialize = false;
            _audioSource.spatialBlend = 0;
            
            _audioSource.clip = clip;
            _audioSource.loop = isLooped;
            
            _audioSource.Play();
            
            if (!isLooped)
            {
               _waitCoroutine = StartCoroutine(WaitingCoroutine(clip.length));
            }
        }

        public void StopSound()
        {
            OnReleased?.Invoke(this);
        }
        
        public void Release()
        {
            _audioSource.Stop();

            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
                
                _waitCoroutine = null;
            }
            
            SetMixerGroup(null);
            
            _audioSource.transform.localPosition = Vector3.zero;
            
            _audioSource.clip = null;
            _audioSource.loop = false;
            
            _audioSource.spatialize = false;
            _audioSource.spatialBlend = 0f;
        }

        private IEnumerator WaitingCoroutine(float duration)
        {
            _waitDuration ??= new WaitForSeconds(duration);
            
            yield return _waitDuration;
            
            OnReleased?.Invoke(this);
        }
    }
}