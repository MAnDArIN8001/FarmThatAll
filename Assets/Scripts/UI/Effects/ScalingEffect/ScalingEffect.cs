using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Effects.ScalingEffect
{
    [Serializable]
    public class ScalingEffect : IDisposable
    {
        [SerializeField] private float _scalingTime;

        [Space, SerializeField] private Ease _scalingEase;

        private Tween _scalingTween;

        public void Play(Vector3 endScale, Transform targetTransform, TweenCallback callback = null)
        {
            Stop();
            
            _scalingTween = targetTransform.DOScale(endScale, _scalingTime).SetEase(_scalingEase);

            if (callback is not null)
            {
                _scalingTween.OnComplete(callback);
            }
        }

        public void Stop()
        {
            if (_scalingTween is not null && _scalingTween.IsActive())
            {
                _scalingTween.Kill();
            }
        }
        
        public void Dispose()
        {
            Stop();
        }
    }
}