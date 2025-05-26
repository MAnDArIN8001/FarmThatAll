using System;
using Zenject;
using ProcessBuilding.Systems.GrowingSystems;
using ProcessBuilding.Systems.ModuleSystems;
using UI.Effects.ScalingEffect;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp.Variants.Garden
{
    public class GardenPopUp : AbstractPopUp
    {
        public override event Action<AbstractPopUp> OnPopUpOpened;
        public override event Action<AbstractPopUp> OnPopUpClosed;

        [Space, SerializeField] private Vector3 _defaultScale;

        [Space, SerializeField] private ScalingEffect _scalingEffect;

        #region Buttons

        [Header("Buttons")] 
        [SerializeField] private Button _closeButton;

        #endregion

        [Inject] private Storage.Storage _storage;

        private GrowingSystem _growingSystem;
        private ModuleSystem _moduleSystem;

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            if (_closeButton is not null)
            {
                _closeButton.onClick.AddListener(Close);
            }
        }

        private void OnDisable()
        {   
            if (_closeButton is not null)
            {
                _closeButton.onClick.RemoveListener(Close);
            }
        }

        public void Initialize(GrowingSystem growingSystem, ModuleSystem moduleSystem)
        {
            _growingSystem = growingSystem;
            _moduleSystem = moduleSystem;
        }

        public override void Open()
        {
            _scalingEffect.Play(_defaultScale, transform);
            
            OnPopUpOpened?.Invoke(this);
        }

        public override void Close()
        {
            _scalingEffect.Play(Vector3.zero, transform, () => OnPopUpClosed?.Invoke(this));
        }
    }
}