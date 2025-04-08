using Building;
using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utiles.EventSystem;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class BuildingCard : MonoBehaviour
    {
        public BuildingData buildingDataConfig;
        
        [SerializeField] private TMP_Text buildingNameText;
        [SerializeField] private TMP_Text buildingDescriptionText;
        [SerializeField] private TMP_Text buildingPriceText;
        
        [SerializeField] private Image buildingSprite; 
        
        private Button _buildingCardButton;
        
        private EventBus _eventBus;
        
        private SoundService _soundService;
        
        private void OnEnable()
        {
            _buildingCardButton = GetComponent<Button>();
            
            if (!buildingDataConfig)
            {
                Debug.LogError($"BuildingDataConfig is null, setup cancelled");
                return;
            }

            if (!ValidateBuildingCard())
            {
                Debug.LogError($"Card will not setup until BuildingCard is valid");
                return;
            }

            SetupBuildingCard(buildingDataConfig);
        }

        private void OnDisable()
        {
            _buildingCardButton.onClick.RemoveListener(BuildingSelect);
        }
        
        public void Initialize(EventBus eventBus, SoundService soundService)
        {
            _eventBus = eventBus;
            
            _soundService = soundService;
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
            
            _soundService.Play2DSfx(SoundType.UIClick, 1f);
            _eventBus.Publish(buildingDataConfig);
        }

        private bool ValidateBuildingCard()
        {
            if (!_buildingCardButton)
            {
                Debug.LogError($"BuildingCardButton is null");
                return false;
            }
            
            if (!buildingNameText)
            {
                Debug.LogError($"BuildingNameText is null");
                return false;
            }
            
            if (!buildingDescriptionText)
            {
                Debug.LogError($"BuildingDescriptionText is null");
                return false;
            }
            
            if (!buildingPriceText)
            {
                Debug.LogError($"BuildingPriceText is null");
                return false;
            }
            
            if (!buildingSprite)
            {
                Debug.LogError($"BuildingNameText is null");
                return false;
            }
            
            return true;
        }

        private void SetupBuildingCard(BuildingData buildingData)
        {
            _buildingCardButton.onClick.AddListener(BuildingSelect);
            
            buildingNameText.text = buildingData.Name;
            buildingDescriptionText.text = buildingData.Description;
            buildingPriceText.text = buildingData.Price.ToString();
            
            buildingSprite.sprite = buildingData.Sprite;
        }
    }
}