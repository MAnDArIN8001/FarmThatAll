using System.Collections.Generic;
using Building;
using UnityEngine;
using UnityEngine.UI;
using Utiles.EventSystem;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class BuildingsShop : MonoBehaviour
    {
        private bool _shopOpenState;
        
        [SerializeField] private Button shopToggleButton;
        
        [SerializeField] private GameObject cardPrefab;
        
        [SerializeField] private List<BuildingData> buildingDataList;
        
        private CanvasGroup _canvasGroup;
        
        private List<GameObject> _buildingsCards = new List<GameObject>();
        
        [Inject] private EventBus _eventBus;
        
        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _eventBus.Subscribe<BuildingData>(BuildingSelectedHandler);
            
            if (!cardPrefab)
            {
                Debug.LogWarning("Card prefab not set");
                return;
            }

            if (!cardPrefab.TryGetComponent(out BuildingCard card))
            {
                Debug.LogWarning("Prefab does not contain BuildingCard component");
                return;
            }

            card.buildingDataConfig = null;
            
            if (shopToggleButton)
            {
                shopToggleButton.onClick.AddListener(ToggleShop);
            }
        }

        private void OnDisable()
        {
            if (shopToggleButton)
            {
                shopToggleButton.onClick.RemoveListener(ToggleShop);
            }
        }

        private void ToggleShop()
        {
            _shopOpenState = !_shopOpenState;

            if (_shopOpenState)
            {
                foreach (var buildingData in buildingDataList)
                {
                    _buildingsCards.Add(CreateBuildingCard(buildingData));
                }
                _canvasGroup.alpha = 1;
                return;
            }

            _canvasGroup.alpha = 0;
            foreach (var buildingsCard in _buildingsCards)
            {
                Destroy(buildingsCard);
            }
            _buildingsCards.Clear();
        }

        private GameObject CreateBuildingCard(BuildingData buildingData)
        {
            cardPrefab.TryGetComponent(out BuildingCard cardComponent);
            
            cardComponent.buildingDataConfig = buildingData;
            
            var card = Instantiate(cardPrefab, transform);
            
            card.TryGetComponent(out cardComponent);
            
            if (_eventBus != null)
            {
                cardComponent.Initialize(_eventBus);
            }
            
            card.name = $"{buildingData.Name.ToLower()}_card";
            
            return card;
        }


        private void BuildingSelectedHandler(BuildingData buildingData)
        {
            ToggleShop();
        }
    }
}