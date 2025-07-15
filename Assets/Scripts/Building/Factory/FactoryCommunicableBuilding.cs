using System;
using Communication;
using Cysharp.Threading.Tasks;
using UI;
using UI.PopUp;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace Building.Factory
{
    [RequireComponent(typeof(FactoryBuilding))]
    public class FactoryCommunicableBuilding : MonoBehaviour, ICommunicable
    {
        [Inject] private PopUpService _popUpService;
        
        [field: SerializeField] public FactoryBuilding Factory { get; private set; }
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }

        [Space, SerializeField] private Transform _popUpRoot;
        
        private FactoryPopUp _popUp;

        private void Awake()
        {
            Factory = GetComponent<FactoryBuilding>();
            
            _popUpRoot ??= FindAnyObjectByType<CentralPoint>(FindObjectsInactive.Include)?.transform;
        }

        public void StartCommunication()
        {
            _popUpService.OpenPopUp(_popUpRoot.position, out _popUp);

            if (_popUp == null)
            {
                Debug.LogWarning("No factory pop up found");
                
                return;
            }
            
            _popUp.transform.name = $"{nameof(FactoryPopUp)}";
            _popUp.transform.localPosition = Vector3.zero;
            _popUp.factory = Factory; 
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<FactoryPopUp>();

            _popUp = null;
        }
    }
}