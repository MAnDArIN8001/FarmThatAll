using System;
using ProcessBuilding.Systems.GrowingSystems;
using Storage.Items;
using UI.ElementCard.Slot;
using UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.PopUp.Variants.Garden.Windows.SubElements
{
    public class ChoiseWindow : AbstractWindow
    {
        [SerializeField] private DropSlot _dropSlot;
        
        [Space, SerializeField] private Button _growingButton;

        private ItemType _itemType;

        private GrowingSystem _growingSystem;

        public void Initialize(GrowingSystem growingSystem)
        {
            _growingSystem = growingSystem;
        }

        private void OnEnable()
        {
            if (_dropSlot is not null)
            {
                _dropSlot.OnAcceptCard += HandleAcceptCard;
            }

            if (_growingButton)
            {
                _growingButton.onClick.AddListener(HandleGrowClick);
            }

            _growingButton.interactable = !_dropSlot.IsFree;
        }

        private void OnDisable()
        {
            if (_dropSlot is not null)
            {
                _dropSlot.OnAcceptCard -= HandleAcceptCard;
            }
            
            if (_growingButton)
            {
                _growingButton.onClick.RemoveListener(HandleGrowClick);
            }
        }

        private void HandleAcceptCard(ItemType itemType)
        {
            _growingButton.interactable = true;
            _itemType = itemType;
        }

        private void HandleGrowClick()
        {
            _growingSystem.SetCulture(_itemType);
        }
    }
}