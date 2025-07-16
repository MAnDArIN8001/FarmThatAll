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
        [SerializeField] private Button _openDestroyWindowButton;
        [SerializeField] private Button _closeDestroyWindowButton;

        #endregion

        [Header("Windows")] 
        [SerializeField] private SeedsWindow _seedsWindow;
        [SerializeField] private DestroyWindow _destroyWindow;

        [Header("Elements")] 
        [SerializeField] private CardsBar _cards;
        [SerializeField] private GameObject _tab;
        
        [Space, SerializeField] private DropSlot _slot;

        private Storage.Storage _storage;

        private GrowingSystem _growingSystem;

        [Inject]
        private void InitializeByDI(Storage.Storage storage)
        {
            _storage = storage;
            
            _cards.Initialize(_storage);
        }
        
        public void Initialize(GrowingSystem growingSystem, ProcessBuilding.Garden.Garden garden)
        {
            _growingSystem = growingSystem;
            _growingSystem.OnCultureCollect += HandleResourceTake;
            
            Debug.Log("Follow");

            if (_growingSystem.Culture is not null)
            {
                _slot.InsertCard(growingSystem.Culture.CultureType);
            }

            _seedsWindow.Initialize(growingSystem);
            _destroyWindow.Initialize(garden, this);
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

            if(_openDestroyWindowButton is not null)
            {
                _openDestroyWindowButton.onClick.AddListener(OpenDestroyWindow);
            }

            if(_closeDestroyWindowButton is not null)
            {
                _closeDestroyWindowButton.onClick.AddListener(CloseDestroyWindow);
            }

            /*
            if (_growingSystem is not null)
            {
                _growingSystem.OnCultureCollect += HandleResourceTake;
            }
            */
           
            if (_growingSystem?.Culture is not null
                && _slot.IsFree)
            {
                _slot.InsertCard(_growingSystem.Culture.CultureType);
            }
        }

        private void OnDisable()
        {   
            if (_closeButton is not null)
            {
                _closeButton.onClick.RemoveListener(Close);
            }
            
            if(_openDestroyWindowButton is not null)
            {
                _openDestroyWindowButton.onClick.RemoveListener(OpenDestroyWindow);
            }

            if(_closeDestroyWindowButton is not null)
            {
                _closeDestroyWindowButton.onClick.RemoveListener(CloseDestroyWindow);
            }

            if (_growingSystem is not null)
            {
                _growingSystem.OnCultureCollect -= HandleResourceTake;
            }
        }

        public override void Open()
        {
            base.Open();
            
            _scalingEffect.Play(_defaultScale, transform);
            OnPopUpOpened?.Invoke(this);
        }

        public override void Close()
        {
            _scalingEffect.Play(Vector3.zero, transform, () =>
            {
                OnPopUpClosed?.Invoke(this);
                
                base.Close();
            });
        }

        private void HandleResourceTake(float count)
        {
            _slot.Clear();
            
            Debug.Log("Clear");
        }

        private void OpenDestroyWindow()
        {
            _destroyWindow.Open();
            
            Vector3 targetPosition = _openDestroyWindowButton.transform.position;
            StartCoroutine(MoveOverTime(_tab.transform, targetPosition, 0.5f));
        }

        public void CloseDestroyWindow()
        {
            _destroyWindow.Close();

            Vector3 targetPosition = _closeDestroyWindowButton.transform.position;
            StartCoroutine(MoveOverTime(_tab.transform, targetPosition, 0.5f));
        }

        private System.Collections.IEnumerator MoveOverTime(Transform transform,Vector3 target, float time)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < time)
            {
                transform.position = Vector3.Lerp(start, target, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
        }
    }
}