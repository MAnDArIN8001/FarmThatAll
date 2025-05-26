using Communication;
using ProcessBuilding.Systems.GrowingSystems;
using ProcessBuilding.Systems.ModuleSystems;
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
        [SerializeField] private GrowingSystem _growingSystem;
        [SerializeField] private ModuleSystem _moduleSystem;
        #endregion

        [Space, SerializeField] private Transform _popUpRoot;
        
        [Inject] private PopUpService _popUpService;

        private GardenPopUp _gardenPopUp;
        
        public void StartCommunication()
        {
            _popUpService.OpenPopUp<GardenPopUp>(_popUpRoot.position, out _gardenPopUp);
            
            _gardenPopUp.Initialize(_growingSystem, _moduleSystem);
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<GardenPopUp>();
        }
    }
}