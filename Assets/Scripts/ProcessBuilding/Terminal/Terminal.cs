using System;
using Communication;
using UI;
using UI.PopUp.Variants.Terminal;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace ProcessBuilding.Terminal
{
    public class Terminal : MonoBehaviour, ICommunicable
    {
        #region CommunicationViewPoints
        [Header("Communication View Points")]
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }
        #endregion

        private Transform _popUpRoot;
        
        [Inject] private PopUpService _popUpService;

        private TerminalPopUp _terminalPopUp;

        private void Awake()
        {
            _popUpRoot = GameObject.FindAnyObjectByType<CentralPoint>(FindObjectsInactive.Include)?.transform;
        }

        public void StartCommunication()
        {
            _popUpService.OpenPopUp<TerminalPopUp>(_popUpRoot.position, out _terminalPopUp);
            
            _terminalPopUp.Initialize(this);
        }

        public void StopCommunication()
        {
            _popUpService.ClosePopUp<TerminalPopUp>();
        }
    }
} 