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
         private PopUpService _popUpService = PopUpService.Instance;
        
        [field: SerializeField] public FactoryBuilding Factory { get; private set; }
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }

        [Space, SerializeField] private Transform _popUpRoot;
        
        private FactoryPopUp _popUp;

        private void Awake()
        {
            Factory = GetComponent<FactoryBuilding>();
            
            _popUpService = PopUpService.Instance;
            _popUpRoot = FindAnyObjectByType<CentralPoint>(FindObjectsInactive.Include)?.transform;
        }

        public void StartCommunication()
        {
            if (_popUp is null)
            {
                _popUpService.SimpleOpen(_popUpRoot.position, out _popUp);    
            }

            if (_popUp == null)
            {
                Debug.LogWarning("No factory pop up found");
                
                return;
            }

            _popUp.Setup(Factory);
            _popUp.Open();
            
            _popUp.transform.name = $"{nameof(FactoryPopUp)}";
            _popUp.transform.localPosition = Vector3.zero;
        }

        public void StopCommunication()
        {
            _popUp.Close();
        }
    }
}