using System;
using System.Collections.Generic;
using NUnit.Framework;
using Player.CameraControls;
using UnityEngine;
using Utiles;
using Utiles.EventSystem;
using CameraType = Player.CameraControls.CameraType;

namespace Ship
{
    public class UpgradeSystem : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [Space, SerializeField] private List<UpgradeData> _upgradeData;
        
        [Space, SerializeField] private CameraSystem _cameraSystem;
        
        private LevelManager _levelManager = LevelManager.Instance;

        private EventBus _eventBus = EventBus.Instance;

        private void OnEnable()
        {
            if (_levelManager is not null)
            {
                _levelManager.OnLevelChanged += HandleLevelChanged;
            }
        }

        private void OnDisable()
        {
            if (_levelManager is not null)
            {
                _levelManager.OnLevelChanged -= HandleLevelChanged;
            }
        }

        private void HandleLevelChanged(int newLevel)
        {
            var upgradeData = _upgradeData[newLevel];

            foreach (var itemToUpdate in upgradeData.ItemsToUpdate)
            {
                switch (itemToUpdate.UpgradeType)
                {
                    case UpgradeType.Show:
                        itemToUpdate.Element.SetActive(true);
                        break;
                    
                    case UpgradeType.Hide:
                        itemToUpdate.Element.SetActive(false);
                        break;
                }
            }

            if (newLevel == _upgradeData.Count-1)
            {
                _cameraSystem.SetCamera(CameraType.Ship);
                
                _animator.SetTrigger("End");
            }
        }
    }

    [Serializable]
    public class UpgradeData
    {
        [field: SerializeField] public int Level { get; private set; }

        [SerializeField, Space] private List<ChangeData> _itemsToHide = new();
        
        public IReadOnlyList<ChangeData> ItemsToUpdate => _itemsToHide;
    }

    [Serializable]
    public class ChangeData
    {
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
        
        [field: SerializeField] public GameObject Element { get; private set; }
    }

    public enum UpgradeType
    {
        Hide,
        Show
    }
}