using System;
using Communication;
using Cysharp.Threading.Tasks;
using UI.PopUp;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace Building.Factory
{
    [RequireComponent(typeof(SteelFactoryBuilding))]
    public class SteelFactoryCommunicableBuilding : MonoBehaviour, ICommunicable
    {
        [Inject] private PopUpService _popUpService;
        
        [field: SerializeField] public SteelFactoryBuilding SteelSteelFactory { get; private set; }
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }

        private SteelFactoryPopUp _popUp;

        private void Awake()
        {
            SteelSteelFactory = GetComponent<SteelFactoryBuilding>();
        }

        private void Start()
        {
            Debug.Log("Test");
        }
        
        public void StartCommunication()
        {
            _popUpService.OpenPopUp(Vector3.zero, out _popUp);
            
            _popUp.transform.name = $"{nameof(SteelFactoryPopUp)}";
            _popUp.transform.localPosition = Vector3.zero;
            _popUp.steelSteelFactory = SteelSteelFactory; 
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<SteelFactoryPopUp>();

            _popUp = null;
        }
    }
}