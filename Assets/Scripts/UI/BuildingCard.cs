using System;
using Building;
using UnityEngine;
using UnityEngine.UI;
using Utiles.EventSystem;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class BuildingCard : MonoBehaviour
    {
        [SerializeField] private BuildingData buildingDataConfig;
        
        private EventBus _eventBus;
        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(BuildingSelect);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(BuildingSelect);
        }

        [Inject]
        private void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void BuildingSelect()
        {
            if (!buildingDataConfig)
            {
                Debug.LogError("Building data config is missing!");
                return;
            }

            if (_eventBus == null)
            {
                Debug.LogError("Event bus initialize error!");
                return;
            }
            
            _eventBus.Publish(buildingDataConfig);
        }
    }
}