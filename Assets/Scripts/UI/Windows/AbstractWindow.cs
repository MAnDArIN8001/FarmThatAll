using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Windows
{
    public abstract class AbstractWindow : MonoBehaviour
    {
        [SerializeField] protected float _scalingTime;

        private Tween _scalingTween;
        
        public virtual void Open(Action callBack = null)
        {
            _scalingTween?.Kill();
            
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            
            _scalingTween = transform.DOScale(Vector3.one, _scalingTime).OnComplete(() =>
            {
                callBack?.Invoke();
            });
        }

        public virtual void Close(Action callBack = null)
        {
            _scalingTween?.Kill();
            
            _scalingTween = transform.DOScale(Vector3.zero, _scalingTime).OnComplete(() =>
            {
                gameObject?.SetActive(false);
                
                callBack?.Invoke();
            });
        }
    }
}