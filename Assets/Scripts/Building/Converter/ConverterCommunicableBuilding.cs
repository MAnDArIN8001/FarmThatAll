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
        [Inject] private PopUpService _popUpService;

        private ConverterBuilding _converter;
        
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }
        
        [Space, SerializeField] private Transform _popUpRoot;
        
        private ConverterPopUp _popUp;

        private void Awake()
        {
            _converter = GetComponent<ConverterBuilding>();   
            
            _popUpRoot ??= FindAnyObjectByType<CentralPoint>(FindObjectsInactive.Include)?.transform;
        }
        
        public void StartCommunication()
        {
            _popUpService.OpenPopUp(_popUpRoot.position, out _popUp);
            
            if (_popUp == null)
            {
                Debug.Log("Failed to open pop up");
                
                return;
            }
            
            _popUp.Setup(_converter);
            
            _popUp.transform.name = $"{nameof(ConverterPopUp)}";
            _popUp.transform.localPosition = Vector3.zero;
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<ConverterPopUp>();

            _popUp = null;
        }
    }
}