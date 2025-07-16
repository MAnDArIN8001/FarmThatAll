using System.Collections.Generic;
using Communication;
using UI;
using UI.PopUp.Variants.Converter;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace Building.Converter
{
    [RequireComponent(typeof(ConverterBuilding))]
    public class ConverterCommunicableBuilding : MonoBehaviour, ICommunicable
    {
        private PopUpService _popUpService;

        private ConverterBuilding _converter;
        
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }
        
        [Space, SerializeField] private Transform _popUpRoot;

        [SerializeField] private List<Recipe> _recipes = new();
        
        private ConverterPopUp _popUp;

        private void Awake()
        {
            _converter = GetComponent<ConverterBuilding>();   
            
            _popUpRoot = FindAnyObjectByType<CentralPoint>(FindObjectsInactive.Include).transform;
            _popUpService = PopUpService.Instance;
        }
        
        public void StartCommunication()
        {
            if (_popUp is null)
            {
                _popUpService.SimpleOpen(_popUpRoot.position, out _popUp);    
            }
            
            if (_popUp == null)
            {
                Debug.Log("Failed to open pop up");
                
                return;
            }
            
            _popUp.Setup(_converter, _recipes);
            _popUp.Open();
            
            _popUp.transform.name = $"{nameof(ConverterPopUp)}";
            _popUp.transform.localPosition = Vector3.zero;
        }

        public void StopCommunication()
        {
            _popUp.Close();
        }
    }
}