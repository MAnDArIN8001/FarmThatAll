using System;
using Communication;
using UI.PopUp;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace Building.Factory
{
    [RequireComponent(typeof(FactoryBuilding))]
    public class SteelFactoryCommunicableBuilding : MonoBehaviour, ICommunicable
    {
        [Inject] private PopUpService _popUpService;
        
        [field: SerializeField] public FactoryBuilding SteelFactory { get; private set; }
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }

        private SteelFactoryPopUp _popUp;

        public void Awake()
        {
            SteelFactory = GetComponent<FactoryBuilding>();
        }
        
        public void StartCommunication()
        {
            _popUpService.OpenPopUp(Vector3.zero, out _popUp);
            
            _popUp.transform.name = $"{nameof(SteelFactoryPopUp)}";
            _popUp.transform.localPosition = Vector3.zero;
            _popUp.steelFactory = SteelFactory; 
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<SteelFactoryPopUp>();
        }
    }
}