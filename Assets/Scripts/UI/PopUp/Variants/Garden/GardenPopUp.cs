using System;
using Zenject;
using ProcessBuilding.Systems.GrowingSystems;
using Storage;
using UI.Effects.ScalingEffect;
using UI.ElementCard.Slot;
using UI.PopUp.Variants.Garden.Windows;
using UI.ToolBar;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using ProcessBuilding.Garden;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.VFX;

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
        [SerializeField] private Button _destroyButton;

        #endregion

        [Header("Windows")] 
        [SerializeField] private SeedsWindow _seedsWindow;
        [SerializeField] private AbstractWindow _destroyWindow;

        [Header("Elements")] 
        [SerializeField] private CardsBar _cards;

        [Header("Effects")]
        [SerializeField] private VisualEffect _destroyEffect;
        
        [Space, SerializeField] private DropSlot _slot;

        private Storage.Storage _storage;

        private GrowingSystem _growingSystem;
        private ProcessBuilding.Garden.Garden _garden;

        [Inject]
        private void InitializeByDI(Storage.Storage storage)
        {
            _storage = storage;
            
            _cards.Initialize(_storage);
        }
        
        public void Initialize(GrowingSystem growingSystem, ProcessBuilding.Garden.Garden garden)
        {
            _garden = garden;
            _growingSystem = growingSystem;
            _growingSystem.OnCultureCollect += HandleResourceTake;

            if (_growingSystem.Culture is not null )
            {
                _slot.InsertCard(growingSystem.Culture.CultureType);
            }

            _seedsWindow.Initialize(growingSystem);
        }

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

            if(_destroyButton is not null)
            {
                _destroyButton.onClick.AddListener(DestroyGarden);
            }
        }

        private void OnDisable()
        {   
            if (_closeButton is not null)
            {
                _closeButton.onClick.RemoveListener(Close);
            }
        }

        private void OnDestroy()
        {
            if (_growingSystem is not null)
            {
                _growingSystem.OnCultureCollect -= HandleResourceTake;
            }
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

        private void HandleResourceTake(float count)
        {
            Debug.Log("Resource taken");
            
            _slot.Clear();
        }

        private void DestroyGarden()
        {
            Close();
            Instantiate(_destroyEffect, _garden.transform.position, Quaternion.identity).Play();
            Destroy(_garden.gameObject);
        }
    }
}