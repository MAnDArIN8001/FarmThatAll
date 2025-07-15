using System;
using UnityEngine;
using UnityEngine.UI;
using UI.Windows.Variants.TerminalWindows;
using Utiles;
using Utiles.EventSystem;
using Zenject;

namespace UI.PopUp.Variants.Terminal
{
    public class TerminalPopUp : AbstractPopUp
    {
        public override event Action<AbstractPopUp> OnPopUpOpened;
        public override event Action<AbstractPopUp> OnPopUpClosed;

        [SerializeField] private Vector3 _defaultScale = Vector3.one;

        #region Buttons
        [Header("Buttons")] 
        [SerializeField] private Button _closeButton;
        #endregion

        #region UI Elements
        [Header("UI Elements")] 
        [SerializeField] private SellWindow _sellWindow;
        [SerializeField] private ShopWindow _shopWindow;
        #endregion

        private ProcessBuilding.Terminal.Terminal _terminal;

        [Inject] private Storage.Storage _storage;
        [Inject] private LevelManager _levelManager;
        [Inject] private EventBus _eventBus;

        private void Awake()
        {
            transform.localScale = Vector3.zero;
            
            _shopWindow.Initialize(_storage, _levelManager, _eventBus);
        }

        private void OnEnable()
        {
            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(Close);
            }
        }

        private void OnDisable()
        {   
            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveListener(Close);
            }
        }

        public void Initialize(ProcessBuilding.Terminal.Terminal terminal)
        {
            _terminal = terminal;
        }

        public override void Open()
        {
            transform.localScale = _defaultScale;
            
            OnPopUpOpened?.Invoke(this);
            base.Open();
        }

        public override void Close()
        {
            transform.localScale = Vector3.zero;
            
            OnPopUpClosed?.Invoke(this);
            base.Close();
        }
    }
} 