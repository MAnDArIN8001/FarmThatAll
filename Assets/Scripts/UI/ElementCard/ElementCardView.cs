using System;
using DG.Tweening;
using TMPro;
using UI.Effects.ScalingEffect;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ElementCard
{
    public class ElementCardView : MonoBehaviour
    {
        private Vector3 _defaultScale;
        
        [SerializeField] private ScalingEffect _scalingEffect;
        
        [Space, SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _counter;

        [Space, SerializeField] private Image _icon;

        public void Initialize(string title, string count, Sprite icon)
        {
            _title.text = title;
            _counter.text = count;

            _icon.sprite = icon;

            _defaultScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void Show(TweenCallback callBack = null)
        {
            _scalingEffect.Play(_defaultScale, transform, callBack);
        }

        public void Hide(TweenCallback callBack = null)
        {
            _scalingEffect.Play(Vector3.zero, transform, callBack);
        }

        public void UpdateCounter(string newCount)
        {
            _counter.text = newCount;
        }
    }
}