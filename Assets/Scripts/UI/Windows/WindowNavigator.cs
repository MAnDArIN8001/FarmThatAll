using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Windows
{
    public class WindowNavigator : MonoBehaviour
    {
        public event Action<AbstractWindow> OnWindowChanged;

        [SerializeField] private AbstractWindow _defaultWindow;
        private AbstractWindow _currentWindow;

        private void Awake()
        {
            NavigateTo(_defaultWindow);
        }

        public void NavigateTo(AbstractWindow abstractWindow)
        {
            if (_currentWindow == abstractWindow)
            {
                return;
            }

            if (_currentWindow is not null)
            {
                _currentWindow.Close(() =>
                {
                    _currentWindow = abstractWindow;
                    _currentWindow.Open();
                });
            }
            else
            {
                _currentWindow = abstractWindow;
                _currentWindow.Open();
            }
            
            OnWindowChanged?.Invoke(_currentWindow);
        }

        public void SwapWindows(AbstractWindow from, AbstractWindow to)
        {
            from.Close(() =>
            {
                to.Open();
            });
        }
    }
}