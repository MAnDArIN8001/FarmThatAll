using System;
using Communication;
using Cysharp.Threading.Tasks;
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

        private FactoryPopUp _popUp;

        private void Awake()
        {
            Factory = GetComponent<FactoryBuilding>();
        }

        public void StartCommunication()
        {
            _popUpService.OpenPopUp(Vector3.zero, out _popUp);

            if (_popUp == null)
            {
                Debug.LogWarning("No factory pop up found");
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