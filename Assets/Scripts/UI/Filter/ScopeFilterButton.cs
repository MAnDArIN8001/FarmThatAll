using System;
using Storage.Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Filter
{
    [RequireComponent(typeof(Button))]
    public class ScopeFilterButton : MonoBehaviour
    {
        [SerializeField] private ItemScope _targetFilter;

        [Space, SerializeField] private ScopeFilterManager _targetFilterManager;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_button is not null)
            {
                _button.onClick.AddListener(SetFilter);
            }
        }

        private void OnDisable()
        {
            if (_button is not null)
            {
                _button.onClick.RemoveListener(SetFilter);
            }
        }

        private void SetFilter()
        {
            _targetFilterManager.SetFilter(_targetFilter);
        }
    }
}