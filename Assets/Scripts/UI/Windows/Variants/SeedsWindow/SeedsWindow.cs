using Cultures;
using ProcessBuilding.Systems.GrowingSystems;
using UI.PopUp.Variants.Garden.Windows.SubElements;
using UI.Windows;
using UnityEngine;

namespace UI.PopUp.Variants.Garden.Windows
{
    public class SeedsWindow : AbstractWindow
    {
        [SerializeField] private WindowNavigator _navigator;
        
        [Header("Sub Windows")]
        [SerializeField] private ChoiseWindow _choiseWindow;
        [SerializeField] private ProgressWindow _progressWindow;
        
        private GrowingSystem _growingSystem;

        public void Initialize(GrowingSystem growingSystem)
        {
            _growingSystem = growingSystem;
            
            _progressWindow.Initialize(growingSystem);
            _choiseWindow.Initialize(growingSystem);
            
            if (_growingSystem is not null)
            {
                _growingSystem.OnCultureCollect += HandleCultureCollected;
                _growingSystem.OnCultureChanged += HandleCultureChanged;

                if (_growingSystem.Culture is not null)
                {
                    _progressWindow.Open();
                }
            }
        }

        private void OnDisable()
        {
            if (_growingSystem is not null)
            {
                _growingSystem.OnCultureCollect -= HandleCultureCollected;
                _growingSystem.OnCultureChanged -= HandleCultureChanged;
            }
        }

        private void HandleCultureChanged(Culture culture)
        {
            _navigator.SwapWindows(_choiseWindow, _progressWindow);
        }

        private void HandleCultureCollected(float value)
        {
            _navigator.SwapWindows(_progressWindow, _choiseWindow);
        }
    }
}