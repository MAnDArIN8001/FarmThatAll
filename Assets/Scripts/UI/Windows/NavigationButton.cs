using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    [RequireComponent(typeof(Button))]
    public class NavigationButton : MonoBehaviour
    {
        [SerializeField] private WindowNavigator _windowNavigator;

        [Space, SerializeField] private AbstractWindow _targetWindow;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_button is not null)
            {
                _button.onClick.AddListener(Navigate);
            }
        }

        private void OnDisable()
        {
            if (_button is not null)
            {
                _button.onClick.RemoveListener(Navigate);
            }
        }

        private void Navigate()
        {
            _windowNavigator.NavigateTo(_targetWindow);
        }
    }
}