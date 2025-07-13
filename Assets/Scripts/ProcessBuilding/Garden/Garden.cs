using System;
using Communication;
using ProcessBuilding.Systems.GrowingSystems;
using ProcessBuilding.Systems.GrowingSystems.Variants;
using ProcessBuilding.Systems.ModuleSystems;
using UI;
using UI.PopUp.Variants.Garden;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace ProcessBuilding.Garden
{
    public class Garden : MonoBehaviour, ICommunicable
    {
        #region CommunicationViewPoints
        [Header("Communication View Points")]
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }
        #endregion

        #region Systems
        [Header("Systems")]
        [SerializeField] private BaseGrowingSystem _growingSystem;
        #endregion

        [Space, SerializeField] private Transform _popUpRoot;
        
        private PopUpService _popUpService = PopUpService.Instance;

        private GardenPopUp _gardenPopUp;

        private void Awake()
        {
            _popUpRoot = GameObject.FindAnyObjectByType<CentralPoint>(FindObjectsInactive.Include)?.transform;
        }

        public void StartCommunication()
        {
            _popUpService.OpenPopUp<GardenPopUp>(_popUpRoot.position, out _gardenPopUp);
            
            _gardenPopUp.Initialize(_growingSystem);
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<GardenPopUp>();
        }
    }
}